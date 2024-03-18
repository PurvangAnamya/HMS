using HMS.Data;
using HMS.Models;
using HMS.Models.CommonViewModel;
using HMS.Models.ManageUserRolesVM;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageUserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoles _roles;

        public ManageUserRolesController(ApplicationDbContext context, ICommon iCommon, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IRoles roles)
        {
            _context = context;
            _iCommon = iCommon;
            _userManager = userManager;
            _roleManager = roleManager;
            _roles = roles;
        }

        [Authorize(Roles = Pages.MainMenu.ManageUserRoles.RoleName)]
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
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
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
   
        private IQueryable<ManageUserRolesCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _ManageRole in _context.ManageUserRoles join _UserImages in _context.UserImages
                        on _ManageRole.ImageId equals _UserImages.Id  into userImageGroup 
                        from userImage in userImageGroup.DefaultIfEmpty() // leftjoin
                        where _ManageRole.Cancelled == false
                        select new ManageUserRolesCRUDViewModel
                        {
                            Id = _ManageRole.Id,
                            Name = _ManageRole.Name,
                            Description = _ManageRole.Description,
                            ImageId = _ManageRole.ImageId,
                            DashboardImageId = _ManageRole.DashboardImageId,
                            ProfilePicture = _context.UserImages.FirstOrDefault(x => x.Id == _ManageRole.ImageId).ImagePath ?? "/upload/blank-person.png",
                            DashboardPicture = _context.UserImages.FirstOrDefault(x => x.Id == _ManageRole.DashboardImageId).ImagePath ?? "/upload/blank-person.png",
                            CreatedDate = _ManageRole.CreatedDate,
                            ModifiedDate = _ManageRole.ModifiedDate,
                            CreatedBy = _ManageRole.CreatedBy,
                            ModifiedBy = _ManageRole.ModifiedBy,
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
            ManageUserRolesCRUDViewModel vm = await _context.ManageUserRoles.FirstOrDefaultAsync(m => m.Id == id);
            vm.listManageUserRolesDetails = await _iCommon.GetManageRoleDetailsList(id);
            var userImage = await _context.UserImages.Where(x => x.Id == vm.ImageId).SingleOrDefaultAsync();
            if (userImage != null)
            {
                vm.ProfilePicture = userImage.ImagePath;
            }
            var DashboarduserImage = await _context.UserImages.Where(x => x.Id == vm.DashboardImageId).SingleOrDefaultAsync();
            if (DashboarduserImage != null)
            {
                vm.DashboardPicture = DashboarduserImage.ImagePath;
            }
            return PartialView("_Info", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(Int64 id)
        {
            ManageUserRolesCRUDViewModel vm = new();
            if (id > 0)
            {
                vm = await _context.ManageUserRoles.Where(x => x.Id == id).SingleOrDefaultAsync();
                vm.listManageUserRolesDetails = await _iCommon.GetManageRoleDetailsList(id);
                var userImage = await _context.UserImages.Where(x => x.Id == vm.ImageId).SingleOrDefaultAsync();
                if (userImage != null)
                {
                    vm.ProfilePicture = userImage.ImagePath;
                }

                var DashboarduserImage = await _context.UserImages.Where(x => x.Id == vm.DashboardImageId).SingleOrDefaultAsync();
                if (DashboarduserImage != null)
                {
                    vm.DashboardPicture = DashboarduserImage.ImagePath;
                }
            }
            else
            {
                vm.listManageUserRolesDetails = await _roles.GetRoleList();
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ManageUserRolesCRUDViewModel vm)
        {
            try
            {
                ManageUserRoles _ManageUserRoles = new();
                var _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _ManageUserRoles = await _context.ManageUserRoles.FindAsync(vm.Id);
                    
                    vm.ImageId = _iCommon.GetImageFileDetails(_UserName, vm.ProfilePictureDetails, "LeftMenuImages", vm.ImageId);
                    vm.DashboardImageId = _iCommon.GetImageFileDetails(_UserName, vm.DashboardPictureDetails, "DashboardImages", vm.DashboardImageId);
                    vm.CreatedDate = _ManageUserRoles.CreatedDate;
                    vm.CreatedBy = _ManageUserRoles.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_ManageUserRoles).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    foreach (var item in vm.listManageUserRolesDetails)
                    {
                        var _ManageUserRolesDetails = await _context.ManageUserRolesDetails.FindAsync(item.Id);
                        _ManageUserRolesDetails.IsAllowed = item.IsAllowed;
                        _context.ManageUserRolesDetails.Update(_ManageUserRolesDetails);
                        await _context.SaveChangesAsync();
                    }
                    var _AlertMessage = "User Role Updated Successfully. ID: " + _ManageUserRoles.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    vm.ImageId = _iCommon.GetImageFileDetails(_UserName, vm.ProfilePictureDetails, "LeftMenuImages", vm.ImageId);
                    vm.DashboardImageId = _iCommon.GetImageFileDetails(_UserName, vm.DashboardPictureDetails, "DashboardImages", vm.DashboardImageId);
                    _ManageUserRoles = vm;
                    _ManageUserRoles.CreatedDate = DateTime.Now;
                    _ManageUserRoles.ModifiedDate = DateTime.Now;
                    _ManageUserRoles.CreatedBy = _UserName;
                    _ManageUserRoles.ModifiedBy = _UserName;
                    _context.Add(_ManageUserRoles);
                    await _context.SaveChangesAsync();

                    foreach (var item in vm.listManageUserRolesDetails)
                    {
                        ManageUserRolesDetails _ManageRoleDetails = new();

                        _ManageRoleDetails.ManageRoleId = _ManageUserRoles.Id;
                        _ManageRoleDetails.RoleId = item.RoleId;
                        _ManageRoleDetails.RoleName = item.RoleName;
                        _ManageRoleDetails.IsAllowed = item.IsAllowed;

                        _ManageRoleDetails.CreatedDate = DateTime.Now;
                        _ManageRoleDetails.ModifiedDate = DateTime.Now;
                        _ManageRoleDetails.CreatedBy = _UserName;
                        _ManageRoleDetails.ModifiedBy = _UserName;
                        _context.Add(_ManageRoleDetails);
                        await _context.SaveChangesAsync();
                    }

                    var _AlertMessage = "User Role Created Successfully. ID: " + _ManageUserRoles.Id;
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
                var _ManageUserRoles = await _context.ManageUserRoles.FindAsync(id);
                _ManageUserRoles.ModifiedDate = DateTime.Now;
                _ManageUserRoles.ModifiedBy = HttpContext.User.Identity.Name;
                _ManageUserRoles.Cancelled = true;

                _context.Update(_ManageUserRoles);
                await _context.SaveChangesAsync();
                return new JsonResult(_ManageUserRoles);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUserRole(Int64 id)
        {
            ManageUserRolesCRUDViewModel vm = new();
            UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            var _listIdentityRole = _roleManager.Roles.ToList();

            GetRolesByUserViewModel _GetRolesByUserViewModel = new()
            {
                ApplicationUserId = _UserProfile.ApplicationUserId,
                UserManager = _userManager,
                listIdentityRole = _listIdentityRole
            };
            vm.listManageUserRolesDetails = await _roles.GetRolesByUser(_GetRolesByUserViewModel);
            vm.ApplicationUserId = _UserProfile.ApplicationUserId;
            return PartialView("_UpdateRoleInUM", vm);
        }

        [HttpPost]
        public async Task<JsonResultViewModel> SaveUpdateUserRole(ManageUserRolesCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                _JsonResultViewModel = await _roles.UpdateUserRoles(vm);
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
