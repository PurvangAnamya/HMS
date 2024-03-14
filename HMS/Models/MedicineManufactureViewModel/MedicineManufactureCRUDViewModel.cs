using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.MedicineManufactureViewModel
{
    public class MedicineManufactureCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string Description { get; set; }



        public static implicit operator MedicineManufactureCRUDViewModel(MedicineManufacture _MedicineManufacture)
        {
            return new MedicineManufactureCRUDViewModel
            {
                Id = _MedicineManufacture.Id,
                Name = _MedicineManufacture.Name,
                Address = _MedicineManufacture.Address,
                Description = _MedicineManufacture.Description,
                CreatedDate = _MedicineManufacture.CreatedDate,
                ModifiedDate = _MedicineManufacture.ModifiedDate,
                CreatedBy = _MedicineManufacture.CreatedBy,
                ModifiedBy = _MedicineManufacture.ModifiedBy,
                Cancelled = _MedicineManufacture.Cancelled,
            };
        }

        public static implicit operator MedicineManufacture(MedicineManufactureCRUDViewModel vm)
        {
            return new MedicineManufacture
            {
                Id = vm.Id,
                Name = vm.Name,
                Address = vm.Address,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
