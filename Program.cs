using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class Value
    {
        public string Id { get; set; }
        public string Name { get; set; }
       
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Value value)
        {
            Console.WriteLine($"Name: {value.Name}");
        }

        static async Task<Uri> CreateProductAsync(Value product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Value> GetProductAsync(string path)
        {
            Value product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Value>();
            }
            return product;
        }

        static async Task<Value> UpdateProductAsync(Value product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Value>();
            return product;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://candidate.tuva.ru/swagger/");
            request.Credentials = new NetworkCredential("2801f6e8-0be3-11ed-972d-13fb191cd00c", "2b8172da-0be3-11ed-9d4d-df7c5b3d8aa6");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Value product = new Value
                {
                    Name = "ValueOdin",
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine($"Created at {url}");

                // Get the product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}