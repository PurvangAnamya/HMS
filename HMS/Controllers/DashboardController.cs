using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HMS.Data;
using HMS.Models;
using HMS.Models.DashboardViewModel;
using HMS.Services;
using HMS.Models.ReportViewModel;
using HMS.Helpers;
using static HMS.Pages.MainMenu;
using UserProfile = HMS.Models.UserProfile;
using ManageUserRoles = HMS.Models.ManageUserRoles;

namespace HMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public DashboardController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.Dashboard.RoleName)]
        public IActionResult Index()
        {
            try
            {
                DashboardDataViewModel _DashboardDataViewModel = new();
                DashboardSummaryViewModel _DashboardSummaryViewModel = new();
                List<ManageUserRoles> _manageUserRoles = _context.ManageUserRoles.Where(x => x.Cancelled == false).ToList();
                List<UserProfile> _UserProfile = _context.UserProfile.Where(x => x.Cancelled == false).ToList();
                _DashboardSummaryViewModel.TotalDoctor = _UserProfile.Where(x => x.RoleId == UserType.Doctor).Count();
                _DashboardSummaryViewModel.TotalNurse = _UserProfile.Where(x => x.UserType == UserType.Nurse).Count();
                _DashboardSummaryViewModel.TotalPharmacist = _UserProfile.Where(x => x.UserType == UserType.Pharmacist).Count();
                _DashboardSummaryViewModel.TotalLaboratorist = _UserProfile.Where(x => x.UserType == UserType.Laboraties).Count();
                _DashboardSummaryViewModel.TotalAccountant = _UserProfile.Where(x => x.UserType == UserType.Accountants).Count();

                _DashboardSummaryViewModel.TotalPatient = _context.PatientInfo.Where(x => x.Cancelled == false).Count();
                _DashboardSummaryViewModel.TotalBeds = _context.Bed.Where(x => x.Cancelled == false).Count();
                _DashboardSummaryViewModel.TotalMedicines = _context.Medicines.Where(x => x.Cancelled == false).Count();

                foreach (var userRole in _manageUserRoles)
                {
                    if (userRole.Name.ToLower() != "admin")
                    {
                        try
                        {
                            _DashboardDataViewModel.userRolesCountsModel.Add(new UserRoleCountsModel()
                            {
                                RoleId = userRole.Id,
                                RoleName = userRole.Name,
                                UserCounts = _context.UserProfile.Where(x => x.RoleId == userRole.Id && x.Cancelled == false).Count(),
                                LeftMenuImage = _context.UserImages.Where(x => x.Id == userRole.ImageId).FirstOrDefault()?.ImagePath,
                                DashboardImage = _context.UserImages.Where(x => x.Id == userRole.DashboardImageId).FirstOrDefault()?.ImagePath,
                            });
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
                _DashboardDataViewModel.DashboardSummaryViewModel = _DashboardSummaryViewModel;

                _DashboardDataViewModel.listPaymentsCRUDViewModel = _iCommon.GetPaymentDetails().Take(10).ToList();
                _DashboardDataViewModel.listCheckupSummaryCRUDViewModel = _iCommon.GetCheckupGridItem().Take(10).ToList();
                return View(_DashboardDataViewModel);
            }
            catch (Exception ex)
            {
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