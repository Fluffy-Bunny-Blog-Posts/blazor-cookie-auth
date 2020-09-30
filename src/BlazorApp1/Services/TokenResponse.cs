using System.Text.Json.Serialization;

namespace BlazorApp1.Services
{
    public class TokenResponse
    {
        [JsonPropertyName("authority")]
        public string Authority { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

    }
}
