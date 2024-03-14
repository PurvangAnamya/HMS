using HMS.Data;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DownloadReportController : Controller
    {
        private readonly string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"10\" --footer-spacing 6 --footer-font-name \"calibri light\"";
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DownloadReportController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.Admin.RoleName)]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DownloadPaymentInvoiceReportPDF(Int64 _PaymentId)
        {
            var _PrintPaymentInvoice = await _iCommon.PrintPaymentInvoice(_PaymentId);
            var rpt = new ViewAsPdf();
            rpt.PageOrientation = Orientation.Portrait;
            rpt.CustomSwitches = footer;

            rpt.FileName = "Payment_Invoice_Report_" + _PaymentId + ".pdf";
            rpt.ViewName = "DownloadPaymentInvoiceReportPDF";
            rpt.Model = _PrintPaymentInvoice;
            return rpt;
        }

        public async Task<IActionResult> DownloadCheckupReportPDF(Int64 id)
        {
            var _GetByCheckupDetails = await _iCommon.GetByCheckupDetails(id);
            var rpt = new ViewAsPdf();
            rpt.PageOrientation = Orientation.Portrait;
            rpt.CustomSwitches = footer;

            rpt.FileName = "Checkup_Report_" + id + ".pdf";
            rpt.ViewName = "DownloadCheckupReportPDF";
            rpt.Model = _GetByCheckupDetails;
            return rpt;
        }

    }
}
