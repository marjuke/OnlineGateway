namespace Domain.ResponseC
{
    public class PaymentConfirmRes
    {
        public string id { get; set; }
        public DateTime trDate { get; set; }
        public string referenceNo { get; set; }
        public string trIDNo { get; set; }
        public string status { get; set; }
        public string BranchName { get; set; }
        public string WalletNumber { get; set; }
    }
}
