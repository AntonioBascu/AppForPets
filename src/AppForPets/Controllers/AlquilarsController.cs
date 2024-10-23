using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.AlquilarViewModel;
using Microsoft.AspNetCore.Authorization;

namespace AppForPets.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class AlquilarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlquilarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Alquilars
        public async Task<IActionResult> Index()
        {
            return View(_context.Alquilar.Include(p => p.Cliente).Where(p => p.Cliente.UserName.Equals(User.Identity.Name)).OrderByDescending(p => p.FechaInicio).ToList());
        }

        // GET: Alquilar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquilar = _context.Alquilar.Include(p => p.AlquilarProductos).ThenInclude<Alquilar, AlquilarProductos, Producto>(p => p.Producto).Include(p => p.Cliente).Where(p => p.AlquilarID == id).ToList();
            //var alquilar = _context.Alquilar.First(p => p.AlquilarID== id);
            if (alquilar.Count == 0)
            {
                return NotFound();
            }

            return View(alquilar.First());
        }



        // GET: Alquilars/Create
        public IActionResult Create(SelectedProductosForAlquilerViewModel selectedProductos)
        {

            AlquilarCreateViewModel alquilar = new AlquilarCreateViewModel();
            alquilar.AlquilarProductos = new List<AlquilarProductoViewModel>();

            if (selectedProductos.IdsToAdd == null)
            {
                ModelState.AddModelError("ProductoNoSelected", "Debes seleccionar al menos un producto para alquilar, por favor");
            }
            else
                alquilar.AlquilarProductos = _context.Producto.Include(producto => producto.TipoAnimal)
                    .Select(producto => new AlquilarProductoViewModel()
                    {
                        ProductoID = producto.ProductoID,
                        TipoAnimal = producto.TipoAnimal.NombreAnimal,
                        PrecioAlquiler = producto.PrecioAlquiler,
                        NombreProducto = producto.NombreProducto
                    })
                    .Where(producto => selectedProductos.IdsToAdd.Contains(producto.ProductoID.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            alquilar.Nombre = Cliente.Nombre;
            alquilar.PrimerApellido = Cliente.PrimerApellido;
            alquilar.SegundoApellido = Cliente.SegundoApellido;

            return View(alquilar);
        }

        // POST: Alquilars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(AlquilarCreateViewModel alquilarViewModel, IList<AlquilarProductoViewModel> AlquilarProductos)
        {
            Producto producto; AlquilarProductos alquilarProductos;
            Cliente cliente;
            Alquilar alquilar= new Alquilar();
            alquilar.PrecioTotal = 0;
            alquilar.AlquilarProductos = new List<AlquilarProductos>();
            if (ModelState.IsValid)
            {
                foreach (AlquilarProductoViewModel item in AlquilarProductos)
                {
                    producto = await _context.Producto.FirstOrDefaultAsync<Producto>(m => m.ProductoID== item.ProductoID);
                    if (producto.CantidadAlquilar < item.CantidadAlquiler)
                    {

                        ModelState.AddModelError("","No hay suficiente.");
                        alquilarViewModel.AlquilarProductos= AlquilarProductos;
                    }
                    else
                    {
                        if (item.CantidadAlquiler > 0)
                        {
                            producto.CantidadAlquilar= producto.CantidadAlquilar- item.CantidadAlquiler;
                            alquilarProductos = new AlquilarProductos();
                            alquilarProductos.Producto = producto;
                            alquilarProductos.Alquilar= alquilar;
                            alquilarProductos.Cantidad= item.CantidadAlquiler;
                            alquilar.PrecioTotal+= item.CantidadAlquiler* producto.PrecioAlquiler;
                            alquilar.AlquilarProductos.Add(alquilarProductos);
                        }
                    }
                }
            }
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.ErrorCount > 0)
            {
                alquilarViewModel.Nombre = cliente.Nombre;
                alquilarViewModel.PrimerApellido = cliente.PrimerApellido;
                alquilarViewModel.SegundoApellido = cliente.SegundoApellido;
                return View(alquilarViewModel);
            }
            if (alquilar.PrecioTotal == 0)
            {
                alquilarViewModel.Nombre = cliente.Nombre;
                alquilarViewModel.PrimerApellido = cliente.PrimerApellido;
                alquilarViewModel.SegundoApellido = cliente.SegundoApellido;
                ModelState.AddModelError("", $"Please select at least a producto to be alquilar or cancel your alquiler");
                alquilarViewModel.AlquilarProductos= AlquilarProductos;
                return View(alquilarViewModel);
            }

            alquilar.Cliente = cliente;
            alquilar.FechaInicio= DateTime.Now;
            alquilar.FechaFin = DateTime.Now;
            
            _context.Add(alquilar);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = alquilar.AlquilarID });
        }



        // GET: Alquilars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquilar = await _context.Alquilar.FindAsync(id);
            if (alquilar == null)
            {
                return NotFound();
            }
            return View(alquilar);
        }

        // POST: Alquilars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlquilarID,PrecioTotal,FechaInicio,FechaFin")] Alquilar alquilar)
        {
            if (id != alquilar.AlquilarID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alquilar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlquilarExists(alquilar.AlquilarID))
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
            return View(alquilar);
        }

        // GET: Alquilars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquilar = await _context.Alquilar
                .FirstOrDefaultAsync(m => m.AlquilarID == id);
            if (alquilar == null)
            {
                return NotFound();
            }

            return View(alquilar);
        }

        // POST: Alquilars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alquilar = await _context.Alquilar.FindAsync(id);
            _context.Alquilar.Remove(alquilar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlquilarExists(int id)
        {
            return _context.Alquilar.Any(e => e.AlquilarID == id);
        }


        //Los parámetros son los descritos en el ViewModel para poder filtrar, con el mismo nombre
        public IActionResult SelectProductosForAlquiler(string productoNombre, string productoAnimalSelected)
        {
            SelectProductosForAlquilerViewModel selectProductos = new SelectProductosForAlquilerViewModel();
            //Para que el desplegable ofrezca la lista de Géneros que hay en la BD
            selectProductos.TipoAnimal = new SelectList(
            _context.TipoAnimal.Select(g => g.NombreAnimal).ToList());
            //Sólo mostrará películas para las que hay cantidad suficiente para comprar
            selectProductos.Productos = _context.Producto.Include(m => m.TipoAnimal).Where(m =>
            m.CantidadAlquilar > 0);
            //utilizado si el usuario quiere buscar por título
            if (productoNombre != null)
                selectProductos.Productos = selectProductos.Productos.Where(m =>
                m.NombreProducto.Contains(productoNombre));
            //utilizado si el usuario selecciona un género en el desplegable
            if (productoAnimalSelected != null)
                selectProductos.Productos = selectProductos.Productos.Where(m =>
                m.TipoAnimal.NombreAnimal.Contains(productoAnimalSelected));
            //en este punto ejecuta la consulta con los filtros que hemos establecido
            selectProductos.Productos.ToList();
            return View(selectProductos);
        }
        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProductosForAlquiler(SelectedProductosForAlquilerViewModel selectedProductos)
        {
            if (selectedProductos.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedProductos);
            }
            //se mostrará un mensaje de error al usuario para indicar que seleccione algun producto
            ModelState.AddModelError(string.Empty, "Al menos debes seleccionar uno");
            //se recreará otra vez el view model
            SelectProductosForAlquilerViewModel selectProductos = new
            SelectProductosForAlquilerViewModel();
            selectProductos.TipoAnimal = new SelectList(_context.TipoAnimal.Select(g =>
            g.NombreAnimal).ToList());
            selectProductos.Productos = _context.Producto.Include(m => m.TipoAnimal).Where(m =>
            m.CantidadAlquilar > 0).ToList();
            return View(selectProductos);
        }
    }
}
