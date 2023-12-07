var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapPost("/orders", (Order order) =>
{
    var failureRate = Environment.GetEnvironmentVariable("FAILURE_RATE") ?? "20";
    var random = new Random();
    if (random.Next(1, 100) <= int.Parse(failureRate))
    {
        Console.WriteLine("Order failed : " + order);
        throw new Exception("Order failed");
    }
    Console.WriteLine("Order received : " + order);
    return order.ToString();
});

await app.RunAsync();

public record Order(int orderId);
