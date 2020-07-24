using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    public class User
    {
        [JsonPropertyName("email")] public string Email { get; set; }

        [JsonPropertyName("first_name")] public string FirstName { get; set; }

        [JsonPropertyName("last_name")] public string LastName { get; set; }

        [JsonPropertyName("full_name")] public string FullName { get; set; }

        [JsonPropertyName("is_email_verified")]
        public bool IsEmailVerified { get; set; }

        [JsonPropertyName("vanity")] public string Vanity { get; set; }

        [JsonPropertyName("about")] public string About { get; set; }

        [JsonPropertyName("image_url")] public string ImageUrl { get; set; }

        [JsonPropertyName("thumb_url")] public string ThumbUrl { get; set; }

        [JsonPropertyName("can_see_nsfw")] public bool? CanSeeNsfw { get; set; }

        [JsonPropertyName("created")] public DateTimeOffset? Created { get; set; }

        [JsonPropertyName("url")] public string Url { get; set; }

        [JsonPropertyName("like_count")] public int Likes { get; set; }

        [JsonPropertyName("hide_pledges")] public bool HidePledges { get; set; }

        [JsonPropertyName("social_connections")]
        public Dictionary<string, Social> SocialConnections { get; set; }
    }
}