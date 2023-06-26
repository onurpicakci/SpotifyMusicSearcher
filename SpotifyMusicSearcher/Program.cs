using Newtonsoft.Json;
using RestSharp;

class Program
{
    static async Task Main(string[] args)
    {
        string apiUrl = "https://api.spotify.com/v1/search";
        string searchTerm = "SearchQuery"; 
        
        var headers = new
        {
            Authorization =
                "Bearer AccessToken" 
        };

        var parameters = new
        {
            q = searchTerm,
            type = "track",
            limit = 10 
        };

        using (var httpClient = new HttpClient())
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{apiUrl}?q={parameters.q}&type={parameters.type}&limit={parameters.limit}");
            request.Headers.TryAddWithoutValidation("Authorization", headers.Authorization);

            
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

        
            dynamic result = JsonConvert.DeserializeObject(content);
            var tracks = result?.tracks?.items;

            if (tracks != null && tracks.Count > 0)
            {
                string spotifyUrl = tracks[0]?.external_urls?.spotify;
                Console.WriteLine($"Spotify URL: {spotifyUrl}");
                
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(spotifyUrl)
                    { UseShellExecute = true });
            }
            else
            {
                Console.WriteLine("No tracks found.");
            }
        }

        Console.ReadLine();
    }
}