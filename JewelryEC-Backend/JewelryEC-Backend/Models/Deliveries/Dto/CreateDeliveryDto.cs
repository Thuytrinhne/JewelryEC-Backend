using JewelryEC_Backend.Models.Addresses;

using System;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Deliveries.Dto
{
    public class CreateDeliveryDto
    {
        [Required(ErrorMessage = "Department flag is required.")]
        public bool IsDepartment { get; set; }

        [Required(ErrorMessage = "ReceiverIsMe flag is required.")]
        public bool ReceiverIsMe { get; set; }

        [Required(ErrorMessage = "Information is required.")]
        [StringLength(255, ErrorMessage = "Information must not exceed 255 characters.")]
        public string Information { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(100, ErrorMessage = "Province must not exceed 100 characters.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "District is required.")]
        [StringLength(100, ErrorMessage = "District must not exceed 100 characters.")]
        public string District { get; set; }

        [Required(ErrorMessage = "Ward is required.")]
        [StringLength(100, ErrorMessage = "Ward must not exceed 100 characters.")]
        public string Ward { get; set; }
    }
}
