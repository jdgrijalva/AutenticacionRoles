
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutenticacionRoles.Models;

public class GenerosController : Controller
{
    private readonly AutenticacionRolesContext _context;

    public GenerosController(AutenticacionRolesContext context)
    {
        _context = context;
    }

    // GET: Generos
    public async Task<IActionResult> Index()
    {
        return View(await _context.Genero.ToListAsync());
    }

    // GET: Generos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Genero
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genero == null)
        {
            return NotFound();
        }

        return View(genero);
    }

    // GET: Generos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Generos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nombre,Descripcion")] Genero genero)
    {
        if (ModelState.IsValid)
        {
            _context.Genero.Add(genero);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(genero);
    }

    // GET: Generos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Genero.FindAsync(id);

        if (genero == null)
        {
            return NotFound();
        }

        return View(genero);
    }

    // POST: Generos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Nombre,Descripcion")] Genero genero)
    {
        if (id != genero.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(genero);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroExists(genero.Id))
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

        return View(genero);
    }

    // GET: Generos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Genero
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genero == null)
        {
            return NotFound();
        }

        // Comprobar si tiene películas relacionadas.
        ViewBag.TienePeliculas = await _context.Pelicula
            .AnyAsync(p => p.GeneroId == id);

        return View(genero);
    }

    // POST: Generos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genero = await _context.Genero.FindAsync(id);

        if (genero == null)
        {
            return NotFound();
        }

        // Impedir eliminar géneros con películas.
        bool tienePeliculas = await _context.Pelicula
            .AnyAsync(p => p.GeneroId == id);

        if (tienePeliculas)
        {
            ViewBag.TienePeliculas = true;
            ViewBag.MensajeError =
                "No se puede eliminar este género porque tiene películas asociadas.";

            return View("Delete", genero);
        }

        try
        {
            _context.Genero.Remove(genero);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Controlar errores de la base de datos.
            ViewBag.TienePeliculas = true;
            ViewBag.MensajeError =
                "No se pudo eliminar el género. Verifique si tiene películas asociadas.";

            return View("Delete", genero);
        }
        
        return RedirectToAction(nameof(Index));
    }

    // Verifica si existe un género.
    private bool GeneroExists(int id)
    {
        return _context.Genero.Any(g => g.Id == id);
    }
}
