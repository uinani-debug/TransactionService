using TransactionService.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TransactionLibrary.API.Entities
{
    public class Transaction
    {
        public string AccountIdentifier { get; set; }
        public string TransactionTowards { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public double AvailableBalance { get; set; }

    }
}
