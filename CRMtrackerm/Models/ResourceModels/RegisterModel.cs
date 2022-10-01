using System.ComponentModel.DataAnnotations;
namespace CrmTracker.Models.ResourceModels
{
    public class RegisterModels
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required(ErrorMessage = "pls provide the email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        
    }






}

