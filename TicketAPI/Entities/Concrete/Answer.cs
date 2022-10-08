namespace Entities.Concrete
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
