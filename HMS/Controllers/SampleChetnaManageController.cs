using HMS.Data;
using HMS.Models;
using HMS.Models.CommonViewModel;
using HMS.Models.ManageUserRolesVM;
using HMS.Models.SampleChetnaManageVM;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static HMS.Pages.MainMenu;


namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SampleChetnaManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoles _roles;

        public SampleChetnaManageController(ApplicationDbContext context, ICommon iCommon, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IRoles roles)
        {
            _context = context;
            _iCommon = iCommon;
            _userManager = userManager;
            _roleManager = roleManager;
            _roles = roles;
        }

        [Authorize(Roles = Pages.MainMenu.SampleChetnaManage.RoleName)]
        [HttpGet]
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
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.Title.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    //|| obj.ShortDescription.ToLower().Contains(searchValue)
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

        private IQueryable<SampleChetnaManageCURDViewModel> GetGridItem()
        {
            try
            {
                //return (from _SampleChetnaManage in _context.SampleChetnaManage
                //        join _UserImage in _context.UserImages on _SampleChetnaManage.ImageId equals _UserImage.Id
                //        where _SampleChetnaManage.Cancelled == false
                //         select new SampleChetnaManageCURDViewModel
                //         {
                //             Id = _SampleChetnaManage.Id,
                //             Title = _SampleChetnaManage.Title,
                //             Description = _SampleChetnaManage.Description,
                //             DateOfBirth = _SampleChetnaManage.DateOfBirth,
                //             ImageId= _SampleChetnaManage.ImageId,
                //             ProfilePicture = _UserImage.ImagePath,
                //             CreatedDate = _SampleChetnaManage.CreatedDate,
                //             ModifiedDate = _SampleChetnaManage.ModifiedDate,
                //             CreatedBy = _SampleChetnaManage.CreatedBy,
                //             ModifiedBy = _SampleChetnaManage.ModifiedBy,
                //         }).OrderByDescending(x => x.Id);
                return (from _SampleChetnaManage in _context.SampleChetnaManage
                        join _UserImage in _context.UserImages
                            on _SampleChetnaManage.ImageId equals _UserImage.Id into userImageGroup
                        from userImage in userImageGroup.DefaultIfEmpty() // left join
                        where _SampleChetnaManage.Cancelled == false
                        select new SampleChetnaManageCURDViewModel
                        {
                            Id = _SampleChetnaManage.Id,
                            Title = _SampleChetnaManage.Title,
                            Description = _SampleChetnaManage.Description,
                            DateOfBirth = _SampleChetnaManage.DateOfBirth,
                            ImageId = _SampleChetnaManage.ImageId,
                            ProfilePicture = userImage != null ? userImage.ImagePath : "/upload/blank-person.png", // null check for left join
                            CreatedDate = _SampleChetnaManage.CreatedDate,
                            ModifiedDate = _SampleChetnaManage.ModifiedDate,
                            CreatedBy = _SampleChetnaManage.CreatedBy,
                            ModifiedBy = _SampleChetnaManage.ModifiedBy,
                        }).OrderByDescending(x => x.Id);
            
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(Int64 id)
        {
            SampleChetnaManageCURDViewModel vm = await _context.SampleChetnaManage.FirstOrDefaultAsync(m => m.Id == id);
            vm.listSampleChetnaManageRoleDetails = await _iCommon.GetSampleChetnaManageRoleDetailsList(id);
            return PartialView("_Info", vm);
        }
        [HttpGet]


        [HttpGet]
        public async Task<IActionResult> AddEdit(Int64 id)
        {
            SampleChetnaManageCURDViewModel vm = new();
            if (id > 0)
            {
                vm = await _context.SampleChetnaManage.Where(x => x.Id == id).SingleOrDefaultAsync();
                vm.listSampleChetnaManageRoleDetails = await _iCommon.GetSampleChetnaManageRoleDetailsList(id);
                var userImage = await _context.UserImages.Where(x => x.Id == vm.ImageId).SingleOrDefaultAsync();
                if (userImage != null)
                {
                    vm.ProfilePicture = userImage.ImagePath;
                }
            }
            else
            {
                vm.listSampleChetnaManageRoleDetails = await _roles.GetSampleChetnaManageRoleList();
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SampleChetnaManageCURDViewModel vm)
        {
            try
            {
                Models.SampleChetnaManage _SampleChetnaManage = new();
                var _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _SampleChetnaManage = await _context.SampleChetnaManage.FindAsync(vm.Id);

                    // Check if a new image file is provided
                    if (vm.ProfilePictureDetails != null)
                    {
                        // Upload the new image file and set ProfilePicture
                        vm.ProfilePicture = "/upload/" + _iCommon.UploadedFile(vm.ProfilePictureDetails);
                    }

                    // Retain existing ProfilePicture value if ProfilePictureDetails is null
                    else
                    {
                        vm.ProfilePicture = _SampleChetnaManage.ProfilePicture;
                    }

                    vm.ImageId = _iCommon.GetImageFileDetails(_UserName, vm.ProfilePictureDetails, "SampleChetna", vm.ImageId);
                    // Update other properties
                    vm.CreatedDate = _SampleChetnaManage.CreatedDate;
                    vm.CreatedBy = _SampleChetnaManage.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_SampleChetnaManage).CurrentValues.SetValues(vm);
                   await _context.SaveChangesAsync();


                    foreach (var item in vm.listSampleChetnaManageRoleDetails)
                    {
                        var _SampleChetnaManageRoleDetails = await _context.SampleChetnaManageRoleDetails.FindAsync(item.Id);
                        _SampleChetnaManageRoleDetails.IsAllowed = item.IsAllowed;
                        _context.SampleChetnaManageRoleDetails.Update(_SampleChetnaManageRoleDetails);
                        await _context.SaveChangesAsync();
                    }

                    

                    // Update SampleChetnaManageDetails

                    var _AlertMessage = "User Role Updated Successfully. ID: " + _SampleChetnaManage.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    // Add new record

                    _SampleChetnaManage = vm;
                    _SampleChetnaManage.CreatedDate = DateTime.Now;
                    _SampleChetnaManage.ModifiedDate = DateTime.Now;
                    _SampleChetnaManage.CreatedBy = _UserName;
                    _SampleChetnaManage.ModifiedBy = _UserName;

                    // Set default image path using UploadedFile method
                    _SampleChetnaManage.ProfilePicture = "/upload/" + _iCommon.UploadedFile(vm.ProfilePictureDetails);

                    _SampleChetnaManage.ImageId = _iCommon.GetImageFileDetails(_UserName, vm.ProfilePictureDetails, "SampleChetna",_SampleChetnaManage.ImageId);
                    _context.Add(_SampleChetnaManage);
                    await _context.SaveChangesAsync();
                    foreach (var item in vm.listSampleChetnaManageRoleDetails)
                    {
                        SampleChetnaManageRoleDetails _SampleChetnaManageRoleDetails = new();

                        // Populate ManageSampleJalakRolesDetails properties
                        _SampleChetnaManageRoleDetails.ManageRoleId = _SampleChetnaManage.Id;
                        _SampleChetnaManageRoleDetails.RoleId = item.RoleId;
                        _SampleChetnaManageRoleDetails.RoleName = item.RoleName;
                        _SampleChetnaManageRoleDetails.IsAllowed = item.IsAllowed;

                        _SampleChetnaManageRoleDetails.CreatedDate = DateTime.Now;
                        _SampleChetnaManageRoleDetails.ModifiedDate = DateTime.Now;
                        _SampleChetnaManageRoleDetails.CreatedBy = _UserName;
                        _SampleChetnaManageRoleDetails.ModifiedBy = _UserName;

                        _context.Add(_SampleChetnaManageRoleDetails);
                        await _context.SaveChangesAsync();
                    }

                   

                    // Add SampleChetnaManageDetails

                    var _AlertMessage = "User Role Created Successfully. ID: " + _SampleChetnaManage.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
                throw;
            }
        }

       

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _SampleChetnaManage = await _context.SampleChetnaManage.FindAsync(id);
                _SampleChetnaManage.ModifiedDate = DateTime.Now;
                _SampleChetnaManage.ModifiedBy = HttpContext.User.Identity.Name;
                _SampleChetnaManage.Cancelled = true;

                _context.Update(_SampleChetnaManage);
                await _context.SaveChangesAsync();
                return new JsonResult(_SampleChetnaManage);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUserRole(Int64 id)
        {
            SampleChetnaManageCURDViewModel vm = new();
            Models.UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            var _listIdentityRole = _roleManager.Roles.ToList();

            GetSampleChetnaManageByUserVM _GetSampleChetnaManageByUserVM = new()
            {
                ApplicationUserId = _UserProfile.ApplicationUserId,
                UserManager = _userManager,
                listIdentityRole = _listIdentityRole
            };
            vm.listSampleChetnaManageRoleDetails = await _roles.GetSampleChetnaManageByUser(_GetSampleChetnaManageByUserVM);
            vm.ApplicationUserId = _UserProfile.ApplicationUserId;
            return PartialView("_UpdateRoleInUM", vm);
        }

        

        [HttpPost]
        public async Task<JsonResultViewModel> SaveUpdateUserRole(SampleChetnaManageCURDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                _JsonResultViewModel = await _roles.UpdateSampleChetnaManageRoles(vm);
                return _JsonResultViewModel;
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return _JsonResultViewModel;
                throw;
            }
        }
    }
}