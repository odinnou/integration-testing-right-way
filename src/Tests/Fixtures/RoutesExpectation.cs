using MockServerClientNet;
using MockServerClientNet.Model;
using static MockServerClientNet.Model.HttpRequest;
using static MockServerClientNet.Model.HttpResponse;

namespace Tests.Fixtures;

public static class RoutesExpectation
{
    public static async Task SetExpectationsForEnrichData(MockServerClient mockServerClient)
    {
        await mockServerClient.When(Request()
                .WithMethod(HttpMethod.Get)
                .WithPath($"/reverse-geocoding")
                .WithQueryStringParameter("latitude", PandaData.Constants.Latitude)
                .WithQueryStringParameter("longitude", PandaData.Constants.Longitude),
            Times.Unlimited()
            )
        .RespondAsync(Response()
            .WithStatusCode(System.Net.HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithBody(@$"{{""address"":""{PandaData.Constants.Address}""}}")
        );
    }
}