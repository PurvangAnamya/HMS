using DataTablesParser;
using HMS.Data;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PatientPresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public PatientPresController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.PatientPrescriptions.RoleName)]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetDataTabelData()
        {
            try
            {
                var _UserName = HttpContext.User.Identity.Name;
                List<CheckupSummaryCRUDViewModel> listCheckupSummary = new();
                var _PatientInfo = await _context.PatientInfo.Where(x => x.Email == _UserName && x.Cancelled == false).SingleOrDefaultAsync();
                if (_PatientInfo != null)
                {
                    var _PatientAppointment = await _context.PatientAppointment.Where(x => x.PatientId == _PatientInfo.Id && x.Cancelled == false).SingleOrDefaultAsync();
                    if (_PatientAppointment != null)
                    {
                        listCheckupSummary = await _iCommon.GetCheckupGridItem().Where(x => x.PatientId == _PatientInfo.Id && x.Cancelled == false).ToListAsync();
                    }
                }
                var _Parser = new Parser<CheckupSummaryCRUDViewModel>(Request.Form, listCheckupSummary.AsQueryable());
                return Json(_Parser.Parse());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
