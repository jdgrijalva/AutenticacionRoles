
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutenticacionRoles.Models;

public class PeliculasController : Controller
{
    private readonly AutenticacionRolesContext _context;

    public PeliculasController(AutenticacionRolesContext context)
    {
        _context = context;
    }

    // GET: Peliculas
    // Muestra las películas con el nombre de su género.
    public async Task<IActionResult> Index()
    {
        var peliculas = await _context.Pelicula
            .Include(p => p.Genero)
            .ToListAsync();

        return View(peliculas);
    }

    // GET: Peliculas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Pelicula
            .Include(p => p.Genero)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula == null)
        {
            return NotFound();
        }

        return View(pelicula);
    }

    // GET: Peliculas/Create
    public async Task<IActionResult> Create()
    {
        await CargarGeneros();
        return View();
    }

    // POST: Peliculas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Titulo,Director,DuracionMinutos,GeneroId")] Pelicula pelicula)
    {
        // Verificar que el género seleccionado exista.
        if (!await _context.Genero.AnyAsync(g => g.Id == pelicula.GeneroId))
        {
            ModelState.AddModelError("GeneroId", "Debe seleccionar un género válido.");
        }

        if (ModelState.IsValid)
        {
            _context.Pelicula.Add(pelicula);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Volver a cargar los géneros si hay errores.
        await CargarGeneros(pelicula.GeneroId);
        return View(pelicula);
    }

    // GET: Peliculas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Pelicula.FindAsync(id);

        if (pelicula == null)
        {
            return NotFound();
        }

        await CargarGeneros(pelicula.GeneroId);
        return View(pelicula);
    }

    // POST: Peliculas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Titulo,Director,DuracionMinutos,GeneroId")] Pelicula pelicula)
    {
        if (id != pelicula.Id)
        {
            return NotFound();
        }

        // Verificar que el género seleccionado exista.
        if (!await _context.Genero.AnyAsync(g => g.Id == pelicula.GeneroId))
        {
            ModelState.AddModelError("GeneroId", "Debe seleccionar un género válido.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pelicula);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(pelicula.Id))
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

        // Mantener los géneros disponibles si hay errores.
        await CargarGeneros(pelicula.GeneroId);
        return View(pelicula);
    }

    // GET: Peliculas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Pelicula
            .Include(p => p.Genero)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula == null)
        {
            return NotFound();
        }

        return View(pelicula);
    }

    // POST: Peliculas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Pelicula.FindAsync(id);

        if (pelicula != null)
        {
            _context.Pelicula.Remove(pelicula);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Carga los géneros para la lista desplegable.
    private async Task CargarGeneros(int? generoSeleccionado = null)
    {
        var generos = await _context.Genero
            .OrderBy(g => g.Nombre)
            .ToListAsync();

        ViewData["GeneroId"] = new SelectList(
            generos, "Id", "Nombre", generoSeleccionado);
    }

    // Verifica si existe una película.
    private bool PeliculaExists(int id)
    {
        return _context.Pelicula.Any(p => p.Id == id);
    }
}
