using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prog7311PoeTwo.Models
{
    public class Contracts
    {
         [Key]
        public int ContractID { get; set; }
        
        [DisplayName("Contract Name")]
        public string? ContractName { get; set; }

        public int ClientID { get; set; }

        public ClientDetails? ClientDetails { get; set; }

        public ContractStatus Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public decimal AmountInZAR { get; set; }
        [DisplayName("Start")]
        public DateOnly StartDate { get; set; }
        
        [DisplayName("End")]
        public DateOnly EndDate { get; set; }
    }

    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        Hold
    }
}