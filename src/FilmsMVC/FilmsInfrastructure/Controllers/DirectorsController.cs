﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsDomain.Model;
using FilmsInfrastructure;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;

namespace FilmsInfrastructure.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly DbfilmsContext _context;

        public DirectorsController(DbfilmsContext context)
        {
            _context = context;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Directors.ToListAsync());
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            //return View(director);
            return RedirectToAction("IndexDirector", "Films", new { id = director.Id, name = director.Name });
        }

        [Authorize(Roles = "admin, стажер(-ка)")]
        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, стажер(-ка)")]
        public async Task<IActionResult> Create([Bind("Name,Id")] Director director)
        {
            if (_context.Directors.Any(g => g.Name == director.Name))
            {
                ModelState.AddModelError("Name", "Такий режисер вже існує.");
                return View(director);
            }

            if (ModelState.IsValid)
            {
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        // GET: Directors/Edit/5
        [Authorize(Roles = "admin, стажер(-ка)")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, стажер(-ка)")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Director director)
        {
            if (id != director.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Directors.Any(g => g.Name == director.Name))
                    {
                        ModelState.AddModelError("Name", "Такий режисер вже існує.");
                        return View(director);
                    }
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.Id))
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
            return View(director);
        }

        // GET: Directors/Delete/5
        [Authorize(Roles = "admin,стажер(-ка)")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,стажер(-ка)")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.Id == id);
        }
    }
}
