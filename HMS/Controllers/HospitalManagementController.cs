using HMS.ConHelper;
using HMS.Data;
using HMS.Models;
using HMS.Models.HospitalViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class HospitalManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommon _iCommon;
        private readonly IEmailSender _emailSender;
        private readonly IAccount _iAccount;
        private readonly IRoles _roles;
        public HospitalManagementController(UserManager<ApplicationUser> userManager,
           ApplicationDbContext context,
           ICommon iCommon,
           IEmailSender emailSender,
           IAccount iAccount,
           IRoles roles)
        {
            _context = context;
            _userManager = userManager;
            _iCommon = iCommon;
            _emailSender = emailSender;
            _iAccount = iAccount;
            _roles = roles;
        }

        [Authorize(Roles = Pages.MainMenu.HospitalManagement.RoleName)]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetDataTabelData()
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

                var _GetGridItem = GetGridItem();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    //_GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.HospitalName.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.Address.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }
        }
        private IQueryable<HospitalViewModel> GetGridItem()
        {
            try
            {
                return (from _Hospital in _context.Hospital
                        select new HospitalViewModel
                        {
                            Id = _Hospital.Id,
                            HospitalName = _Hospital.HospitalName,
                            Description = _Hospital.Description,
                            Address = _Hospital.Address,
                            HospitalLogoImagePath = _Hospital.HospitalLogoImagePath,
                            CreatedDate = _Hospital.CreatedDate,
                            ModifiedDate = _Hospital.ModifiedDate,
                            CreatedBy = _Hospital.CreatedBy,
                            ModifiedBy = _Hospital.ModifiedBy,

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            HospitalCRUDViewModel vm = await _context.Hospital.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        public async Task<IActionResult> AddEdit(int id)
        {
            HospitalCRUDViewModel vm = new HospitalCRUDViewModel();
            if (id > 0) vm = await _context.Hospital.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(HospitalCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Hospital _hospital = new Hospital();
                        if (vm.Id > 0)
                        {
                            _hospital = await _context.Hospital.FindAsync(vm.Id);
                            vm.HospitalName = _hospital.HospitalName;
                            vm.Description = _hospital.Description;
                            vm.Address = _hospital.Address;
                            if (vm.HospitalLogo != null)
                                vm.HospitalLogoImagePath = "/upload/" + _iCommon.UploadedFile(vm.HospitalLogo);
                            vm.CreatedDate = _hospital.CreatedDate;
                            vm.CreatedBy = _hospital.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_hospital).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Hospital Updated Successfully. ID: " + _hospital.Id;
                            TempData["hospitalId"] = _hospital.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _hospital = vm;
                            vm.HospitalName = _hospital.HospitalName;
                            vm.Description = _hospital.Description;
                            vm.Address = _hospital.Address;
                            _hospital.CreatedDate = DateTime.Now;
                            _hospital.ModifiedDate = DateTime.Now;
                            _hospital.CreatedBy = HttpContext.User.Identity.Name;
                            _hospital.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_hospital);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Bed Categories Created Successfully. ID: " + _hospital.Id;
                            TempData["hospitalId"] = _hospital.Id;
                            await AddHospitalintoMasterTables(_hospital.Id);
                            return RedirectToAction(nameof(Index));
                        }
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
                        throw;
                    }
                }
            }
            return View(vm);
        }

        private async Task AddHospitalintoMasterTables(long hospitalId)
        {
            try
            {
                SeedData _SeedData = new();
                var _GetExpenseCategoriesList = _SeedData.GetExpenseCategoriesList();
                foreach (var item in _GetExpenseCategoriesList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.ExpenseCategories.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetPaymentCategoriesList = _SeedData.GetPaymentCategoriesList();
                foreach (var item in _GetPaymentCategoriesList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.PaymentCategories.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetInsuranceCompanyInfoList = _SeedData.GetInsuranceCompanyInfoList();
                foreach (var item in _GetInsuranceCompanyInfoList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.InsuranceCompanyInfo.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetBedCategoriesList = _SeedData.GetBedCategoriesList();
                foreach (var item in _GetBedCategoriesList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    _context.BedCategories.Add(item);
                    item.HospitalId = hospitalId;
                    await _context.SaveChangesAsync();
                }

                var _GetBedList = _SeedData.GetBedList();
                foreach (var item in _GetBedList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.Bed.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetLabTestCategoriesList = _SeedData.GetLabTestCategoriesList();
                foreach (var item in _GetLabTestCategoriesList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.LabTestCategories.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetLabTestsList = _SeedData.GetLabTestsList();
                foreach (var item in _GetLabTestsList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.LabTests.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetMedicineCategoriesList = _SeedData.GetMedicineCategoriesList();
                foreach (var item in _GetMedicineCategoriesList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.MedicineCategories.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetUnitList = _SeedData.GetUnitList();
                foreach (var item in _GetUnitList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.Unit.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetMedicineManufactureList = _SeedData.GetMedicineManufactureList();
                foreach (var item in _GetMedicineManufactureList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.MedicineManufacture.Add(item);
                    await _context.SaveChangesAsync();
                }
                var _GetMedicinesList = _SeedData.GetMedicinesList();
                foreach (var item in _GetMedicinesList)
                {
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    item.HospitalId = hospitalId;
                    _context.Medicines.Add(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _hospital = await _context.Hospital.FindAsync(id);
                _hospital.ModifiedDate = DateTime.Now;
                _hospital.ModifiedBy = HttpContext.User.Identity.Name;
                _hospital.Cancelled = true;

                _context.Update(_hospital);
                await _context.SaveChangesAsync();
                return new JsonResult(_hospital);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Hospital.Any(e => e.Id == id);
        }
    }
}
