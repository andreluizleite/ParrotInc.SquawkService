using ParrotInc.SquawkService.Application.Dtos;

namespace ParrotInc.SquawkService.Application.Responses
{
    public class CreateSquawkResponse
    {
        public SquawkDto Squawk { get; }

        public CreateSquawkResponse(SquawkDto squawk)
        {
            Squawk = squawk;
        }
    }
}
