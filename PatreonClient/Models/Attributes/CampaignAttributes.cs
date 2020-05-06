using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    public class CampaignAttributes
    {
        [JsonPropertyName("creation_name")]
        public string CreationName { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }
}