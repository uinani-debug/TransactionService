using TransactionLibrary.API.Entities;
using TransactionService.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace TransactionLibrary.API.Services
{
    public interface ITransactionLibraryRepository
    {       
       bool TransactionDetails(Transaction accountNumber);
    

    }
}
