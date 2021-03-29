namespace CashManagment.Api.Models
{
    public class RealContainerPropertiesRequest
    {
        public int[] ContainersId { get; set; }
        public bool? WroteOff { get; set; }
        public bool? NeedCheck { get; set; }
    }
}
