using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var baseURL = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://ca-order-processor";

var client = new HttpClient();
client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

while (true)
{
    for (int i = 1; i <= 20; i++)
    {
        try {
            var order = new Order(i);
            var orderJson = JsonSerializer.Serialize<Order>(order);
            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

            // Invoking a service
            var response = await client.PostAsync($"{baseURL}/orders", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Order passed: " + order);

                await Task.Delay(TimeSpan.FromSeconds(1));
                continue;
            }
            Console.WriteLine("Order failed: " + order);            
        } catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
        }
        
    }
    await Task.Delay(TimeSpan.FromSeconds(10));
}

public record Order([property: JsonPropertyName("orderId")] int OrderId);
