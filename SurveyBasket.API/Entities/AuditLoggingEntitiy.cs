namespace SurveyBasket.API.Entities
{
    public class AuditLoggingEntitiy
    {
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreateOn { get; set; } = DateTime.UtcNow;

        public string? UpdatedById { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public ApplicationUser? UpdatedBy { get; set; }  //Navigation Property         UpdatedById <= created Fk

        public ApplicationUser CreatedBy { get; set; } = default!; //Navigation Property  

    }

}
