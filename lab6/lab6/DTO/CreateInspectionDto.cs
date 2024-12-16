namespace lab6.DTO
{
    public class CreateInspectionDto
    {
        public int InspectorId { get; set; }
        public int EnterpriseId { get; set; }
        public int ViolationTypeId { get; set; }
        public DateOnly InspectionDate { get; set; }
        public string ProtocolNumber { get; set; }
        public string ResponsiblePerson { get; set; }
        public decimal PenaltyAmount { get; set; }
        public DateOnly PaymentDeadline { get; set; }
        public DateOnly CorrectionDeadline { get; set; }
        public string PaymentStatus { get; set; }
        public string CorrectionStatus { get; set; }
    }
}
