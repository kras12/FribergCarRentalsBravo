using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Entities
{
    public class AdminUser
    {
        #region Properties

        [Key]
        public int AdminId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        #endregion
    }
}
