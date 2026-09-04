using System.Net;
using System.Net.Http.Json;
using ParrotInc.SquawkService.Application.Dtos;

namespace ParrotInc.SquawkService.Tests.Api;

public sealed class SquawkApiTests : IClassFixture<SquawkApiFactory>
{
    private readonly HttpClient _client;

    public SquawkApiTests(SquawkApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAndReadSquawk_ShouldCompleteTheCqrsFlow()
    {
        var request = new
        {
            userId = Guid.NewGuid(),
            content = $"Portfolio flow {Guid.NewGuid():N}"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/squawks", request);
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<SquawkDto>();
        Assert.NotNull(created);

        var getResponse = await _client.GetAsync($"/api/squawks/{created.Id}");
        getResponse.EnsureSuccessStatusCode();

        var loaded = await getResponse.Content.ReadFromJsonAsync<SquawkDto>();
        Assert.Equal(created, loaded);
    }

    [Fact]
    public async Task CreateSquawk_WithRestrictedContent_ShouldReturnProblemDetails()
    {
        var response = await _client.PostAsJsonAsync("/api/squawks", new
        {
            userId = Guid.NewGuid(),
            content = "This contains Twitter."
        });

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("restricted_content", problem?.Code);
    }

    [Fact]
    public async Task CreateDuplicateSquawk_ShouldReturnConflict()
    {
        var request = new
        {
            userId = Guid.NewGuid(),
            content = $"Duplicate sample {Guid.NewGuid():N}"
        };

        var firstResponse = await _client.PostAsJsonAsync("/api/squawks", request);
        var secondResponse = await _client.PostAsJsonAsync("/api/squawks", request);

        firstResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task CreateDifferentSquawkInsideCooldown_ShouldReturnTooManyRequests()
    {
        var userId = Guid.NewGuid();

        var firstResponse = await _client.PostAsJsonAsync("/api/squawks", new
        {
            userId,
            content = $"First cooldown sample {Guid.NewGuid():N}"
        });

        var secondResponse = await _client.PostAsJsonAsync("/api/squawks", new
        {
            userId,
            content = $"Second cooldown sample {Guid.NewGuid():N}"
        });

        firstResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.TooManyRequests, secondResponse.StatusCode);
        Assert.Equal(TimeSpan.FromSeconds(20), secondResponse.Headers.RetryAfter?.Delta);
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnHealthy()
    {
        var response = await _client.GetAsync("/health/live");

        response.EnsureSuccessStatusCode();
    }

    private sealed record ProblemDetailsResponse(string Code);
}
