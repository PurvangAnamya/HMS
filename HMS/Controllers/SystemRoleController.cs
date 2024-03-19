using HMS.Data;
using HMS.Models;
using HMS.Models.CommonViewModel;
using HMS.Models.ManageUserRolesVM;
using HMS.Pages;
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
    public class SystemRoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoles _roles;
        private readonly ILogger<SystemRoleController> _logger;


        public SystemRoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IRoles roles, ILogger<SystemRoleController> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _roles = roles;
            _logger = logger;
        }

        [Authorize(Roles = Pages.MainMenu.SystemRole.RoleName)]
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
                    _GetGridItem = _GetGridItem.Where(obj => obj.RoleId_SL.ToString().Contains(searchValue)
                    || obj.RoleName.ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting System Role.");
                throw;
            }
        }
        private IQueryable<ManageUserRolesViewModel> GetGridItem()
        {
            List<ManageUserRolesViewModel> list = new List<ManageUserRolesViewModel>();
            try
            {
                var result = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                int Count = 1;
                foreach (var role in result)
                {
                    var userRolesViewModel = new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleId_SL = Count,
                        RoleName = role.Name
                    };
                    list.Add(userRolesViewModel);
                    Count++;
                }
                return list.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add System Role.");
                throw;
            }
        }

        [HttpGet]
        public IActionResult AddNewRole()
        {
            return PartialView("_AddNewRole");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddNewRole(AddNewRoleViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _CreateSingleRole = await _roles.CreateSingleRole(vm.RoleName);
                _JsonResultViewModel.AlertMessage = _CreateSingleRole;

                if (_CreateSingleRole.Contains("Created"))
                {
                    _JsonResultViewModel.IsSuccess = true;
                    var _IdentityRole = await _roleManager.FindByNameAsync(vm.RoleName);
                    var _UserName = HttpContext.User.Identity.Name;
                    await AddManageUserRolesDetails(_IdentityRole, _UserName);
                }
                else
                {
                    _JsonResultViewModel.IsSuccess = false;
                }


                return new JsonResult(_JsonResultViewModel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error in Save AddNew Role.");
                return new JsonResult(ex.Message);
                throw;
            }
        }
        [HttpDelete]
        public async Task<JsonResult> DeleteRole(string RoleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(RoleName);
                var result = await _roleManager.DeleteAsync(role);

                var _UserRoles = await _context.UserRoles.Where(x => x.RoleId == role.Id).ToListAsync();
                _context.RemoveRange(_UserRoles);
                await _context.SaveChangesAsync();

                var _ManageUserRoles = await _context.ManageUserRolesDetails.Where(x => x.RoleId == role.Id).ToListAsync();
                _context.RemoveRange(_ManageUserRoles);
                await _context.SaveChangesAsync();

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete Role.");
                throw;
            }
        }
        private async Task AddManageUserRolesDetails(IdentityRole _Roles, string _UserName)
        {
            try
            {
                var _ManageUserRoles = await _context.ManageUserRoles.ToListAsync();
                foreach (var item in _ManageUserRoles)
                {
                    ManageUserRolesDetails _ManageRoleDetails = new()
                    {
                        ManageRoleId = item.Id,
                        RoleId = _Roles.Id,
                        RoleName = _Roles.Name,
                        IsAllowed = false,

                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserName,
                        ModifiedBy = _UserName
                    };
                    _context.Add(_ManageRoleDetails);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add ManageUserRolesDetails.");
                throw;
            }
        }
    }
}