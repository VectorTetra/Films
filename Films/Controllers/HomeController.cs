using Films.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Films.Controllers
{
    public class HomeController : Controller
    {
        private readonly FilmContext _context;
        // IWebHostEnvironment предоставляет информацию об окружении, в котором запущено приложение
        IWebHostEnvironment _appEnvironment;
        public HomeController(FilmContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Films.ToListAsync());
        }

        public IActionResult Add()
        {
            return View();
        }
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IFormFile uploadedFile, [Bind("Name,Director,Description,ReleaseYear,Genre")] Film film)
        {
            try
            {
                if (film.Description.Length < 20)
                {
                    ModelState.AddModelError("", "Довжина опису фільму - мінімум 20 символів");
                }
                if (uploadedFile != null && ModelState.IsValid)
                {
                    // Путь к папке Files
                    string path = "/Files/" + uploadedFile.FileName; // имя файла
                    string vpath = "~" + path;
                    // Сохраняем файл в папку Files в каталоге wwwroot
                    // Для получения полного пути к каталогу wwwroot
                    // применяется свойство WebRootPath объекта IWebHostEnvironment
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    //Film film = new Film { Name = Name, ReleaseYear = ReleaseYear, Genre = Genre, Director = Director, Description = Description };
                    film.PosterPath = vpath;
                    _context.Films.Add(film);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(film);
                }
                
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", ex.Message);
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.SingleOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] IFormFile fileinput, int id, Film film)
        {
            
            try
            {
                if (id != film.Id)
                {
                    return NotFound();
                }
                if (film.Description.Length < 20)
                {
                    ModelState.AddModelError("", "Довжина опису фільму - мінімум 20 символів");
                }
                if (fileinput != null)
                {
                    // Путь к папке Files
                    string path = "/Files/" + fileinput.FileName; // имя файла
                    string vpath = "~" + path;
                    // Сохраняем файл в папку Files в каталоге wwwroot
                    // Для получения полного пути к каталогу wwwroot
                    // применяется свойство WebRootPath объекта IWebHostEnvironment
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await fileinput.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    //Film film = new Film { Name = Name, ReleaseYear = ReleaseYear, Genre = Genre, Director = Director, Description = Description };
                    film.PosterPath = vpath;
                }
                if (ModelState.IsValid)
                {
                _context.Update(film);
                await _context.SaveChangesAsync();
                }
                else { return View(film); }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(film.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
           
            //return View(film);
        }

        // GET: Films/Delete/Id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .SingleOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Films.SingleOrDefaultAsync(m => m.Id == id);
            _context.Films.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Films/Details/Id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .SingleOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}