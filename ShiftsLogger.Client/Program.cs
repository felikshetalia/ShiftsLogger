using Microsoft.Extensions.Configuration;
namespace ShiftsLogger.Client;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(AppContext.BaseDirectory);
        var config = Setup();

        string baseUrl = config["ShiftsDB:BaseUrl"]
            ?? throw new InvalidOperationException("ShiftsDB:BaseUrl is missing.");

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/"),
            Timeout = TimeSpan.FromSeconds(15)
        };

        IAppView appview = new AppView();
        AppController mainApp = new(appview);

        await mainApp.Run();
    }

    private static IConfigurationRoot Setup()
    {
        IConfigurationRoot config =
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    "Configuration/appsettings.json",
                    optional: false)
                .Build();

        return config;
    }
}