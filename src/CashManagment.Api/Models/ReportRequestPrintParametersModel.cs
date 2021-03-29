namespace CashManagment.Api.Models
{
    public class ReportRequestPrintParametersModel
    {
        public double BottomField { get; set; }
        public double TopField { get; set; }
        public double LeftField { get; set; }
        public double RightField { get; set; }
        public double ColumnMargin { get; set; }
        public double RowMargin { get; set; }
        public int StartRow { get; set; }
        public int StartCol { get; set; }
        public int[] ContainersId { get; set; }
        public int UserId { get; set; }
        public int CreditOrgId { get; set; }
    }
}
