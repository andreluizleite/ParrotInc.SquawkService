using Polly;
using Polly.Retry;
using System;
using System.Net.Http;

public class PolicyProvider
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode) 
            .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(20),
                (result, timeSpan) => {
                    Console.WriteLine($"Request failed with status {result.Result.StatusCode}. Retrying in {timeSpan.TotalSeconds} seconds.");
                });
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
            .CircuitBreakerAsync(1, TimeSpan.FromSeconds(20),
                (result, duration) => {
                    Console.WriteLine($"Circuit broken due to: {result.Result.StatusCode}");
                },
                () => {
                    Console.WriteLine("Circuit reset.");
                });
    }
}
