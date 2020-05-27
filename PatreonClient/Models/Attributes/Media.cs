using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    public class Media
    {
        [JsonPropertyName("file_name")] public string FileName { get; set; }
        [JsonPropertyName("size_bytes")] public int SizeBytes { get; set; }
        [JsonPropertyName("mimetype")] public string MimeType { get; set; }
        [JsonPropertyName("state")] public string State { get; set; }
        [JsonPropertyName("owner_type")] public string OwnerType { get; set; }
        [JsonPropertyName("owner_id")] public string OwnerId { get; set; }

        [JsonPropertyName("owner_relationship")]
        public string OwnerRelationship { get; set; }

        [JsonPropertyName("upload_expires_at")]
        public DateTimeOffset UploadExpiresAt { get; set; }

        [JsonPropertyName("upload_url")] public string UploadUrl { get; set; }

        [JsonPropertyName("upload_parameters")]
        public object UploadParameters { get; set; }

        [JsonPropertyName("download_url")] public string DownloadUrl { get; set; }
        [JsonPropertyName("image_urls")] public object ImageUrls { get; set; }
        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }
        [JsonPropertyName("metadata")] public object Metadata { get; set; }
    }
}