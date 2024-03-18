using HMS.ConHelper;
using HMS.Data;
using HMS.Models;
using HMS.Models.CommonViewModel;
using HMS.Models.ManageUserRolesVM;
using HMS.Models.UserProfileViewModel;
using HMS.Helpers;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommon _iCommon;
        private readonly IEmailSender _emailSender;
        private readonly IAccount _iAccount;
        private readonly IRoles _roles;

        public UserManagementController(UserManager<ApplicationUser> userManager,
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

        [Authorize(Roles = Pages.MainMenu.UserManagement.RoleName)]
        public IActionResult IndexAllUser()
        {
            return View();
        }
        [Authorize(Roles = Pages.MainMenu.Nurse.RoleName)]
        public IActionResult IndexNurse()
        {
            return View();
        }
        [Authorize(Roles = Pages.MainMenu.Laboraties.RoleName)]
        public IActionResult IndexLaboraties()
        {
            return View();
        }
        [Authorize(Roles = Pages.MainMenu.Accountants.RoleName)]
        public IActionResult IndexAccountants()
        {
            return View();
        }
        [Authorize(Roles = Pages.MainMenu.Pharmacist.RoleName)]
        public IActionResult IndexPharmacist()
        {
            return View();
        }


        

        [HttpPost]
        public IActionResult GetDataTabelData(int _UserType)
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
                IQueryable<UserProfile> _AccountUser = null;
                if (_UserType == UserType.General)
                {
                    _AccountUser = _context.UserProfile.Where(x => x.Cancelled == false);
                }
                else
                {
                    _AccountUser = _context.UserProfile.Where(x => x.Cancelled == false && x.UserType == _UserType);
                }

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _AccountUser = _AccountUser.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _AccountUser = _AccountUser.Where(obj => obj.FirstName.ToLower().Contains(searchValue)
                    || obj.LastName.ToLower().Contains(searchValue)
                    || obj.PhoneNumber.ToLower().Contains(searchValue)
                    || obj.Email.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _AccountUser.Count();

                var result = _AccountUser.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception) { throw; }
        }

        [HttpGet]
        public async Task<IActionResult> ViewUserDetails(Int64 id)
        {
            var result = await _iCommon.GetUserProfileDetails().Where(x => x.UserProfileId == id).SingleOrDefaultAsync();
            ManageUserRolesCRUDViewModel _ManageUserRolesCRUDViewModel = new();
            _ManageUserRolesCRUDViewModel.listManageUserRolesDetails = await _iCommon.GetManageRoleDetailsList(result.RoleId);
            result.ManageUserRolesCRUDViewModel = _ManageUserRolesCRUDViewModel;
            return PartialView("_ViewUserDetails", result);
        }

        [HttpGet]
        public IActionResult AddEditUserAccount(Int64 id)
        {
            ViewBag.ddlManageUserRoles = new SelectList(_iCommon.GetTableData<ManageUserRoles>(_context), "Id", "Name");
            ViewBag.ddlDepartment = new SelectList(_iCommon.GetTableData<Department>(_context), "Id", "Name");
            ViewBag.ddlSubDepartment = new SelectList(_iCommon.GetTableData<SubDepartment>(_context), "Id", "Name");
            ViewBag.ddlDesignation = new SelectList(_iCommon.GetTableData<Designation>(_context), "Id", "Name");

            UserProfileCRUDViewModel _UserProfileViewModel = new();
            if (id > 0)
            {
                _UserProfileViewModel = _iCommon.GetByUserProfile(id);
                return PartialView("_EditUserAccount", _UserProfileViewModel);
            }
            return PartialView("_AddUserAccount", _UserProfileViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddEditUserAccount(UserProfileCRUDViewModel _UserProfileViewModel)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                if (_UserProfileViewModel.UserProfileId > 0)
                {
                    UserProfile _UserProfile = _iCommon.GetByUserProfile(_UserProfileViewModel.UserProfileId);
                    if (_UserProfileViewModel.ProfilePictureDetails != null)
                        _UserProfileViewModel.ProfilePicture = "/upload/" + _iCommon.UploadedFile(_UserProfileViewModel.ProfilePictureDetails);
                    else
                        _UserProfileViewModel.ProfilePicture = _UserProfile.ProfilePicture;

                    Int64 _CurrentRoleId = _UserProfile.RoleId;
                    _UserProfileViewModel.ModifiedDate = DateTime.Now;
                    _UserProfileViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                    _UserProfileViewModel.CreatedDate = _UserProfile.CreatedDate;
                    _UserProfileViewModel.CreatedBy = _UserProfile.CreatedBy;
                    _UserProfileViewModel.UserType = _UserProfile.UserType;
                    _context.Entry(_UserProfile).CurrentValues.SetValues(_UserProfileViewModel);
                    await _context.SaveChangesAsync();

                    if (_CurrentRoleId != _UserProfileViewModel.RoleId)
                    {
                        var _ManageRoleDetails = await _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == _UserProfileViewModel.RoleId && x.IsAllowed == true).ToListAsync();
                        ManageUserRolesCRUDViewModel _ManageUserRolesCRUDViewModel = new() 
                        {
                            ApplicationUserId = _UserProfileViewModel.ApplicationUserId,
                            listManageUserRolesDetails = _ManageRoleDetails,
                        };
                        await _roles.UpdateUserRoles(_ManageUserRolesCRUDViewModel);
                    }
                        

                    _JsonResultViewModel.AlertMessage = "User info Updated Successfully. User Name: " + _UserProfile.Email;
                    _JsonResultViewModel.CurrentURL = _UserProfileViewModel.CurrentURL;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    var _ApplicationUser = await _iAccount.CreateUserProfile(_UserProfileViewModel, HttpContext.User.Identity.Name);
                    if (_ApplicationUser.Item2 == "Success")
                    {
                        var _DefaultIdentityOptions = await _context.DefaultIdentityOptions.FirstOrDefaultAsync(m => m.Id == 1);
                        if (_DefaultIdentityOptions.SignInRequireConfirmedEmail)
                        {
                            var _ConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(_ApplicationUser.Item1);
                            var callbackUrl = Url.EmailConfirmationLink(_ApplicationUser.Item1.Id, _ConfirmationToken, Request.Scheme);
                            await _emailSender.SendEmailConfirmationAsync(_ApplicationUser.Item1.Email, callbackUrl);
                        }

                        _JsonResultViewModel.AlertMessage = "User Created Successfully. User Name: " + _ApplicationUser.Item1.Email;
                        _JsonResultViewModel.CurrentURL = _UserProfileViewModel.CurrentURL;
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
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAdmin(Int64 id)
        {
            UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            var _ApplicationUser = await _userManager.FindByIdAsync(_UserProfile.ApplicationUserId);
            ResetPasswordViewModel _ResetPasswordViewModel = new ResetPasswordViewModel();
            _ResetPasswordViewModel.ApplicationUserId = _ApplicationUser.Id;
            return PartialView("_ResetPasswordAdmin", _ResetPasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAdmin(ResetPasswordViewModel vm)
        {
            try
            {
                string AlertMessage = string.Empty;
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (vm.NewPassword.Equals(vm.ConfirmPassword))
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(_ApplicationUser);
                    var _ResetPasswordAsync = await _userManager.ResetPasswordAsync(_ApplicationUser, code, vm.NewPassword);
                    if (_ResetPasswordAsync.Succeeded)
                        AlertMessage = "Reset Password Succeeded. User name: " + _ApplicationUser.Email;
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in _ResetPasswordAsync.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        AlertMessage = "error Reset password failed." + errorMessage;
                    }
                }
                return new JsonResult(AlertMessage);
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAccount(int id)
        {
            UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            var _ApplicationUser = await _userManager.FindByIdAsync(_UserProfile.ApplicationUserId);
            var _DeleteAsync = await _userManager.DeleteAsync(_ApplicationUser);

            if (_DeleteAsync.Succeeded)
            {
                _UserProfile.Cancelled = true;
                _UserProfile.ModifiedDate = DateTime.Now;
                _UserProfile.ModifiedBy = HttpContext.User.Identity.Name;
                var result2 = _context.UserProfile.Update(_UserProfile);
                await _context.SaveChangesAsync();
            }
            return new JsonResult(_UserProfile);
        }
        private async Task<bool> UpdateUserRole(UserProfileCRUDViewModel vm)
        {
            var _ManageRoleDetails = await _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == vm.RoleId && x.IsAllowed == true).ToListAsync();
            var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
            var roles = await _userManager.GetRolesAsync(_ApplicationUser);
            var result = await _userManager.RemoveFromRolesAsync(_ApplicationUser, roles);
            foreach (var item in _ManageRoleDetails)
            {
                await _userManager.AddToRoleAsync(_ApplicationUser, item.RoleName);
            }
            return true;
        }
    }
}