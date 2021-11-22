using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.IServices;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{    public class PeopleController : Controller
    {
        private readonly CsvFileContext _context;
        private IHostingEnvironment _environment;
        private ICsvService _csvService;

        public PeopleController(CsvFileContext context, IHostingEnvironment environment, ICsvService csvService)
        {
            _context = context;
            _environment = environment;
            _csvService = csvService;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile postedFile, string filter)
        {
            if (filter != null)
            {
                switch (filter)
                {
                    case "Id":
                        return View(await _context.Person.OrderBy(x => x.Id).ToListAsync());
                    case "Name":
                        return View(await _context.Person.OrderBy(x => x.Name).ToListAsync());
                    case "DateOfBirth":
                        return View(await _context.Person.OrderBy(x => x.DateOfBirth).ToListAsync());
                    case "Married":
                        return View(await _context.Person.OrderBy(x => x.Married).ToListAsync());
                    case "Phone":
                        return View(await _context.Person.OrderBy(x => x.Phone).ToListAsync());
                }
            }

            if(postedFile != null) { 
                var people = new List<Person>();
                foreach(var row in _csvService.GetCsv(postedFile, this._environment))
                {
                    var rowEl = row.Trim().Split(',');
                    var person = new Person(rowEl[0], 
                        Convert.ToDateTime(rowEl[1]), 
                        Convert.ToBoolean(Convert.ToInt32(rowEl[2])) ? true: false, 
                        rowEl[3], 
                        Convert.ToDecimal(rowEl[4]));
                    people.Add(person);
                    await Create(person);
                }
            }

            return View(await _context.Person.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
