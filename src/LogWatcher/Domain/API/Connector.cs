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

        // GET
        //var response = await client.GetAsync("http://127.0.0.1:8000/api");
        //var content = await response.Content.ReadAsStringAsync();

        // POST
        //var stringForApi = "{\"name\":\"Foo\",\"description\":\"An optional description\",\"price\":45.3,\"tax\":3.5}";

        private const string uri = "http://localhost:8000";

        public async Task<string> SendSearch(SearchSet searchSet)
        {
            var endpoint = "searchset";

            using (var client = new HttpClient())
            {
                var payload = JsonConvert.SerializeObject(searchSet);
                var query = new StringContent(payload, Encoding.UTF8, "application/json");
                var result = await client.PostAsync($"{uri}/{endpoint}", query); // http://localhost:8000/items
                var content = await result.Content.ReadAsStringAsync();

                return content;
            }
        }
    }
}
