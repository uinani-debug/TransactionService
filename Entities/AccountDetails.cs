using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.API.Entities
{
    public class AccountDetails
    {
        public string account_identifier { get; set; }

        public double amount { get; set; }

    }
}
