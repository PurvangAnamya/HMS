using HMS.Data;
using HMS.Pages;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AuditLogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ILogger<AuditLogsController> _logger;


        public AuditLogsController(ApplicationDbContext context, ICommon iCommon, ILogger<AuditLogsController> logger)
        {
            _context = context;
            _iCommon = iCommon;
            _logger = logger;

        }
        
        [Authorize(Roles = MainMenu.AuditLogs.RoleName)]
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

                var _GetGridItem = _context.AuditLogs.AsQueryable();
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
                    || obj.UserId.ToLower().Contains(searchValue)
                    || obj.Type.ToLower().Contains(searchValue)
                    || obj.TableName.ToLower().Contains(searchValue)
                    || obj.DateTime.ToString().Contains(searchValue)
                    || obj.OldValues.ToLower().Contains(searchValue)
                    || obj.NewValues.ToLower().Contains(searchValue)
                    || obj.AffectedColumns.ToLower().Contains(searchValue)

                    || obj.PrimaryKey.ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                _logger.LogInformation("Error in getting Successfully.");
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting Audit Logs.");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            var result = await _context.AuditLogs.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null) return NotFound();
            return PartialView("_Details", result);
        }
    }
}
