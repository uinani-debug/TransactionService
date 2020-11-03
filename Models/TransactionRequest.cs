using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.API.Models
{
    public class TransactionRequest
    {
        

        public string AccountIdentifier { get; set; }
        public string TransactionTowards { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public double AvailableBalance { get; set; }
        
    }
}
