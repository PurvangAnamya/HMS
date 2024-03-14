using HMS.Models.MedicinesViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.MedicineHistoryViewModel
{
    public class MedicineHistoryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 MedicineId { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public Int64 ManufactureId { get; set; }
        public string ManufactureName { get; set; }
        public Int64 UnitId { get; set; }
        public string UnitName { get; set; }
        public double? UnitPrice { get; set; }
        public double? SellPrice { get; set; }
        public double? OldUnitPrice { get; set; }
        public double? OldSellPrice { get; set; }
        public double? OldQuantity { get; set; }
        public double? NewQuantity { get; set; }
        public int TranQuantity { get; set; }
        public string UpdateQntType { get; set; }
        public string UpdateQntNote { get; set; }
        public string Note { get; set; }
        public string Action { get; set; }



        public static implicit operator MedicineHistoryCRUDViewModel(MedicineHistory _MedicineHistory)
        {
            return new MedicineHistoryCRUDViewModel
            {
                Id = _MedicineHistory.Id,
                MedicineId = _MedicineHistory.MedicineId,
                Code = _MedicineHistory.Code,
                MedicineName = _MedicineHistory.MedicineName,
                ManufactureId = _MedicineHistory.ManufactureId,
                UnitId = _MedicineHistory.UnitId,
                UnitPrice = _MedicineHistory.UnitPrice,
                SellPrice = _MedicineHistory.SellPrice,
                OldUnitPrice = _MedicineHistory.OldUnitPrice,
                OldSellPrice = _MedicineHistory.OldSellPrice,
                OldQuantity = _MedicineHistory.OldQuantity,
                NewQuantity = _MedicineHistory.NewQuantity,
                TranQuantity = _MedicineHistory.TranQuantity,
                UpdateQntType = _MedicineHistory.UpdateQntType,
                UpdateQntNote = _MedicineHistory.UpdateQntNote,
                Note = _MedicineHistory.Note,
                Action = _MedicineHistory.Action,
                CreatedDate = _MedicineHistory.CreatedDate,
                ModifiedDate = _MedicineHistory.ModifiedDate,
                CreatedBy = _MedicineHistory.CreatedBy,
                ModifiedBy = _MedicineHistory.ModifiedBy,
                Cancelled = _MedicineHistory.Cancelled,
            };
        }

        public static implicit operator MedicineHistory(MedicineHistoryCRUDViewModel vm)
        {
            return new MedicineHistory
            {
                Id = vm.Id,
                MedicineId = vm.MedicineId,
                Code = vm.Code,
                MedicineName = vm.MedicineName,
                ManufactureId = vm.ManufactureId,
                UnitId = vm.UnitId,
                UnitPrice = vm.UnitPrice,
                SellPrice = vm.SellPrice,
                OldUnitPrice = vm.OldUnitPrice,
                OldSellPrice = vm.OldSellPrice,
                OldQuantity = vm.OldQuantity,
                NewQuantity = vm.NewQuantity,
                TranQuantity = vm.TranQuantity,
                UpdateQntType = vm.UpdateQntType,
                UpdateQntNote = vm.UpdateQntNote,
                Note = vm.Note,
                Action = vm.Action,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }




        public static implicit operator MedicineHistoryCRUDViewModel(MedicinesCRUDViewModel _MedicineHistory)
        {
            return new MedicineHistoryCRUDViewModel
            {
                MedicineId = _MedicineHistory.Id,
                Code = _MedicineHistory.Code,
                MedicineName = _MedicineHistory.MedicineName,
                ManufactureId = _MedicineHistory.ManufactureId,
                UnitId = _MedicineHistory.UnitId,
                UnitPrice = _MedicineHistory.UnitPrice,
                SellPrice = _MedicineHistory.SellPrice,
                OldUnitPrice = _MedicineHistory.OldUnitPrice,
                OldSellPrice = _MedicineHistory.OldSellPrice,
                UpdateQntType = _MedicineHistory.UpdateQntType,
                UpdateQntNote = _MedicineHistory.UpdateQntNote,
                Note = _MedicineHistory.Note,
                CreatedDate = _MedicineHistory.CreatedDate,
                ModifiedDate = _MedicineHistory.ModifiedDate,
                CreatedBy = _MedicineHistory.CreatedBy,
                ModifiedBy = _MedicineHistory.ModifiedBy,
                Cancelled = _MedicineHistory.Cancelled,
            };
        }

        public static implicit operator MedicinesCRUDViewModel(MedicineHistoryCRUDViewModel vm)
        {
            return new MedicinesCRUDViewModel
            {
                Id = vm.MedicineId,
                Code = vm.Code,
                MedicineName = vm.MedicineName,
                ManufactureId = vm.ManufactureId,
                UnitId = vm.UnitId,
                UnitPrice = vm.UnitPrice,
                SellPrice = vm.SellPrice,
                OldUnitPrice = vm.OldUnitPrice,
                OldSellPrice = vm.OldSellPrice,
                UpdateQntType = vm.UpdateQntType,
                UpdateQntNote = vm.UpdateQntNote,
                Note = vm.Note,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}

