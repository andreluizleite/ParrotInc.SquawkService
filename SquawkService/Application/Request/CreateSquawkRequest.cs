namespace ParrotInc.SquawkService.Application.Requests
{
    public class CreateSquawkRequest
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}
