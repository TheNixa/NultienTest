using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using NultienTest.Util;

namespace PlaywrightTests;

[TestFixture]
public class WeatherAPITests : PlaywrightTest
{
    private IAPIRequestContext Request = null;

    private string APP_ID;

    [SetUp]
    public async Task SetUp()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var BASE_URL = configuration["WeatherAPI:BASE_URL"];
        APP_ID = configuration["WeatherAPI:APP_ID"];

        Request = await this.Playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = BASE_URL,
        });


    }

    [Test]
    public async Task SuccessfullWeatherAPIRequest()
    {

        var queryParams = Util.ConstructQueryParams(new Dictionary<string, string>{
            { "lat", "44.34" },
            { "lon", "10.99" },
            { "appid", APP_ID },
        });

        var response = await Request.GetAsync("weather?" + queryParams);

        Assert.True(response.Ok);
    }


    [Test]
    public async Task SucesfullWeatherAPICallWithSpecificLocation()
    {
        var queryParams = Util.ConstructQueryParams(new Dictionary<string, string>{
            { "lat", "44.34" },
            { "lon", "10.99" },
            { "appid", APP_ID },
        });

        var response = await Request.GetAsync("weather?" + queryParams);

        Assert.True(response.Ok);

        var jsonResponse = await response.JsonAsync();
        var responseName = jsonResponse?.GetProperty("name").GetString();

        Assert.That(responseName, Is.EqualTo("Zocca"));
    }

    [Test]
    public async Task SuccessfullWeatherAPICallWithZipCode()
    {
        var queryParams = Util.ConstructQueryParams(new Dictionary<string, string>{
            { "zip", "94040" },
            { "appid", APP_ID },
        });

        var response = await Request.GetAsync("weather?" + queryParams);

        Assert.True(response.Ok);

        var jsonResponse = await response.JsonAsync();
        var responseName = jsonResponse?.GetProperty("name").GetString();

        Assert.That(responseName, Is.EqualTo("Mountain View"));
    }

    [Test]
    public async Task UnsuccessfullWeatherAPICallWithWrongLocation()
    {
        var queryParams = Util.ConstructQueryParams(new Dictionary<string, string>{
            { "lat", "999" },
            { "lon", "10" },
            { "appid", APP_ID },
        });

        var response = await Request.GetAsync("weather?" + queryParams);
        var jsonResponse = await response.JsonAsync();

        Assert.That(response.Status, Is.EqualTo(400));
        Assert.That(jsonResponse?.GetProperty("message").GetString(), Is.EqualTo("wrong latitude"));
        Assert.False(response.Ok);
    }

    [Test]
    public async Task UnsuccessfullWeatherAPICallWithWrongAppKey()
    {
        var queryParams = Util.ConstructQueryParams(new Dictionary<string, string>{
            { "lat", "44.34" },
            { "lon", "10.99" },
            { "appid", "invalid" },
        });

        var response = await Request.GetAsync("weather?" + queryParams);

        Assert.False(response.Ok);
    }

    [Test]
    public async Task MissingRequiredParameters()
    {
        var queryParams = Util.ConstructQueryParams(new Dictionary<string, string>{
        { "appid", "invalid" },
        });

        var response = await Request.GetAsync("weather?" + queryParams);

        Assert.False(response.Ok);
    }

    [TearDown]
    public async Task TearDownAPITesting()
    {
        await Request.DisposeAsync();
    }
}