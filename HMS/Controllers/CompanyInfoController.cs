using HMS.Data;
using HMS.Models;
using HMS.Models.CompanyInfoViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CompanyInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<CompanyInfoController> _logger;

        public CompanyInfoController(ApplicationDbContext context, ICommon iCommon, ILogger<CompanyInfoController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.CompanyInfo.RoleName)]
        public async Task<IActionResult> Index()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return View(vm);
        }


        public async Task<IActionResult> Edit()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyInfoCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        CompanyInfo _CompanyInfo = new CompanyInfo();
                        _CompanyInfo = await _context.CompanyInfo.FindAsync(vm.Id);
                        if (vm.CompanyLogo != null)
                            vm.CompanyLogoImagePath = "/upload/" + _iCommon.UploadedFile(vm.CompanyLogo);
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_CompanyInfo).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        TempData["successAlert"] = "Company Info Updated Successfully. Company Name: " + _CompanyInfo.Name;
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["errorAlert"] = "Operation failed.";
                    return View("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError( "Error in Updated Company Info.");
                        throw;
                    }
                }
            }
            return View(vm);
        }

        private bool IsExists(long id)
        {
            return _context.CompanyInfo.Any(e => e.Id == id);
        }
    }
}

