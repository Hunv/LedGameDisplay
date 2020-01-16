using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace LedGameDisplayFrontend.Data
{
    public static class PanelApiService
    {
        public static async Task<Tournament[]> GetTournamentsAsync()
        {
            var tournamentList = new Tournament[0];
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync("https://localhost:44302/api/tournament"))
            {
                JsonSerializerOptions jso = new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                tournamentList = await JsonSerializer.DeserializeAsync<Tournament[]>(jsonStream, jso);
            }

            return tournamentList;
        }

        public static async Task<Tournament> GetTournamentAsync(int tournamentId)
        {
            var tournament = new Tournament();
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync("https://localhost:44302/api/tournament/" + tournamentId))
            {
                JsonSerializerOptions jso = new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                tournament = await JsonSerializer.DeserializeAsync<Tournament>(jsonStream, jso);
            }

            return tournament;
        }

        public static async Task<Team[]> GetTeamsAsync()
        {
            var teamList = new Team[0];
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync("https://localhost:44302/api/team"))
            {
                JsonSerializerOptions jso = new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                teamList = await JsonSerializer.DeserializeAsync<Team[]>(jsonStream, jso);
            }

            return teamList;
        }
        public static async Task<Player[]> GetPlayersAsync()
        {
            var playerList = new Player[0];
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync("https://localhost:44302/api/player"))
            {
                JsonSerializerOptions jso = new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                playerList = await JsonSerializer.DeserializeAsync<Player[]>(jsonStream, jso);
            }

            return playerList;
        }

    }
}
