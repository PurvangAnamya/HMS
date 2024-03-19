using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HMS.Data;
using HMS.Models;
using HMS.Models.DashboardViewModel;
using HMS.Services;
using HMS.Models.ReportViewModel;
using HMS.Helpers;

namespace HMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(ApplicationDbContext context, ICommon iCommon, ILogger<DashboardController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.Dashboard.RoleName)]
        public IActionResult Index()
        {
            try
            {
                DashboardDataViewModel _DashboardDataViewModel = new();
                DashboardSummaryViewModel _DashboardSummaryViewModel = new();

                List<UserProfile> _UserProfile = _context.UserProfile.Where(x => x.Cancelled == false).ToList();
                _DashboardSummaryViewModel.TotalDoctor = _UserProfile.Where(x => x.UserType == UserType.Doctor).Count();
                _DashboardSummaryViewModel.TotalNurse = _UserProfile.Where(x => x.UserType == UserType.Nurse).Count();
                _DashboardSummaryViewModel.TotalPharmacist = _UserProfile.Where(x => x.UserType == UserType.Pharmacist).Count();
                _DashboardSummaryViewModel.TotalLaboratorist = _UserProfile.Where(x => x.UserType == UserType.Laboraties).Count();
                _DashboardSummaryViewModel.TotalAccountant = _UserProfile.Where(x => x.UserType == UserType.Accountants).Count();

                _DashboardSummaryViewModel.TotalPatient = _context.PatientInfo.Where(x => x.Cancelled == false).Count();
                _DashboardSummaryViewModel.TotalBeds = _context.Bed.Where(x => x.Cancelled == false).Count();
                _DashboardSummaryViewModel.TotalMedicines = _context.Medicines.Where(x => x.Cancelled == false).Count();

                _DashboardDataViewModel.DashboardSummaryViewModel = _DashboardSummaryViewModel;

                _DashboardDataViewModel.listPaymentsCRUDViewModel = _iCommon.GetPaymentDetails().Take(10).ToList();
                _DashboardDataViewModel.listCheckupSummaryCRUDViewModel = _iCommon.GetCheckupGridItem().Take(10).ToList();
                return View(_DashboardDataViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Dashboard.");
                throw;
            }
        }



        [HttpGet]
        public JsonResult GetPaymentsDetailsGroupBy()
        {
            var _PaymentsDetails = _context.PaymentsDetails.Where(x => x.Cancelled == false).ToList();
            List<PaymentsDetailsPieChartViewModel> listPaymentsDetailsPieChartViewModel = new List<PaymentsDetailsPieChartViewModel>();
            PaymentsDetailsPieChartViewModel vm = new();
            vm.PaymentCategoriesName = "Consulting Charge";
            vm.PaymentCategoriesTotalAmount = (double)_PaymentsDetails.Where(x => x.PaymentItemCode == "CMN20211006022754526").Sum(x => x.TotalAmount);
            listPaymentsDetailsPieChartViewModel.Add(vm);

            vm = new PaymentsDetailsPieChartViewModel();
            vm.PaymentCategoriesName = "Medicine Item Sell";
            vm.PaymentCategoriesTotalAmount = (double)_PaymentsDetails.Where(x => x.PaymentItemCode.Contains("MED")).Sum(x => x.TotalAmount);
            listPaymentsDetailsPieChartViewModel.Add(vm);

            vm = new PaymentsDetailsPieChartViewModel();
            vm.PaymentCategoriesName = "Lab Test Item Sell";
            vm.PaymentCategoriesTotalAmount = (double)_PaymentsDetails.Where(x => x.PaymentItemCode.Contains("LAB")).Sum(x => x.TotalAmount);
            listPaymentsDetailsPieChartViewModel.Add(vm);

            vm = new PaymentsDetailsPieChartViewModel();
            vm.PaymentCategoriesName = "Common Item Sell";
            vm.PaymentCategoriesTotalAmount = (double)_PaymentsDetails.Where(x => x.PaymentItemCode.Contains("CMN")).Sum(x => x.TotalAmount);
            listPaymentsDetailsPieChartViewModel.Add(vm);

            return new JsonResult(listPaymentsDetailsPieChartViewModel.ToDictionary(x => x.PaymentCategoriesName, x => x.PaymentCategoriesTotalAmount));
        }
    }
}