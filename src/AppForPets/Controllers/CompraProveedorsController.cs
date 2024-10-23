using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppForPets.Data;
using AppForPets.Models;
using AppForPets.Models.CompraProveedorViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AppForPets.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CompraProveedorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompraProveedorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompraProveedors
        public async Task<IActionResult> Index()
        {
            return View(_context.CompraProveedor.Include(c => c.Usuario).Where(c => c.Usuario.UserName.Equals(User.Identity.Name)).OrderByDescending(c => c.FechaCompra).ToList());
        }

        // GET: CompraProveedors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compraProveedor = _context.CompraProveedor.Include(p => p.CompraItems).ThenInclude<CompraProveedor, CompraProvItem, ProductoProveedor>(p => p.ProductoProveedor).Include(p => p.Usuario).Where(p => p.IdCompraProveedor == id).ToList();
            //var compraProveedor = _context.CompraProveedor.First(p => p.IdCompraProveedor == id);

            if (compraProveedor.Count == 0)
            {
                return NotFound();
            }

            return View(compraProveedor.First());
        }

        // GET: CompraProveedors/Create
        public IActionResult Create(SelectedProductosProveedorForCompraViewModel selectedProductos)
        {
            CompraProvCreateViewModel compra = new CompraProvCreateViewModel();
            compra.CompraItems = new List<CompraProvItemViewModel>();

            if (selectedProductos.IdsProductos == null)
            {
                ModelState.AddModelError("ProductoNoSeleccionado", "Debes seleccionar al menos un producto para la compra");
            }
            else
                compra.CompraItems = _context.ProductoProveedor.Include(p => p.Producto.TipoAnimal).Include(p => p.Proveedor)
                    .Select(p => new CompraProvItemViewModel()
                    {
                        ProductoProvId = p.IdProductoProv,
                        TipoAnimal = p.Producto.TipoAnimal.NombreAnimal,
                        PrecioCompra = p.Precio,
                        NombreProductoProv = p.Nombre,
                        Proveedor = p.Proveedor.Nombre
                    })
                    .Where(p => selectedProductos.IdsProductos.Contains(p.ProductoProvId.ToString())).ToList();

            ApplicationUser Usuario = _context.Users.OfType<ApplicationUser>().FirstOrDefault<ApplicationUser>(u => u.UserName.Equals(User.Identity.Name));
            compra.Nombre = Usuario.Nombre;
            compra.PrimerApellido = Usuario.PrimerApellido;
            compra.SegundoApellido = Usuario.SegundoApellido;

            return View(compra);
        }

        // POST: CompraProveedors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CompraProvCreateViewModel compraViewModel, IList<CompraProvItemViewModel> CompraItems, Tarjeta_Credito TarjetaCredito, PayPal Paypal)
        {
            ProductoProveedor producto; CompraProvItem compraItem;
            ApplicationUser usuario;
            CompraProveedor compra = new CompraProveedor();
            compra.PrecioTotal = 0;
            compra.CompraItems = new List<CompraProvItem>();
            if (ModelState.IsValid)
            {
                foreach (CompraProvItemViewModel item in CompraItems)
                {
                    producto = await _context.ProductoProveedor.FirstOrDefaultAsync<ProductoProveedor>(p => p.IdProductoProv == item.ProductoProvId);
                    if (producto.CantidadDisponible < item.Cantidad)
                    {

                        ModelState.AddModelError("", $"No hay suficientes unidades de {producto.Nombre}, selecciona menos cantidad o {producto.CantidadDisponible} exactamente");
                        compraViewModel.CompraItems = CompraItems;
                    }
                    else
                    {
                        if (item.Cantidad > 0)
                        {
                            producto.CantidadDisponible = producto.CantidadDisponible - item.Cantidad;
                            compraItem = new CompraProvItem();
                            compraItem.ProductoProveedor = producto;
                            compraItem.Compra = compra;
                            compraItem.Cantidad = item.Cantidad;
                            compra.PrecioTotal += item.Cantidad * producto.Precio;
                            compra.CompraItems.Add(compraItem);
                        }
                    }
                }
            }
            usuario = await _context.Users.OfType<ApplicationUser>().FirstOrDefaultAsync<ApplicationUser>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.ErrorCount > 0)
            {
                compraViewModel.Nombre = usuario.Nombre;
                compraViewModel.PrimerApellido = usuario.PrimerApellido;    
                compraViewModel.SegundoApellido = usuario.SegundoApellido;
                return View(compraViewModel);
            }
            if (compra.PrecioTotal == 0)
            {
                compraViewModel.Nombre = usuario.Nombre;
                compraViewModel.PrimerApellido = usuario.PrimerApellido;
                compraViewModel.SegundoApellido = usuario.SegundoApellido;
                ModelState.AddModelError("", $"Selecciona al menos un producto para comprar");
                compraViewModel.CompraItems = CompraItems;
                return View(compraViewModel);
            }

            compra.Usuario = usuario;
            compra.FechaCompra = DateTime.Now;
            if (compraViewModel.MetodoPago == "PayPal")
                compra.MetodoPago = compraViewModel.PayPal;
            else
                compra.MetodoPago = compraViewModel.TarjetaCredito;
            compra.DireccionEnvio = compraViewModel.DireccionEnvio;
            compra.TelefonoContacto = compraViewModel.TelefonoContacto;
            _context.Add(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = compra.IdCompraProveedor });
        }

        // GET: CompraProveedors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compraProveedor = await _context.CompraProveedor.FindAsync(id);
            if (compraProveedor == null)
            {
                return NotFound();
            }
            return View(compraProveedor);
        }

        // POST: CompraProveedors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompraProveedor,PrecioTotal,FechaCompra,DireccionEnvio,TelefonoContacto")] CompraProveedor compraProveedor)
        {
            if (id != compraProveedor.IdCompraProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compraProveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraProveedorExists(compraProveedor.IdCompraProveedor))
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
            return View(compraProveedor);
        }

        // GET: CompraProveedors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compraProveedor = await _context.CompraProveedor
                .FirstOrDefaultAsync(m => m.IdCompraProveedor == id);
            if (compraProveedor == null)
            {
                return NotFound();
            }

            return View(compraProveedor);
        }

        // POST: CompraProveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compraProveedor = await _context.CompraProveedor.FindAsync(id);
            _context.CompraProveedor.Remove(compraProveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraProveedorExists(int id)
        {
            return _context.CompraProveedor.Any(e => e.IdCompraProveedor == id);
        }

        //GET
        public IActionResult SelectProveedorForCompra(string nombreProveedor, string direccionSeleccionada)
        {
            SelectProveedorForCompraViewModel selectProveedor = new SelectProveedorForCompraViewModel();
            selectProveedor.Proveedores = _context.Proveedor.Include(p => p.ProveedorProductos);
            if (nombreProveedor != null)
                selectProveedor.Proveedores = selectProveedor.Proveedores.Where(p => p.Nombre.Contains(nombreProveedor));

            if (direccionSeleccionada != null)
                selectProveedor.Proveedores = selectProveedor.Proveedores.Where(p => p.Direccion.Contains(direccionSeleccionada));

            selectProveedor.Proveedores.ToList();

            return View(selectProveedor);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProveedorForCompra(SelectedProveedorForCompraViewModel selectedProveedor)
        {
            if (selectedProveedor.IdProveedor != null)
            {

                return RedirectToAction("SelectProductosProveedorForCompra", selectedProveedor);
            }

            ModelState.AddModelError(string.Empty, "Debes seleccionar un proveedor para comprar");
            SelectProveedorForCompraViewModel selectProveedor = new SelectProveedorForCompraViewModel();
            selectProveedor.Proveedores = _context.Proveedor.ToList();

            return View(selectProveedor);
        }

        //GET
        public IActionResult SelectProductosProveedorForCompra(SelectedProveedorForCompraViewModel selectedProveedor, string nombreProducto, string tipoAnimalSelect, string proveedor)
        {
            SelectProductosProveedorForCompraViewModel selectProductos = new SelectProductosProveedorForCompraViewModel();
            selectProductos.TipoAnimales = new SelectList(_context.TipoAnimal.Select(t => t.NombreAnimal).ToList());

            if(proveedor != null)
            {
                selectProductos.proveedor = proveedor;
            }
            else
            {
                selectProductos.proveedor = selectedProveedor.IdProveedor;
            }
            
            selectProductos.Productos = _context.ProductoProveedor.Include(p => p.Producto.TipoAnimal).Include(p=> p.Proveedor).Where(p => p.Proveedor.IdProveedor.Equals(int.Parse(selectProductos.proveedor)) && p.CantidadDisponible > 0);

            if (nombreProducto != null)
                selectProductos.Productos = selectProductos.Productos.Where(p => p.Producto.NombreProducto.Contains(nombreProducto));

            if (tipoAnimalSelect != null)
                selectProductos.Productos = selectProductos.Productos.Where(p => p.Producto.TipoAnimal.NombreAnimal.Contains(tipoAnimalSelect));

            selectProductos.Productos.ToList();

            return View(selectProductos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProductosProveedorForCompra(SelectedProductosProveedorForCompraViewModel selectedProductos)
        {
            if (selectedProductos.IdsProductos != null)
            {
                return RedirectToAction("Create", selectedProductos);
            }
       
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un producto");
            SelectProductosProveedorForCompraViewModel selectProductos = new SelectProductosProveedorForCompraViewModel();
            selectProductos.TipoAnimales = new SelectList(_context.TipoAnimal.Select(t => t.NombreAnimal).ToList());
            selectProductos.proveedor = selectedProductos.Proveedor;
            selectProductos.Productos = _context.ProductoProveedor.Include(p => p.Producto.TipoAnimal).Include(p => p.Proveedor).Where(p => p.Proveedor.IdProveedor.Equals(int.Parse(selectProductos.proveedor)) && p.CantidadDisponible > 0);

            return View(selectProductos);
        }

    }
}
