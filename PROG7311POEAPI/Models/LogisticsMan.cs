using System.ComponentModel.DataAnnotations;

namespace PROG7311POEAPI.Models{

    public class LogisticsManager
    {
        [Key]
        public int LogManID {get;set;}
        public int ContractId { get; set; }

        public string? ContractName { get; set; }

        public string? ClientName { get; set; }

        public ContractStatus Status { get; set; }

        public decimal Amount { get; set; }

        public string? Currency { get; set; }

        public decimal AmountInZAR { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool CanRequestSLA =>
            Status == ContractStatus.Active ||
            Status == ContractStatus.Draft;
    }
}