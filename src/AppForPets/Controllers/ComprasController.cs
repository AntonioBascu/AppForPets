using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.CompraViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AppForPets.Controllers
{
    [Authorize (Roles ="Cliente")]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index(string SearchString)
        {
            //if (!String.IsNullOrEmpty(SearchString))
            //{
            //    var animales = _context.Animal.Include(m => m.Tipo).
            //        Where(s => s.Raza.Contains(SearchString)).
            //        OrderBy(m => m.Raza);
            //    return View(await animales.ToListAsync());
            //}
            //else
            //    return View(await _context.Movie.Include(m => m.Genre).
            //        OrderBy(m => m.Title).ToListAsync());
            return View(_context.Compra.Include(p => p.Cliente).Where(p => p.Cliente.UserName.Equals(User.Identity.Name)).OrderByDescending(p => p.FechaCompra).ToList());

        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //selectArticulos.Articulos = _context.Articulo.Include(m => m.Categoria).Include(f => f.AlquilerArticulos).ThenInclude(f1 => f1.Alquiler).Where(m => m.CantidadAlquiler > 0 && !(m.AlquilerArticulos.Any(m => m.Alquiler.FechaFin.CompareTo(fechaInicio) >= 0 && m.Alquiler.FechaInicio.CompareTo(fechaFin) < 0)));
            var compra =  _context.Compra.Include(p => p.L_Compras).ThenInclude<Compra, L_Compra, Tipo>(p => p.Animal.Tipo).Include(p =>p.Cliente).Where(p => p.CompraID == id).ToList();
           
            if (compra.Count == 0)
            {
                return NotFound();
            }

            return View(compra.First());
        }

        // GET: Compras/Create
        public IActionResult Create(SelectedAnimalesForCompraViewModel selectedAnimales)
        {

            CompraCreateViewModel compra = new CompraCreateViewModel();
            compra.l_compras = new List<L_compraViewModel>();

            if (selectedAnimales.IdsToAdd == null)
            {
                ModelState.AddModelError("AnimalNoSelected", "Debe seleccionar al menos un animal para comprar, por favor");
            }
            else
                compra.l_compras = _context.Animal.Include(animal => animal.Tipo)
                    .Select(animal => new L_compraViewModel()
                    {
                        AnimalID = animal.AnimalID,
                        raza = animal.Tipo.Raza,
                        PrecioCompra = animal.Precio
                    })
                    .Where(animal => selectedAnimales.IdsToAdd.Contains(animal.AnimalID.ToString())).ToList();

            Cliente cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            compra.Nombre = cliente.Nombre;
            compra.PrimerApellido = cliente.PrimerApellido;
            compra.SegundoApellido = cliente.SegundoApellido;

            return View(compra);
        }


        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CompraCreateViewModel compraViewModel, IList<L_compraViewModel> l_compras, Tarjeta_Credito tarjeta_Credito, PayPal PayPal)
        {
            Animal animal; 
            L_Compra l_Compra;
            Cliente customer;
            Compra compra = new Compra();
            compra.PrecioTotal = 0;
            compra.L_Compras = new List<L_Compra>();
            if (ModelState.IsValid)
            {
                foreach (L_compraViewModel item in l_compras)
                {
                    animal = await _context.Animal.FirstOrDefaultAsync<Animal>(m => m.AnimalID == item.AnimalID);
                    if (animal.Cantidad < item.cantidad)
                    {

                        ModelState.AddModelError("", $"no hay suficientes animales de la raza seleccionada");
                        compraViewModel.l_compras = l_compras;
                    }
                    else
                    {
                        if (item.cantidad > 0)
                        {
                            animal.Cantidad = animal.Cantidad - item.cantidad;
                            l_Compra = new L_Compra();
                            l_Compra.Animal = animal;
                            l_Compra.Compra = compra;
                            l_Compra.Cantidad = item.cantidad;
                            compra.PrecioTotal += item.cantidad * animal.Precio;
                            compra.L_Compras.Add(l_Compra);
                        }
                    }
                }
            }
            customer = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.ErrorCount > 0)
            {
                compraViewModel.Nombre = customer.Nombre;
                compraViewModel.PrimerApellido = customer.PrimerApellido;
                compraViewModel.SegundoApellido = customer.SegundoApellido;
                return View(compraViewModel);
            }
            if (compra.PrecioTotal == 0)
            {
                compraViewModel.Nombre = customer.Nombre;
                compraViewModel.PrimerApellido = customer.PrimerApellido;
                compraViewModel.SegundoApellido = customer.SegundoApellido;
                ModelState.AddModelError("", $"Seleccione al menos una animal para comprar o cancele su compra");
                compraViewModel.l_compras = l_compras;
                return View(compraViewModel);
            }

            compra.Cliente = customer;
            compra.FechaCompra = DateTime.Now;
            if (compraViewModel.MetodoPago == "PayPal")
                compra.MetodoPago = compraViewModel.PayPal;
            else
                compra.MetodoPago = compraViewModel.TarjetaCredito;
            compra.DirecionEnvio = compraViewModel.DireccionEnvio;
            _context.Add(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = compra.CompraID });
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompraID,PrecioTotal,FechaCompra,DirecionEnvio")] Compra compra)
        {
            if (id != compra.CompraID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.CompraID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FirstOrDefaultAsync(m => m.CompraID == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.CompraID == id);
        }
        // get del select 
        public IActionResult SelectAnimalesForCompra (string TipoAnimal, double precio)
        {
            //creamos una instancia 
            SelectAnimalesForCompraViewModels selectAnimales = new SelectAnimalesForCompraViewModels();
            //include --> es para unir dos tablas (reunion)

            selectAnimales.Animales = _context.Animal.Include(m => m.Tipo).Where(m => m.Cantidad > 0);

            if (TipoAnimal != null)
                selectAnimales.Animales = selectAnimales.Animales.Where(m=>m.Tipo.Raza.Contains(TipoAnimal));

            if (precio != 0)
                selectAnimales.Animales = selectAnimales.Animales.Where(m =>m.Precio.CompareTo(precio)<=0);

                selectAnimales.Animales.ToList();

            return View(selectAnimales);
        }
        // POST: Compras/SelectAnimalesForCompra
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectAnimalesForCompra(SelectedAnimalesForCompraViewModel selectedAnimales)
        {
            if (selectedAnimales.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedAnimales);
            }
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un aminal");

            SelectAnimalesForCompraViewModels selectAnimales = new SelectAnimalesForCompraViewModels();

            selectAnimales.Animales = _context.Animal.Include(m => m.Tipo).Where(m => m.Cantidad > 0);

            return View(selectAnimales);
        }

    }
}
