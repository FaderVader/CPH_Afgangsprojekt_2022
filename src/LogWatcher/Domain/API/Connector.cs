using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API
{
    public class Connector
    {
        // ref: https://www.dotnetfunda.com/articles/show/2341/crud-operation-using-web-api-and-windows-application
        private const string uri = "http://localhost:8000";

        public async Task<string> SendSearch(SearchSet searchSet)
        {
            const string endpoint = "search";

            using (var client = new HttpClient())
            {
                var payload = JsonConvert.SerializeObject(searchSet);
                var query = new StringContent(payload, Encoding.UTF8, "application/json");
                var result = await client.PostAsync($"{uri}/{endpoint}", query);
                var content = await result.Content.ReadAsStringAsync();

                return content;
            }
        }

        public async Task<string> PullResult()
        {
            const string endpoint = "retrieve";

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{uri}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}