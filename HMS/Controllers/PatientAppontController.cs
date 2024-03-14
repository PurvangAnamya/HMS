using HMS.ConHelper;
using HMS.Data;
using HMS.Helpers;
using HMS.Models;
using HMS.Models.PatientAppointmentViewModel;
using HMS.Models.CheckupSummaryViewModel;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using HMS.Models.PaymentsViewModel;

namespace HMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PatientAppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _iDBOperation;

        public PatientAppointmentController(ApplicationDbContext context, ICommon iCommon, IDBOperation iDBOperation)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iDBOperation;
        }

        [Authorize(Roles = Pages.MainMenu.PatientAppointment.RoleName)]
        public IActionResult Index()
        {
            var _IsInRole = User.IsInRole("Admin");
            if (_IsInRole)
            {
                return View();
            }
            else
            {
                return View("IndexGeneral");
            }
        }
         public IActionResult IndexGeneral()
        {
            return View("IndexGeneral");
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
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

                IQueryable<PatientAppointmentCRUDViewModel> _GetGridItem;
                var _UserName = HttpContext.User.Identity.Name;
                var _PatientInfo = await _context.PatientInfo.Where(x => x.Email == _UserName).SingleOrDefaultAsync();
                var _IsInRole = User.IsInRole("Admin");
                if (_IsInRole)
                {
                    _GetGridItem = _iCommon.GetPatientAppointmentGridItem();
                }
                else
                {
                    _GetGridItem = _iCommon.GetPatientAppointmentGridItem().Where(x => x.PatientId == _PatientInfo.Id);
                }
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
                    || obj.PatientName.ToLower().Contains(searchValue)
                    || obj.DoctorName.ToLower().Contains(searchValue)
                    || obj.SerialNo.ToString().Contains(searchValue)
                    || obj.Note.ToLower().Contains(searchValue)
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

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            PatientAppointmentCRUDViewModel vm = await _iCommon.GetPatientAppointmentGridItem().Where(x => x.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            LoadDDL();
            PatientAppointmentCRUDViewModel vm = new();
            if (id > 0)
            {
                vm = await _context.PatientAppointment.Where(x => x.Id == id).SingleOrDefaultAsync();
            }
            else
            {
                var _PatientAppointment = _context.PatientAppointment;
                if (_PatientAppointment.Count() > 0)
                {
                    var _SerialNo = _PatientAppointment.Max(x => x.SerialNo);
                    vm.SerialNo = _SerialNo + 1;
                }
                else
                {
                    vm.SerialNo = 1;
                }
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(PatientAppointmentCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LoadDDL();
                    PatientAppointment _PatientAppointment = new();
                    if (vm.Id > 0)
                    {
                        _PatientAppointment = await _context.PatientAppointment.FindAsync(vm.Id);

                        vm.CreatedDate = _PatientAppointment.CreatedDate;
                        vm.CreatedBy = _PatientAppointment.CreatedBy;
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_PatientAppointment).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        TempData["successAlert"] = "Patient Appointment Updated Successfully. ID: " + _PatientAppointment.Id;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string strCurrentUserName = HttpContext.User.Identity.Name;
                        _PatientAppointment = vm;
                        _PatientAppointment.VisitId = StaticData.GetUniqueID("V");
                        _PatientAppointment.CreatedDate = DateTime.Now;
                        _PatientAppointment.ModifiedDate = DateTime.Now;
                        _PatientAppointment.CreatedBy = strCurrentUserName;
                        _PatientAppointment.ModifiedBy = strCurrentUserName;
                        _context.Add(_PatientAppointment);
                        await _context.SaveChangesAsync();

                        CheckupSummary _CheckupSummary = new()
                        {
                            VisitId = _PatientAppointment.VisitId,
                            SerialNo = _PatientAppointment.SerialNo,
                            PatientId = vm.PatientId,
                            PatientType = vm.PatientType == "1" ? PatientType.InPatient : PatientType.InPatient,
                            DoctorId = vm.DoctorId,
                            CheckupDate = DateTime.Now,
                            NextVisitDate = DateTime.Now.AddDays(7),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = strCurrentUserName,
                            ModifiedBy = strCurrentUserName,
                        };

                        _context.Add(_CheckupSummary);
                        await _context.SaveChangesAsync();

                        AddPaymentViewModel _AddPaymentViewModel = new()
                        {
                            PatientId = _CheckupSummary.PatientId,
                            VisitId = _CheckupSummary.VisitId
                        };

                        var _CreatePayments = await _iDBOperation.CreatePayments(_AddPaymentViewModel, strCurrentUserName);
                        //Add Consultation Payment Details
                        _AddPaymentViewModel.PaymentsId = _CreatePayments.Id;
                        _AddPaymentViewModel.PaymentCategoriesId = 3;
                        var _ConsultationPaymentDetails = await _iDBOperation.CreatePaymentsDetails(_AddPaymentViewModel, PaymentItemType.Consultation, strCurrentUserName);

                        //Uddate Payment
                        MedicineSellStateViewModel _MedicineSellStateViewModel = new MedicineSellStateViewModel
                        {
                            Id = _CreatePayments.Id,
                            InvoiceNo = "INV" + _CreatePayments.Id.ToString(),
                            MedicineSellState = MedicineSellState.AddIntoCheckup,
                            ModifiedBy = strCurrentUserName,
                            GrandTotal = _ConsultationPaymentDetails.TotalAmount
                        };
                        await _iDBOperation.UpdateMedicineSellState(_MedicineSellStateViewModel);


                        _CheckupSummary.PaymentId = _CreatePayments.Id;
                        await UpdateCheckupSummary(_CheckupSummary);
                        var _PatientTest = await _iDBOperation.CreatePatientTest(_AddPaymentViewModel, strCurrentUserName);

                        TempData["successAlert"] = "Patient Appointment Created Successfully. ID: " + _PatientAppointment.Id;
                        return RedirectToAction(nameof(Index));
                    }
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

            TempData["errorAlert"] = "Operation failed.";
            return View("Index");
        }

        private async Task<CheckupSummaryCRUDViewModel> UpdateCheckupSummary(CheckupSummaryCRUDViewModel vm)
        {
            var _CheckupSummary = await _context.CheckupSummary.FindAsync(vm.Id);
            vm.CreatedDate = _CheckupSummary.CreatedDate;
            vm.CreatedBy = _CheckupSummary.CreatedBy;
            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Entry(_CheckupSummary).CurrentValues.SetValues(vm);
            await _context.SaveChangesAsync();
            return vm;
        }
        public async Task<Payments> UpdateMedicineSellState(MedicineSellStateViewModel _MedicineSellStateViewModel)
        {
            var _Payments = await _context.Payments.FindAsync(_MedicineSellStateViewModel.Id);
            _MedicineSellStateViewModel.ModifiedDate = DateTime.Now;
            _context.Entry(_Payments).CurrentValues.SetValues(_MedicineSellStateViewModel);
            await _context.SaveChangesAsync();
            return _Payments;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _PatientAppointment = await _context.PatientAppointment.FindAsync(id);
                _PatientAppointment.ModifiedDate = DateTime.Now;
                _PatientAppointment.ModifiedBy = HttpContext.User.Identity.Name;
                _PatientAppointment.Cancelled = true;

                _context.Update(_PatientAppointment);
                await _context.SaveChangesAsync();
                return new JsonResult(_PatientAppointment);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void LoadDDL()
        {
            ViewBag._LoadddlPatientName = new SelectList(_iCommon.LoadddlPatientName(), "Id", "Name");
            ViewBag._LoadddlDoctorName = new SelectList(_iCommon.LoadddlDoctorName(), "Id", "Name");
        }
        private bool IsExists(long id)
        {
            return _context.PatientAppointment.Any(e => e.Id == id);
        }
    }
}
