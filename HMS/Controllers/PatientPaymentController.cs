using HMS.Data;
using HMS.Models.PaymentsViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PatientPaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<PatientPaymentController> _logger;

        public PatientPaymentController(ApplicationDbContext context, ICommon iCommon, ILogger<PatientPaymentController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.PatientPayments.RoleName)]
        public IActionResult PatientPaymentsList()
        {
            return View();
        }
        [Authorize(Roles = Pages.MainMenu.PatientPayments.RoleName)]
        public IActionResult PatientPaymentsListInBed()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetDataTabelData(string _PatientPaymentsListType)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                IQueryable<PatientPaymentViewModel> _GetGridItem = null;
                if (_PatientPaymentsListType == "PatientPaymentsList")
                {
                    _GetGridItem = _iCommon.GetPatientPaymentsGridItem().Where(x => x.PatientType == "Out Patient");
                }
                else if (_PatientPaymentsListType == "PatientPaymentsListInBed")
                {
                    _GetGridItem = _iCommon.GetPatientPaymentsGridItem().Where(x => x.PatientType == "In Patient");
                }


                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.VisitId.ToLower().Contains(searchValue)
                    || obj.PaymentId.ToString().Contains(searchValue)
                    || obj.PatientName.ToLower().Contains(searchValue)
                    //|| obj.PatientType.ToLower().Contains(searchValue)
                    || obj.DoctorName.ToLower().Contains(searchValue)
                    || obj.CheckupDate.ToString().ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Patient Payment.");
                throw;
            }

        }
    }
}
