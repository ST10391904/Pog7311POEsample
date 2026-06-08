using System;

namespace Shared.Models;

    public class ContractDTO
    {
        public int ClientID { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
