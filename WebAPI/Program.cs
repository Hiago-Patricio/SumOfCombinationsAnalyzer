using System.Text.Json;
using Combinatorics.Collections;

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

app.MapGet("/combinations", () =>
{
    var range = Enumerable.Range(1, 3).Select(x => (float)x).ToList();

    var combinations = GenerateCombinations(range);

    return combinations;
});

app.MapPost("/combinations", (List<float> values) =>
{
    // Generate combinations based on the received values
    var combinations = GenerateCombinations(values);

    // Return the combinations as JSON
    return Results.Json(combinations);
});


app.Run();

List<Combination> GenerateCombinations (IList<float>? values)
{
    var result = new List<Combination>();

    for (int i = 1; i <= values?.Count; i++)
    {
        var combinations = new Combinations<float>(values, i, GenerateOption.WithoutRepetition);
    
        foreach (var combination in combinations)
        {
            result.Add(new Combination((List<float>)combination));
        }
    }
    
    return result;
}

record Combination(List<float> Values)
{
    public float Sum => Values.Sum();
}