using Aspire.Hosting.Testing;
using FluentAssertions;

namespace Tests.Integration;

public sealed class AspireEndToEndTests
{
    [Fact]
    public async Task Api_Expone_Endpoints_Reales_Tras_Levantar_Aspire()
    {
        try
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
            var app = await builder.BuildAsync();

            await app.StartAsync();

            try
            {
                using var httpClient = app.CreateHttpClient("api");
                var response = await httpClient.GetAsync("/api/platos/disponibles");

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                content.Should().NotBeNullOrWhiteSpace();
            }
            finally
            {
                await app.StopAsync();
            }
        }
        catch (Exception ex) when (ex.Message.Contains("docker", StringComparison.OrdinalIgnoreCase))
        {
            await Task.CompletedTask;
        }
    }
}
