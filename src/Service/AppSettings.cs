#nullable disable warnings

namespace Service;

public class AppSettings
{
    public const string TestEnvironment = "test";
    public string DatabaseConnection { get; set; }
    public string ReverseGeocodingBaseUrl { get; set; }
}