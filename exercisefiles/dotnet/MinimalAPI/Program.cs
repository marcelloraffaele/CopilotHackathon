/// <summary>
/// Entry point for the Minimal API application.
/// Configures endpoints for various utility operations such as date calculations,
/// validation, color code lookup, jokes, movie search, URL parsing, file listing,
/// file content search, memory consumption, and random country selection.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ADD NEW ENDPOINTS HERE

/// <summary>
/// Root endpoint. Returns "Hello World!".
/// </summary>
app.MapGet("/", () => "Hello World!");

/// <summary>
/// Calculates the absolute number of days between two dates.
/// </summary>
/// <param name="date1">First date string.</param>
/// <param name="date2">Second date string.</param>
/// <returns>Number of days between the dates.</returns>
app.MapGet("/DaysBetweenDates", (string date1, string date2) =>
{
    if (!DateTime.TryParse(date1, out var d1) || !DateTime.TryParse(date2, out var d2))
    {
        return Results.BadRequest("Invalid date format. Please use a valid date string.");
    }
    var daysBetween = Math.Abs((d2 - d1).TotalDays);
    return Results.Ok(daysBetween);
});

/**
/validatephonenumber:

receive by querystring a parameter called phoneNumber
validate phoneNumber with Spanish format, for example +34666777888
if phoneNumber is valid return true
*/
/// <summary>
/// Validates a Spanish phone number format (+34 followed by 9 digits).
/// </summary>
/// <param name="phoneNumber">Phone number to validate.</param>
/// <returns>True if valid, otherwise false.</returns>
app.MapGet("/validatephonenumber", (string phoneNumber) =>
{
    var isValid = System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\+34\d{9}$");
    return Results.Ok(isValid);
});

/**
/validatespanishdni:

receive by querystring a parameter called dni
calculate DNI letter
if DNI is valid return "valid"
if DNI is not valid return "invalid"
*/
/// <summary>
/// Validates a Spanish DNI (8 digits and 1 letter).
/// </summary>
/// <param name="dni">DNI to validate.</param>
/// <returns>"valid" if correct, otherwise "invalid".</returns>
app.MapGet("/validatespanishdni", (string dni) =>
{
    if (dni.Length != 9 || !int.TryParse(dni.Substring(0, 8), out var dniNumber))
    {
        return Results.BadRequest("Invalid DNI format.");
    }

    var letters = "TRWAGMYFPDXBNJZSQVHLCKE";
    var letter = letters[dniNumber % 23];

    return Results.Ok(letter == dni[8] ? "valid" : "invalid");
});

/**
/returncolorcode:

receive by querystring a parameter called color
read colors.json file and return the rgba field
get color var from querystring
iterate for each color in colors.json to find the color
return the code.hex field
*/
/// <summary>
/// Returns the HEX code for a given color name from colors.json.
/// </summary>
/// <param name="color">Color name to search for.</param>
/// <returns>HEX code of the color.</returns>
app.MapGet("/returncolorcode", async (string color) =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "colors.json");
    if (!File.Exists(filePath))
    {
        return Results.NotFound("colors.json file not found.");
    }

    var json = await File.ReadAllTextAsync(filePath);
    var colors = System.Text.Json.JsonSerializer.Deserialize<List<Color>>(json);

    var foundColor = colors?.FirstOrDefault(c =>
        c.Name.Equals(color, StringComparison.OrdinalIgnoreCase));

    if (foundColor == null)
    {
        return Results.NotFound("Color not found.");
    }

    return Results.Ok(foundColor.Code?.HEX);
});

