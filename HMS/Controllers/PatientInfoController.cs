using HMS.Data;
using HMS.Models;
using HMS.Models.PatientInfoViewModel;
using HMS.Services;
using HMS.Models.CommonViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using HMS.ConHelper;
using HMS.Models.AccountViewModels;
using HMS.Helpers;
using Microsoft.AspNetCore.Identity;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PatientInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IAccount _iAccount;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientInfoController(ApplicationDbContext context, ICommon iCommon, IAccount iAccount, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _iCommon = iCommon;
            _iAccount = iAccount;
            _userManager = userManager;
        }

        [Authorize(Roles = Pages.MainMenu.PatientInfo.RoleName)]
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

                var _GetGridItem = _iCommon.GetPatientInfoGridItem();
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
                    || obj.FirstName.ToLower().Contains(searchValue)
                    || obj.LastName.ToLower().Contains(searchValue)
                    || obj.MaritalStatus.ToLower().Contains(searchValue)
                    || obj.Gender.ToLower().Contains(searchValue)
                    || obj.SpouseName.ToLower().Contains(searchValue)
                    || obj.BloodGroup.ToLower().Contains(searchValue)
                    || obj.DateOfBirth.ToString().ToLower().Contains(searchValue));
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

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            PatientInfoCRUDViewModel vm = await _context.PatientInfo.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            PatientInfoCRUDViewModel vm = new PatientInfoCRUDViewModel();
            if (id > 0) vm = await _context.PatientInfo.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(PatientInfoCRUDViewModel vm)
        {
            try
            {
                string _UserName = HttpContext.User.Identity.Name;
                JsonResultViewModel _JsonResultViewModel = new();
                PatientInfo _PatientInfo = new();
                if (vm.Id > 0)
                {
                    _PatientInfo = await _context.PatientInfo.FindAsync(vm.Id);

                    vm.CreatedDate = _PatientInfo.CreatedDate;
                    vm.CreatedBy = _PatientInfo.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_PatientInfo).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    _JsonResultViewModel.AlertMessage = "Patient Info Updated Successfully. ID: " + _PatientInfo.Id;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {

                    vm.UserType = UserType.Patient;
                    vm.RoleId = 4;
                    var _ApplicationUser = await _iAccount.CreateUserProfile(vm, _UserName);
                    if (_ApplicationUser.Item2 == "Success")
                    {
                        if (vm.ProfilePictureDetails == null)
                            _PatientInfo.ProfilePicture = vm.ProfilePicture;
                        else
                            _PatientInfo.ProfilePicture = "/upload/" + _iCommon.UploadedFile(vm.ProfilePictureDetails);

                        var _Code = "P" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        _PatientInfo = vm;
                        _PatientInfo.ApplicationUserId = _ApplicationUser.Item1.Id;
                        _PatientInfo.PatientCode = _Code;
                        _PatientInfo.CreatedDate = DateTime.Now;
                        _PatientInfo.ModifiedDate = DateTime.Now;
                        _PatientInfo.CreatedBy = _UserName;
                        _PatientInfo.ModifiedBy = _UserName;
                        _context.Add(_PatientInfo);
                        await _context.SaveChangesAsync();

                        _JsonResultViewModel.AlertMessage = "Patient Info Created Successfully. ID: " + _PatientInfo.Id;
                        _JsonResultViewModel.IsSuccess = true;
                        return new JsonResult(_JsonResultViewModel);
                    }
                    else
                    {
                        _JsonResultViewModel.AlertMessage = "User Creation Failed." + _ApplicationUser.Item2;
                        _JsonResultViewModel.IsSuccess = false;
                        return new JsonResult(_JsonResultViewModel);
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _PatientInfo = await _context.PatientInfo.FindAsync(id);
                var _ApplicationUser = await _userManager.FindByIdAsync(_PatientInfo.ApplicationUserId);
                var _DeleteAsync = await _userManager.DeleteAsync(_ApplicationUser);

                if (_DeleteAsync.Succeeded)
                {
                    var _UserProfile = await _context.UserProfile.Where(x => x.ApplicationUserId == _PatientInfo.ApplicationUserId).SingleOrDefaultAsync();
                    _UserProfile.Cancelled = true;
                    _UserProfile.ModifiedDate = DateTime.Now;
                    _UserProfile.ModifiedBy = HttpContext.User.Identity.Name;
                    var result2 = _context.UserProfile.Update(_UserProfile);
                    await _context.SaveChangesAsync();

                    _PatientInfo.ModifiedDate = DateTime.Now;
                    _PatientInfo.ModifiedBy = HttpContext.User.Identity.Name;
                    _PatientInfo.Cancelled = true;
                    _context.Update(_PatientInfo);
                    await _context.SaveChangesAsync();
                }

                return new JsonResult(_PatientInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
