namespace Entities.Concrete
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Answer>? Answers { get; set; }
        public string? Status { get; set; }


    }
}
