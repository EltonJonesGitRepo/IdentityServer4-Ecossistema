using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IndentityServerEcossistema.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string token = await ObterToken();

            if (string.IsNullOrWhiteSpace(token))
                return;

            string resposta = await ObterDadosApi(token);

            Console.WriteLine($"Resposta: {resposta}");

            Console.ReadKey();
        }

        private static async Task<string> ObterDadosApi(string token)
        {            
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44356/");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage resposta = await client.GetAsync("doughnuts/endpoint-console");

            var result = await resposta.Content.ReadAsStringAsync();

            return result;
        }

        
        private static async Task<string> ObterToken()
        {
            var client = new HttpClient();

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                ClientId = "console-cliente",
                ClientSecret = "console-cliente",
                Scope = "doughnutapi"
                
                //todo: configurar o escopo abaixo para funcionar
                //Scope = "console-cliente"
            });


            if (response.IsError)
            {
                Console.WriteLine($"Falha ao obter token de acesso. Erro: {response.Error}");                
                return null;
            }

            return response.AccessToken;
        }

    }
}
