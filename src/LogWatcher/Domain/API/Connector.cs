﻿using Domain.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API
{
    public class Connector
    {        
        private const string uri = "http://localhost:8000";

        public async Task<string> SendSearch(SearchSet searchSet, string endpoint = "search")
        {
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