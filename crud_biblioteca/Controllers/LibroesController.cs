using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crud_biblioteca.Models;
using System.Text.RegularExpressions;

namespace crud_biblioteca.Controllers
{
    public class LibroesController : Controller
    {
        private readonly PruebaBibliotecaContext _context;

        public LibroesController(PruebaBibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libroes
        public async Task<IActionResult> Index()
        {
              return _context.Libros != null ? 
                          View(await _context.Libros.ToListAsync()) :
                          Problem("Entity set 'PruebaBibliotecaContext.Libros'  is null.");
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.IdLibros == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLibros,Titulo,Autor,Isbn,AñoPublicacion,Disponibilidad")] Libro libro)
        {
            if (!string.IsNullOrWhiteSpace(libro.Titulo))
            {
                if (!string.IsNullOrWhiteSpace(libro.Autor))
                {
                    if (!string.IsNullOrWhiteSpace(libro.Isbn) && Regex.IsMatch(libro.Isbn, @"^\d{10}(\d{3})?$"))
                    {
                        if (libro.AñoPublicacion >= 1500 && libro.AñoPublicacion <= DateTime.Now.Year)
                        {
                            try
                            {
                                _context.Add(libro);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", "Ocurrió un error al guardar el libro: " + ex.Message);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("AñoPublicacion", "El año de publicación debe ser válido.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Isbn", "El ISBN debe tener 10 o 13 dígitos numéricos.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Autor", "El autor es obligatorio.");
                }
            }
            else
            {
                ModelState.AddModelError("Titulo", "El título es obligatorio.");
            }

            return View(libro);
        }


        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLibros,Titulo,Autor,Isbn,AñoPublicacion,Disponibilidad")] Libro libro)
        {
            if (id != libro.IdLibros)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.IdLibros))
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
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.IdLibros == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'PruebaBibliotecaContext.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
          return (_context.Libros?.Any(e => e.IdLibros == id)).GetValueOrDefault();
        }
    }
}
