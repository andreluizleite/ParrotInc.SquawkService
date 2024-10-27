namespace ParrotInc.SquawkService.Application.Dtos
{
    public class SquawkDto
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
