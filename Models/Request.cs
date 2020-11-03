namespace TransactionService.API.Models
{
    public class Request
    {
        public string AccountIdentifier { get; set; }
        public string SortCode { get; set; }
        public string PaymentReference { get; set; }

        public double TransferAmount { get; set; }


    }
}
