using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.BedViewModel
{
    public class BedCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Bed Category")]
        [Required]
        public Int64 BedCategoryId { get; set; }
        [Required]
        public string No { get; set; }
        public string Description { get; set; }



        public static implicit operator BedCRUDViewModel(Bed _Bed)
        {
            return new BedCRUDViewModel
            {
                Id = _Bed.Id,
                BedCategoryId = _Bed.BedCategoryId,
                No = _Bed.No,
                Description = _Bed.Description,
                CreatedDate = _Bed.CreatedDate,
                ModifiedDate = _Bed.ModifiedDate,
                CreatedBy = _Bed.CreatedBy,
                ModifiedBy = _Bed.ModifiedBy,
                Cancelled = _Bed.Cancelled,

            };
        }

        public static implicit operator Bed(BedCRUDViewModel vm)
        {
            return new Bed
            {
                Id = vm.Id,
                BedCategoryId = vm.BedCategoryId,
                No = vm.No,
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

