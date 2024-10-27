namespace ParrotInc.SquawkService.Infrastructure.Entity
{
    public class SquawkEventEntity
    {
        public int Id { get; set; }
        public string SquawkId { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime OccurredOn { get; set; }
    }

}