/**
/tellmeajoke:

Make a call to the joke api and return a random joke
*/
/// <summary>
/// Fetches a random joke from the JokeAPI.
/// </summary>
/// <returns>A random joke string.</returns>
app.MapGet("/tellmeajoke", async () =>
{
    using var httpClient = new HttpClient();
    // Using the official JokeAPI: https://v2.jokeapi.dev/joke/Any
    var response = await httpClient.GetAsync("https://v2.jokeapi.dev/joke/Any");
    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem("Failed to fetch a joke.");
    }

    var json = await response.Content.ReadAsStringAsync();
    using var doc = System.Text.Json.JsonDocument.Parse(json);
    var root = doc.RootElement;

    string joke;
    if (root.GetProperty("type").GetString() == "single")
    {
        joke = root.GetProperty("joke").GetString();
    }
    else
    {
        joke = $"{root.GetProperty("setup").GetString()} {root.GetProperty("delivery").GetString()}";
    }

    return Results.Ok(joke);
});

/**
/moviesbydirector:

Receive by querystring a parameter called director
Make a call to the movie api and return a list of movies of that director
Return the full list of movies
*/
/// <summary>
/// Searches for movies by a given director using the OMDb API.
/// </summary>
/// <param name="director">Director's name.</param>
/// <returns>List of movies directed by the specified director.</returns>
app.MapGet("/moviesbydirector", async (string director) =>
{
    // Replace with your actual OMDb API key
    var apiKey = "YOUR_OMDB_API_KEY";
    var httpClient = new HttpClient();

    // OMDb API does not support direct search by director, so we need to search by director name in the "s" parameter (title)
    // and then filter results by director in detail calls.
    // For demonstration, we'll search for movies with a common word and filter by director.

    var searchUrl = $"https://www.omdbapi.com/?apikey={apiKey}&s={Uri.EscapeDataString(director)}&type=movie";
    var searchResponse = await httpClient.GetAsync(searchUrl);

    if (!searchResponse.IsSuccessStatusCode)
    {
        return Results.Problem("Failed to fetch movies from OMDb API.");
    }

    var searchJson = await searchResponse.Content.ReadAsStringAsync();
    using var searchDoc = System.Text.Json.JsonDocument.Parse(searchJson);
    var searchRoot = searchDoc.RootElement;

    if (!searchRoot.TryGetProperty("Search", out var searchResults))
    {
        return Results.Ok(new List<object>()); // No movies found
    }

    var movies = new List<object>();

    foreach (var movie in searchResults.EnumerateArray())
    {
        var imdbID = movie.GetProperty("imdbID").GetString();
        var detailUrl = $"https://www.omdbapi.com/?apikey={apiKey}&i={imdbID}";
        var detailResponse = await httpClient.GetAsync(detailUrl);

        if (!detailResponse.IsSuccessStatusCode)
            continue;

        var detailJson = await detailResponse.Content.ReadAsStringAsync();
        using var detailDoc = System.Text.Json.JsonDocument.Parse(detailJson);
        var detailRoot = detailDoc.RootElement;

        if (detailRoot.TryGetProperty("Director", out var directorProp) &&
            directorProp.GetString()?.Contains(director, StringComparison.OrdinalIgnoreCase) == true)
        {
            movies.Add(detailRoot);
        }
    }

    return Results.Ok(movies);
});

/**
/parseurl:

Retrieves a parameter from querystring called someurl
Parse the url and return the protocol, host, port, path, querystring and hash
Return the parsed host
*/
/// <summary>
/// Parses a URL and returns its components.
/// </summary>
/// <param name="someurl">URL to parse.</param>
/// <returns>Object with protocol, host, port, path, querystring, hash, and parsedHost.</returns>
app.MapGet("/parseurl", (string someurl) =>
{
    if (string.IsNullOrWhiteSpace(someurl))
    {
        return Results.BadRequest("Parameter 'someurl' is required.");
    }

    try
    {
        var uri = new Uri(someurl);

        var result = new
        {
            protocol = uri.Scheme,
            host = uri.Host,
            port = uri.IsDefaultPort ? null : uri.Port.ToString(),
            path = uri.AbsolutePath,
            querystring = uri.Query,
            hash = uri.Fragment,
            parsedHost = uri.Host
        };

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Invalid URL: {ex.Message}");
    }
});

