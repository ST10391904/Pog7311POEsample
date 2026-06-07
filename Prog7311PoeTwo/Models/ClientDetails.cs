using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prog7311PoeTwo.Models
{
    public class ClientDetails
    {
        [Key]
        public int ClientID { get; set; }

        [DisplayName("Client Name")]
        [Required]
        public string ClientName { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        public string ClientEmail { get; set; }
        [DisplayName("Phone Number")]
        public string ClientPhoneNumber { get; set; }

        public List<Contracts> Contracts { get; set; } = new();
    }
}