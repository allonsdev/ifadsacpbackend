namespace sacpapi.Models
{
    public class CreateGeneralActivityModel
    {
        public required GeneralActivity GeneralActivity { get; set; }
        public int[]? FacilitatorIds { get; set; } = [];
    }
}
