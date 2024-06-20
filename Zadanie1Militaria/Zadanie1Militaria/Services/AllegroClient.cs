using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Zadanie1Militaria.Models;
using Newtonsoft.Json.Linq;

namespace Zadanie1Militaria.Services
{
    public class AllegroClient
    {
        private readonly HttpClient _httpClient;

        public AllegroClient(string accessToken)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<List<BillingEntry>> GetBillingEntriesAsync(string orderId)
        {
            var entries = new List<BillingEntry>();
            try
            {
                var response = await _httpClient.GetAsync($"https://api.allegro.pl/billing/billing-entries?order.id={orderId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);
                    foreach (var item in json["billingEntries"])
                    {
                        entries.Add(new BillingEntry
                        {
                            OrderId = orderId,
                            EntryType = item["type"]["id"].ToString(),
                            Amount = decimal.Parse(item["value"]["amount"].ToString()),
                            EntryDate = DateTime.Parse(item["occurredAt"].ToString())
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve billing entries for order {orderId}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving billing entries for order {orderId}: {ex.Message}");
            }
            return entries;
        }
    }
}