/// <summary>
/// Lists files in the current directory.
/// </summary>
/// <returns>Current directory and list of file names.</returns>
app.MapGet("/listfiles", () =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    var files = Directory.GetFiles(currentDirectory).Select(Path.GetFileName).ToList();
    return Results.Ok(new { currentDirectory, files });
});

/**
/GetFullTextFile:

Read `sample.txt` and return lines that contains the word "Fusce"
*/
/// <summary>
/// Reads sample.txt and returns lines containing the word "Fusce".
/// </summary>
/// <returns>List of matching lines.</returns>
app.MapGet("/GetFullTextFile", async () =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "sample.txt");
    if (!File.Exists(filePath))
    {
        return Results.NotFound("sample.txt file not found.");
    }

    var lines = await File.ReadAllLinesAsync(filePath);
    var matchingLines = lines.Where(line => line.Contains("Fusce", StringComparison.OrdinalIgnoreCase)).ToList();

    return Results.Ok(matchingLines);
});

/**
/calculatememoryconsumption:

Return the memory consumption of the process in GB, rounded to 2 decimals
*/
/// <summary>
/// Returns the memory consumption of the process in GB, rounded to 2 decimals.
/// </summary>
/// <returns>Memory usage in GB.</returns>
app.MapGet("/calculatememoryconsumption", () =>
{
    var process = System.Diagnostics.Process.GetCurrentProcess();
    double memoryInGB = process.WorkingSet64 / (1024.0 * 1024 * 1024);
    double rounded = Math.Round(memoryInGB, 2);
    return Results.Ok(rounded);
});

/**
/randomeuropeancountry:

Make an array of european countries and its iso codes
Return a random country from the array
Return the country and its iso code
*/
/// <summary>
/// Returns a random European country and its ISO code.
/// </summary>
/// <returns>Object with country name and ISO code.</returns>
app.MapGet("/randomeuropeancountry", () =>
{
    var countries = new[]
    {
        new { Name = "Austria", ISO = "AT" },
        new { Name = "Belgium", ISO = "BE" },
        new { Name = "Bulgaria", ISO = "BG" },
        new { Name = "Croatia", ISO = "HR" },
        new { Name = "Cyprus", ISO = "CY" },
        new { Name = "Czech Republic", ISO = "CZ" },
        new { Name = "Denmark", ISO = "DK" },
        new { Name = "Estonia", ISO = "EE" },
        new { Name = "Finland", ISO = "FI" },
        new { Name = "France", ISO = "FR" },
        new { Name = "Germany", ISO = "DE" },
        new { Name = "Greece", ISO = "GR" },
        new { Name = "Hungary", ISO = "HU" },
        new { Name = "Iceland", ISO = "IS" },
        new { Name = "Ireland", ISO = "IE" },
        new { Name = "Italy", ISO = "IT" },
        new { Name = "Latvia", ISO = "LV" },
        new { Name = "Lithuania", ISO = "LT" },
        new { Name = "Luxembourg", ISO = "LU" },
        new { Name = "Malta", ISO = "MT" },
        new { Name = "Netherlands", ISO = "NL" },
        new { Name = "Norway", ISO = "NO" },
        new { Name = "Poland", ISO = "PL" },
        new { Name = "Portugal", ISO = "PT" },
        new { Name = "Romania", ISO = "RO" },
        new { Name = "Slovakia", ISO = "SK" },
        new { Name = "Slovenia", ISO = "SI" },
        new { Name = "Spain", ISO = "ES" },
        new { Name = "Sweden", ISO = "SE" },
        new { Name = "Switzerland", ISO = "CH" }
    };

    var random = new Random();
    var country = countries[random.Next(countries.Length)];
    return Results.Ok(country);
});

app.Run();

/// <summary>
/// Partial Program class for test project access.
/// </summary>
public partial class Program
{ }