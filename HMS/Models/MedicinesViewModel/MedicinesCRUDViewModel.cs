using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.MedicinesViewModel
{
    public class MedicinesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Medicine Category")]
        public int MedicineCategoryId { get; set; }
        public string MedicineCategoryName { get; set; }
        public string PaymentItemCode { get; set; }
        [Display(Name = "Medicine Name")]
        [Required]
        public string MedicineName { get; set; }
        [Display(Name = "Manufacture")]
        public Int64 ManufactureId { get; set; }
        public string ManufactureName { get; set; }
        [Display(Name = "Unit")]
        [Required]
        public Int64 UnitId { get; set; }
        public string UnitName { get; set; }
        [Display(Name = "Unit Price")]
        [Required]
        public double? UnitPrice { get; set; }
        [Display(Name = "Sell Price")]
        [Required]
        public double? SellPrice { get; set; }
        public double? OldUnitPrice { get; set; }
        public double? OldSellPrice { get; set; }
        [Required]
        public double? Quantity { get; set; }
        [Display(Name = "Add New Quantity")]
        [Required]
        public int AddNewQuantity { get; set; }
        public string Description { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; } = DateTime.Today.AddYears(1);
        [Display(Name = "Update Quantity Type")]
        public string UpdateQntType { get; set; }
        [Display(Name = "Update Quantity Note")]
        public string UpdateQntNote { get; set; }
        [Display(Name = "SKU")]
        public string StockKeepingUnit { get; set; }
        public string Note { get; set; }



        public static implicit operator MedicinesCRUDViewModel(Medicines _Medicines)
        {
            return new MedicinesCRUDViewModel
            {
                Id = _Medicines.Id,
                Code = _Medicines.Code,
                MedicineCategoryId = _Medicines.MedicineCategoryId,
                PaymentItemCode = _Medicines.PaymentItemCode,
                MedicineName = _Medicines.MedicineName,
                ManufactureId = _Medicines.ManufactureId,
                UnitId = _Medicines.UnitId,
                UnitPrice = _Medicines.UnitPrice,
                SellPrice = _Medicines.SellPrice,
                OldUnitPrice = _Medicines.OldUnitPrice,
                OldSellPrice = _Medicines.OldSellPrice,
                Quantity = _Medicines.Quantity,
                Description = _Medicines.Description,
                ExpiryDate = _Medicines.ExpiryDate,
                UpdateQntType = _Medicines.UpdateQntType,
                UpdateQntNote = _Medicines.UpdateQntNote,
                StockKeepingUnit = _Medicines.StockKeepingUnit,

                CreatedDate = _Medicines.CreatedDate,
                ModifiedDate = _Medicines.ModifiedDate,
                CreatedBy = _Medicines.CreatedBy,
                ModifiedBy = _Medicines.ModifiedBy,
                Cancelled = _Medicines.Cancelled
            };
        }

        public static implicit operator Medicines(MedicinesCRUDViewModel vm)
        {
            return new Medicines
            {
                Id = vm.Id,
                Code = vm.Code,
                MedicineCategoryId = vm.MedicineCategoryId,
                PaymentItemCode = vm.PaymentItemCode,
                MedicineName = vm.MedicineName,
                ManufactureId = vm.ManufactureId,
                UnitId = vm.UnitId,
                UnitPrice = vm.UnitPrice,
                SellPrice = vm.SellPrice,
                OldUnitPrice = vm.OldUnitPrice,
                OldSellPrice = vm.OldSellPrice,
                Quantity = vm.Quantity,
                Description = vm.Description,
                ExpiryDate = vm.ExpiryDate,
                UpdateQntType = vm.UpdateQntType,
                UpdateQntNote = vm.UpdateQntNote,
                StockKeepingUnit = vm.StockKeepingUnit,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled
            };
        }
    }
}
