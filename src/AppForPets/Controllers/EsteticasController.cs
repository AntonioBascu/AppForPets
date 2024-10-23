using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppForPets.Data;
using AppForPets.Models;
using Microsoft.AspNetCore.Authorization;
using AppForPets.Models.EsteticaViewModels;

namespace AppForPets.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class EsteticasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EsteticasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Esteticas
        public async Task<IActionResult> Index()
        {
            return View(_context.Estetica.Include(p => p.Cliente).Where(p => p.Cliente.UserName.Equals(User.Identity.Name)).OrderByDescending(p => p.FechaCompra).ToList());
        }


        // GET: Esteticas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var estetica = _context.Estetica
                .Include(p => p.Linea_Servicios)
                    .ThenInclude<Estetica, Linea_Servicio, Tipo_Servicio>(p => p.Servicio.Tipo_Servicio)
                .Include(p => p.Cliente).Where(p => p.EsteticaID == id).ToList();


            //var purchase = _context.Purchase.First(p => p.PurchaseId == id);
            if (estetica.Count == 0)
            {
                return NotFound();
            }

            return View(estetica.First());
        }


        // GET: Esteticas/Create
        public IActionResult Create(SelectedServiciosForEsteticaViewModels selectedServicios)
            {
                EsteticaCreateViewModel estetica = new EsteticaCreateViewModel();
            estetica.EsteticaItems = new List<EsteticaItemViewModel>();

            if (selectedServicios.IdsToAdd == null)
            {
                ModelState.AddModelError("Servicio no Seleccionado", "Por favor, selecciona al menos un servicio.");
            }
            else
                estetica.EsteticaItems = _context.Servicio.Include(servicio => servicio.Tipo_Servicio)
                    .Select(servicio => new EsteticaItemViewModel()
                    {
                        ServicioId = servicio.ServicioID,
                        Tipo_Servicio = servicio.Tipo_Servicio.Nombre,
                        PrecioCompra = servicio.Precio_Servicio,
                        Nombre_Servicio = servicio.Nombre_Servicio
                    })
                    .Where(servicio => selectedServicios.IdsToAdd.Contains(servicio.ServicioId.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            estetica.Nombre = Cliente.Nombre;
            estetica.Primer_Apellido = Cliente.PrimerApellido;
            estetica.Segundo_Apellido = Cliente.SegundoApellido;

            return View(estetica);
        }

        // POST: Esteticas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(EsteticaCreateViewModel esteticaViewModel, IList<EsteticaItemViewModel> EsteticasItems, Tarjeta_Credito tarjeta_Credito, PayPal payPal)
        {

            Servicio servicio; 
            Linea_Servicio linea_servicio;
            Cliente cliente;
            Estetica estetica = new Estetica();
            estetica.PrecioTotal = 0;
            estetica.Linea_Servicios = new List<Linea_Servicio>();

            if (ModelState.IsValid) // Si el modelo cumple las reglas de validación
            {
                foreach (EsteticaItemViewModel item in esteticaViewModel.EsteticaItems)
                {

                    servicio = await _context.Servicio.FirstOrDefaultAsync<Servicio>(m => m.ServicioID == item.ServicioId);
                    if (servicio.Tiempo_Duracion < item.Tiempo_Duracion)
                    {


                        ModelState.AddModelError("", $"No hay suficiente tiempo para el servicio seleccionado");
                        esteticaViewModel.EsteticaItems = EsteticasItems;
                    }
                    else
                    {
                        if (item.Tiempo_Duracion > 0)
                        {
                            //servicio.Tiempo_Duracion = servicio.Tiempo_Duracion - item.Tiempo_Duracion;
                            linea_servicio = new Linea_Servicio();
                            linea_servicio.Servicio = servicio;
                            linea_servicio.Estetica = estetica;
                            linea_servicio.Turno_Servicio = item.Tiempo_Duracion;
                            estetica.PrecioTotal += item.Tiempo_Duracion * servicio.Precio_Servicio;
                            estetica.Linea_Servicios.Add(linea_servicio);
                        }
                }
            }
            }
            
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.ErrorCount > 0)
            {
                esteticaViewModel.Nombre = cliente.Nombre;
                esteticaViewModel.Primer_Apellido = cliente.PrimerApellido;
                esteticaViewModel.Segundo_Apellido = cliente.SegundoApellido;
                return View(esteticaViewModel);
            }

            if (estetica.PrecioTotal == 0)
            {
                esteticaViewModel.Nombre = cliente.Nombre;
                esteticaViewModel.Primer_Apellido = cliente.PrimerApellido;
                esteticaViewModel.Segundo_Apellido = cliente.SegundoApellido;
                ModelState.AddModelError("", $"Por facor, selecciona al menos un servicio para comprar o cancele la compra");
                esteticaViewModel.EsteticaItems = EsteticasItems;
                return View(esteticaViewModel);
            }

            estetica.Cliente = cliente;
            estetica.FechaCompra = DateTime.Now;
                if (esteticaViewModel.Metodo_Pago == "PayPal")
                    estetica.Metodo_Pago = esteticaViewModel.PayPal;
                else
                    estetica.Metodo_Pago = esteticaViewModel.Tarjeta_Credito;
            
            estetica.Direccion_correo = esteticaViewModel.Direccion_correo;
            estetica.telefono = esteticaViewModel.telefono;

            _context.Add(estetica);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = estetica.EsteticaID });
        }
  


        // GET: Esteticas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estetica = await _context.Estetica.FindAsync(id);
            if (estetica == null)
            {
                return NotFound();
            }
            return View(estetica);
        }

        // POST: Esteticas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EsteticaID,Nombre_Servicio")] Estetica estetica)
        {
            if (id != estetica.EsteticaID)  // Comprobamos que el id coincida
            {
                return NotFound();
            }

            if (ModelState.IsValid) // Si pasa la validación
            {
                try
                {
                    _context.Update(estetica);      // actualiza el objeto
                    await _context.SaveChangesAsync();  // Guardamos los cambios en la BDD
                }
                catch (DbUpdateConcurrencyException)    // Si dos usuarios guardan cambios a la vez
                {
                    if (!EsteticaExists(estetica.EsteticaID))
                    {
                        return NotFound();  // Si no existe, 404
                    }
                    else
                    {
                        throw;  // Si existe, excepción
                    }
                }
                return RedirectToAction("Index"); // Mostramos todos
            }
            return View(estetica); // No es válido, lo editamos otra vez
        }

        // GET: Esteticas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();  // Si no existe la estética pasada por parámetro, se devuelve error 404
            }

            var estetica = await _context.Estetica
                .SingleOrDefaultAsync(m => m.EsteticaID == id); // Buscamos el objeto en la BDD
            if (estetica == null)
            {
                return NotFound();  //Si no existe, se devuelve error 404
            }

            return View(estetica); // Se llama la vista por POST
        }

        // POST: Esteticas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estetica = await _context.Estetica.SingleOrDefaultAsync(m => m.EsteticaID == id); // Se obtiene la BDD  
            _context.Estetica.Remove(estetica); // Lo eliminamos
            await _context.SaveChangesAsync(); // Guardamos asíncronamente
            return RedirectToAction("Index"); // Llamamos a la vista para mostrar todos
        }

        private bool EsteticaExists(int id)
        {
            return _context.Estetica.Any(e => e.EsteticaID == id);
        }


        //GET: Servicios/SelectServiciosForEstetica
        public IActionResult SelectServicioForEstetica(string nombre, int tiempo_duracion)
        {
            SelectServiciosForEsteticaViewModels selectServicios = new SelectServiciosForEsteticaViewModels();
            selectServicios.Tipo_Servicios = new SelectList(_context.Tipo_Servicio.Select(g => g.Nombre).ToList());
            selectServicios.Servicios = _context.Servicio.Include(m => m.Tipo_Servicio).Where(m => m.Tiempo_Duracion > 0);

            if (nombre != null)
                selectServicios.Servicios = selectServicios.Servicios.Where(m => m.Tipo_Servicio.Nombre.Contains(nombre));

            if (tiempo_duracion != 0)
                selectServicios.Servicios = selectServicios.Servicios.Where(m => m.Tiempo_Duracion.Equals(tiempo_duracion));

            selectServicios.Servicios.ToList();

            return View(selectServicios);
        }

        // POST: Purchases/SelectMoviesForPurchase
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectServicioForEstetica(SelectedServiciosForEsteticaViewModels selectedServicios)
        {
            if (selectedServicios.IdsToAdd != null)
            {

                return RedirectToAction("Create", selectedServicios);
            }

            //Mensaje de error que se muestra al cliente si no selecciona ningún servicio
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un servicio");

            SelectServiciosForEsteticaViewModels selectServicios = new SelectServiciosForEsteticaViewModels();
            selectServicios.Tipo_Servicios = new SelectList(_context.Servicio.Select(g => g.Nombre_Servicio).ToList());
            selectServicios.Servicios = _context.Servicio.Include(m => m.Tipo_Servicio).Where(m => m.Tiempo_Duracion > 0);

            return View(selectServicios);
        }


    }
}
