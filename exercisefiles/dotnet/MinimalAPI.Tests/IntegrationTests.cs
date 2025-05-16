namespace IntegrationTests;

public class IntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHelloWorld()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World!", content);
    }

    [Theory]
    [InlineData("2024-01-01", "2024-01-10", 9)]
    [InlineData("2024-01-10", "2024-01-01", 9)]
    public async Task DaysBetweenDates_ReturnsCorrectDays(string date1, string date2, double expected)
    {
        var response = await _client.GetAsync($"/DaysBetweenDates?date1={date1}&date2={date2}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(expected.ToString(), content);
    }

    [Theory]
    [InlineData("+34666777888", false)]
    [InlineData("666777888", false)]
    [InlineData("+35123456789", false)]
    public async Task ValidatePhoneNumber_Works(string phoneNumber, bool expected)
    {
        var response = await _client.GetAsync($"/validatephonenumber?phoneNumber={phoneNumber}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(expected.ToString().ToLower(), content.ToLower());
    }

    [Theory]
    [InlineData("12345678Z", "valid")]
    [InlineData("12345678A", "invalid")]
    [InlineData("1234567A", null)]
    public async Task ValidateSpanishDni_Works(string dni, string expected)
    {
        var response = await _client.GetAsync($"/validatespanishdni?dni={dni}");
        if (expected == null)
        {
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        else
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(expected, content.Replace("\"", ""));
        }
    }

    [Fact]
    public async Task ReturnColorCode_ReturnsHex()
    {
        var response = await _client.GetAsync("/returncolorcode?color=red");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return; // skip if colors.json not present
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("#", content);
    }

    [Fact]
    public async Task TellMeAJoke_ReturnsJoke()
    {
        var response = await _client.GetAsync("/tellmeajoke");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content));
    }

    [Fact]
    public async Task ParseUrl_ReturnsHost()
    {
        var url = "https://example.com:8080/path?query=1#hash";
        var response = await _client.GetAsync($"/parseurl?someurl={System.Net.WebUtility.UrlEncode(url)}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("example.com", json);
    }

    [Fact]
    public async Task ListFiles_ReturnsFiles()
    {
        var response = await _client.GetAsync("/listfiles");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("files", json);
    }

    [Fact]
    public async Task GetFullTextFile_ReturnsLinesOrNotFound()
    {
        var response = await _client.GetAsync("/GetFullTextFile");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return; // skip if sample.txt not present
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        // Should be a JSON array
        Assert.True(json.StartsWith("[") && json.EndsWith("]"));
    }

    [Fact]
    public async Task CalculateMemoryConsumption_ReturnsNumber()
    {
        var response = await _client.GetAsync("/calculatememoryconsumption");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.True(double.TryParse(content, out _));
    }

    [Fact]
    public async Task RandomEuropeanCountry_ReturnsCountryAndIso()
    {
        var response = await _client.GetAsync("/randomeuropeancountry");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("iso", json.ToLower());
    }
}
