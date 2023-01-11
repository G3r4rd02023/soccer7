using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soccer.Data;
using Soccer.Data.Entities;
using Soccer.Helpers;
using Soccer.Models;
using Vereyon.Web;

namespace Soccer.Controllers
{
    public class TeamsController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IImageHelper _imageHelper;

        public TeamsController(DataContext context, IFlashMessage flashMessage,IImageHelper imageHelper)
        {
            _context = context;
            _flashMessage = flashMessage;  
            _imageHelper = imageHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams                
                .ToListAsync());
        }

        public IActionResult Create()
        {           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "teams");
                }

                Team team = new()
                {                    
                    Name = model.Name, 
                    ImageUrl = path,
                };

                try
                {
                    _context.Add(team);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro creado.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un equipo con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
