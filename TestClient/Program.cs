using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await Execute();
			Console.ReadLine();
		}

		private static async Task Execute()
		{
			// discover endpoints from metadata
			var client = new HttpClient();
			var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
			if (disco.IsError)
			{
				Console.WriteLine(disco.Error);
				return;
			}

			// request token
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = disco.TokenEndpoint,
				ClientId = "client",
				ClientSecret = "secret",
				Scope = "api1"
			});

			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}
			else
			{
				Console.WriteLine(tokenResponse.Json);
			}

			// call api
			var apiClient = new HttpClient();
			apiClient.SetBearerToken(tokenResponse.AccessToken);

			var response = await apiClient.GetAsync("http://localhost:5001/identity");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JArray.Parse(content));
			}
		}
	}
}
