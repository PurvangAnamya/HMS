using HMS.ConHelper;
using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.DoctorsInfoViewModel;
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
    public class DoctorsInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ICommon _iCommon;
        private readonly IAccount _iAccount;

        public DoctorsInfoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ICommon iCommon, IAccount iAccount)
        {
            _context = context;
            _iCommon = iCommon;
            _iAccount = iAccount;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [Authorize(Roles = Pages.MainMenu.DoctorsInfo.RoleName)]
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

                var _GetDoctorInfoList = _iCommon.GetDoctorInfoList();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetDoctorInfoList = _GetDoctorInfoList.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetDoctorInfoList = _GetDoctorInfoList.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.ApplicationUserId.ToLower().Contains(searchValue)
                    || obj.FirstName.ToLower().Contains(searchValue)
                    || obj.LastName.ToLower().Contains(searchValue)
                    || obj.PhoneNumber.ToLower().Contains(searchValue)
                    || obj.Email.ToLower().Contains(searchValue)
                    || obj.DesignationDisplay.ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetDoctorInfoList.Count();

                var result = _GetDoctorInfoList.Skip(skip).Take(pageSize).ToList();
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
            DoctorsInfoCRUDViewModel vm = await _iCommon.GetDoctorInfoList().Where(x => x.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.ddlManageUserRoles = new SelectList(_iCommon.GetTableData<ManageUserRoles>(_context), "Id", "Name");         
            ViewBag.ddlDesignation = new SelectList(_iCommon.GetTableData<Designation>(_context), "Id", "Name");
            if (id > 0)
            {
                ManageDoctorInfoViewModel vm = new();
                vm.UserProfileCRUDViewModel = await _context.UserProfile.Where(x => x.UserProfileId == id).SingleOrDefaultAsync();
                vm.DoctorsInfoEditViewModel = await _context.DoctorsInfo.Where(x => x.ApplicationUserId == vm.UserProfileCRUDViewModel.ApplicationUserId).SingleOrDefaultAsync();
                return PartialView("_Edit", vm);
            }
            return PartialView("_Add", new DoctorsInfoCRUDViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DoctorsInfoCRUDViewModel _DoctorsInfoCRUDViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _DoctorsInfoCRUDViewModel.UserType = UserType.Doctor;
                    var _ApplicationUser = await _iAccount.CreateUserProfile(_DoctorsInfoCRUDViewModel, HttpContext.User.Identity.Name);
                    if (_ApplicationUser.Item2 == "Success")
                    {
                        DoctorsInfo _DoctorsInfo = new()
                        {
                            ApplicationUserId = _ApplicationUser.Item1.Id,
                            DesignationId = _DoctorsInfoCRUDViewModel.DesignationId,
                            DoctorsID = _DoctorsInfoCRUDViewModel.DoctorsID,
                            DoctorFee = _DoctorsInfoCRUDViewModel.DoctorFee,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = HttpContext.User.Identity.Name,
                            ModifiedBy = HttpContext.User.Identity.Name
                        };

                        await _context.DoctorsInfo.AddAsync(_DoctorsInfo);
                        var result = await _context.SaveChangesAsync();

                        var _DefaultIdentityOptions = await _context.DefaultIdentityOptions.FirstOrDefaultAsync(m => m.Id == 1);
                        if (_DefaultIdentityOptions.SignInRequireConfirmedEmail)
                        {
                            var _ConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(_ApplicationUser.Item1);
                            var callbackUrl = Url.EmailConfirmationLink(_ApplicationUser.Item1.Id, _ConfirmationToken, Request.Scheme);
                            await _emailSender.SendEmailConfirmationAsync(_ApplicationUser.Item1.Email, callbackUrl);
                        }

                        TempData["successAlert"] = "Doctors User Created Successfully. ID: " + _ApplicationUser.Item1.Email;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["errorAlert"] = "Doctors User Creation Failed." + _ApplicationUser.Item2;
                        return View("Index");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsExists(_DoctorsInfoCRUDViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(_DoctorsInfoCRUDViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManageDoctorInfoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string errorMessage = string.Empty;
                DoctorsInfo _DoctorsInfo = new DoctorsInfo();
                try
                {
                    if (ModelState.IsValid)
                    {
                        _DoctorsInfo = await _context.DoctorsInfo.FindAsync(vm.DoctorsInfoEditViewModel.Id);
                        vm.UserProfileCRUDViewModel.ModifiedDate = DateTime.Now;
                        vm.UserProfileCRUDViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_DoctorsInfo).CurrentValues.SetValues(vm.DoctorsInfoEditViewModel);
                        await _context.SaveChangesAsync();

                        //UserProfile...
                        UserProfile _UserProfile = await _context.UserProfile.FindAsync(vm.UserProfileCRUDViewModel.UserProfileId);
                        if (vm.UserProfileCRUDViewModel.ProfilePictureDetails != null)
                            vm.UserProfileCRUDViewModel.ProfilePicture = "/upload/" + _iCommon.UploadedFile(vm.UserProfileCRUDViewModel.ProfilePictureDetails);

                        vm.UserProfileCRUDViewModel.ModifiedDate = DateTime.Now;
                        vm.UserProfileCRUDViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_UserProfile).CurrentValues.SetValues(vm.UserProfileCRUDViewModel);
                        await _context.SaveChangesAsync();

                        TempData["successAlert"] = "Doctors Info Updated Successfully. ID: " + _DoctorsInfo.Id;
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["errorAlert"] = "Doctors User Update Failed." + errorMessage;
                    return View("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsExists(_DoctorsInfo.Id))
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

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _DoctorsInfo = await _context.DoctorsInfo.FindAsync(id);
                _DoctorsInfo.ModifiedDate = DateTime.Now;
                _DoctorsInfo.ModifiedBy = HttpContext.User.Identity.Name;
                _DoctorsInfo.Cancelled = true;

                _context.Update(_DoctorsInfo);
                await _context.SaveChangesAsync();
                return new JsonResult(_DoctorsInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.DoctorsInfo.Any(e => e.Id == id);
        }
    }
}
