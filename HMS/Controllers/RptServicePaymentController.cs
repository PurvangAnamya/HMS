using HMS.Data;
using HMS.Models.PaymentsDetailsViewModel;
using HMS.Models.ReportViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class RptServicePaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private string _StartDate = null;
        private string _EndDate = null;
        private readonly ILogger<RptServicePaymentController> _logger;

        public RptServicePaymentController(ApplicationDbContext context, ICommon iCommon, ILogger<RptServicePaymentController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.Admin.RoleName)]
        public IActionResult Index(string StartDate, string EndDate)
        {
            if (StartDate != null && EndDate != null)
            {
                HttpContext.Session.SetString("_StartDate", StartDate);
                HttpContext.Session.SetString("_EndDate", EndDate);
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
            }
            else
            {
                HttpContext.Session.SetString("_StartDate", string.Empty);
                HttpContext.Session.SetString("_EndDate", string.Empty);
                ViewBag.StartDate = "Min";
                ViewBag.EndDate = "Max";
            }
            var _GetServicePaymentList = _iCommon.GetServicePaymentList();
            GetServicePaymentSummary(_GetServicePaymentList);
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTableData()
        {
            try
            {
                var abc = ViewBag.StartDate;
                _StartDate = this.HttpContext.Session.GetString("_StartDate");
                _EndDate = this.HttpContext.Session.GetString("_EndDate");

                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                IQueryable<PaymentsDetailsCRUDViewModel> _GetServicePaymentList = null;
                if (_StartDate != null && _EndDate != null && _StartDate != "" && _EndDate != "")
                {
                    _GetServicePaymentList = _iCommon.GetServicePaymentList().Where(x => x.CreatedDate >= Convert.ToDateTime(_StartDate) && x.CreatedDate <= Convert.ToDateTime(_EndDate).AddDays(1));
                }
                else
                {
                    _GetServicePaymentList = _iCommon.GetServicePaymentList();
                }

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetServicePaymentList = _GetServicePaymentList.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetServicePaymentList = _GetServicePaymentList.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.PaymentItemCode.ToLower().Contains(searchValue)
                    || obj.PaymentItemName.ToLower().Contains(searchValue)
                    || obj.Quantity.ToString().ToLower().Contains(searchValue)
                    || obj.UnitPrize.ToString().ToLower().Contains(searchValue)
                    || obj.TotalAmount.ToString().ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetServicePaymentList.Count();
                var result = _GetServicePaymentList.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Rpt Service Payment.");
                throw;
            }

        }

        private void GetServicePaymentSummary(IQueryable<PaymentsDetailsCRUDViewModel> result)
        {
            ServicePaymentSummaryViewModel vm = new ServicePaymentSummaryViewModel();
            vm.LabTotal = result.Where(x => x.PaymentItemCode.Contains("LAB")).Sum(x => x.TotalAmount);
            vm.MedicineTotal = result.Where(x => x.PaymentItemCode.Contains("MED")).Sum(x => x.TotalAmount);
            vm.CommonTotal = result.Where(x => x.PaymentItemCode.Contains("CMN")).Sum(x => x.TotalAmount);
            vm.AllTotal = vm.LabTotal + vm.MedicineTotal + vm.CommonTotal;

            ViewBag.LabTotal = vm.LabTotal;
            ViewBag.MedicineTotal = vm.MedicineTotal;
            ViewBag.CommonTotal = vm.CommonTotal;
            ViewBag.AllTotal = vm.AllTotal;
        }
    }
}
