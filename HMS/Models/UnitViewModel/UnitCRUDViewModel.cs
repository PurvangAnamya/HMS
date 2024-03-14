using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.UnitViewModel
{
    public class UnitCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public static implicit operator UnitCRUDViewModel(Unit _Unit)
        {
            return new UnitCRUDViewModel
            {
                Id = _Unit.Id,
                Name = _Unit.Name,
                Description = _Unit.Description,
                CreatedDate = _Unit.CreatedDate,
                ModifiedDate = _Unit.ModifiedDate,
                CreatedBy = _Unit.CreatedBy,
                ModifiedBy = _Unit.ModifiedBy,
                Cancelled = _Unit.Cancelled,
            };
        }

        public static implicit operator Unit(UnitCRUDViewModel vm)
        {
            return new Unit
            {
                Id = vm.Id,
                Name = vm.Name,
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
