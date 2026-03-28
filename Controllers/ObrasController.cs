using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaleriaArte.Data;
using GaleriaArte.Models;
using Microsoft.AspNetCore.Authorization;

namespace GaleriaArte.Controllers
{
    [Authorize]
    public class ObrasController : Controller
    {
        private readonly GaleriaDbContext _context;

        public ObrasController(GaleriaDbContext context)
        {
            _context = context;
        }

        // GET: Obras
        public async Task<IActionResult> Index()
        {
            var obras = await _context.Obras
                .Take(6)
                .Include(o => o.Artista)
                .ToListAsync();

            var expos = await _context.Exposiciones.ToListAsync();

            var homeModel = new HomeViewModel { Exposiciones = expos, Obras = obras };

            return View(homeModel);
        }

        // GET: Obras/Details/5
        public async Task<IActionResult> Details(Guid? id, int? idExpo)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.idExpo = idExpo;
            var obra = await _context.Obras
                .Include(o => o.Artista)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obra == null)
            {
                return NotFound();
            }

            return View(obra);
        }

        // GET: Obras/Create
        public IActionResult Create()
        {
            ViewData["ArtistaID"] = new SelectList(_context.Artistas, "Id", "Nombre");
            return View();
        }

        // POST: Obras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Estilo,UrlImagen,ArtistaID")] Obra obra)
        {
            if (ModelState.IsValid)
            {
                obra.Id = Guid.NewGuid();
                _context.Add(obra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistaID"] = new SelectList(_context.Artistas, "Id", "Nombre", obra.ArtistaID);
            return View(obra);
        }

        // GET: Obras/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obra = await _context.Obras.FindAsync(id);
            if (obra == null)
            {
                return NotFound();
            }
            ViewData["ArtistaID"] = new SelectList(_context.Artistas, "Id", "Nombre", obra.ArtistaID);
            return View(obra);
        }

        // POST: Obras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Titulo,Estilo,UrlImagen,ArtistaID")] Obra obra)
        {
            if (id != obra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObraExists(obra.Id))
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
            ViewData["ArtistaID"] = new SelectList(_context.Artistas, "Id", "Nombre", obra.ArtistaID);
            return View(obra);
        }

        // GET: Obras/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obra = await _context.Obras
                .Include(o => o.Artista)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obra == null)
            {
                return NotFound();
            }

            return View(obra);
        }

        // POST: Obras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra != null)
            {
                _context.Obras.Remove(obra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObraExists(Guid id)
        {
            return _context.Obras.Any(e => e.Id == id);
        }
    }
}
