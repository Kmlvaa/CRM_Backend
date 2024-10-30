using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs.Customer
{
    public class AddCustomerDTO
    {
        [Required(ErrorMessage = "Name is required!")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Phone number is required!")]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Company name is required!")]
        public required string CompanyName { get; set; }
    } 
}
