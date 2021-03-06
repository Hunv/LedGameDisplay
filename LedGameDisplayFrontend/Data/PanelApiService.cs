using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using LedGameDisplayFrontend.Data.JsonModel;

namespace LedGameDisplayFrontend.Data
{
    public static class PanelApiService
    {
        private static readonly string _ServerBaseUrl = "http://localhost:5000/api/";
        public static async Task<Tournament[]> GetTournamentsAsync()
        {
            var tournamentList = new Tournament[0];
            HttpClient client = new HttpClient();

            using (Stream jsonStream = await client.GetStreamAsync(_ServerBaseUrl + "tournament"))
            {
                JsonSerializerSettings jss = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var sR = new StreamReader(jsonStream);
                var json = await sR.ReadToEndAsync();
                sR.Close();

                tournamentList = JsonConvert.DeserializeObject<Tournament[]>(json, jss);
            }

            return tournamentList;
        }

        public static async Task<Tournament> GetTournamentAsync(int tournamentId)
        {
            var tournament = new Tournament();
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync(_ServerBaseUrl + "tournament/" + tournamentId))
            {
                JsonSerializerSettings jss = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var sR = new StreamReader(jsonStream);
                var json = await sR.ReadToEndAsync();
                sR.Close();

                tournament = JsonConvert.DeserializeObject<Tournament>(json, jss);
            }

            return tournament;
        }

        public static async Task<Match> GetMatchAsync(int matchId)
        {
            var match = new Match();
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync(_ServerBaseUrl + "match/" + matchId))
            {
                JsonSerializerSettings jss = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var sR = new StreamReader(jsonStream);
                var json = await sR.ReadToEndAsync();
                sR.Close();

                match = JsonConvert.DeserializeObject<Match>(json, jss);
            }

            return match;
        }

        public static async Task<Match> GetLiveMatchAsync()
        {
            var match = new Match();
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync(_ServerBaseUrl + "livematch"))
            {
                JsonSerializerSettings jss = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var sR = new StreamReader(jsonStream);
                var json = await sR.ReadToEndAsync();
                sR.Close();

                match = JsonConvert.DeserializeObject<Match>(json, jss);
            }

            return match;
        }

        public static async Task<Team[]> GetTeamsAsync()
        {
            var teamList = new Team[0];
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync(_ServerBaseUrl + "team"))
            {
                JsonSerializerSettings jss = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var sR = new StreamReader(jsonStream);
                var json = await sR.ReadToEndAsync();
                sR.Close();
                teamList = JsonConvert.DeserializeObject<Team[]>(json, jss);
            }

            return teamList;
        }

        public static async Task<Player[]> GetPlayersAsync()
        {
            var playerList = new Player[0];
            HttpClient client = new HttpClient();

            using (var jsonStream = await client.GetStreamAsync(_ServerBaseUrl + "player"))
            {
                JsonSerializerSettings jss = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var sR = new StreamReader(jsonStream);
                var json = await sR.ReadToEndAsync();
                sR.Close();

                playerList = JsonConvert.DeserializeObject<Player[]>(json, jss);
            }

            return playerList;
        }

        public static async Task NewMatchAsync(NewMatchData match)
        {
            var js = new JsonSerializerSettings() 
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, 
                NullValueHandling = NullValueHandling.Ignore, 
                DefaultValueHandling = DefaultValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            var json = JsonConvert.SerializeObject(match, js);

            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(_ServerBaseUrl + "match"),
                Content = new StringContent(json)
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }



        public static async Task NewPlayerAsync(NewPlayerData player)
        {
            var js = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            var json = JsonConvert.SerializeObject(player, js);

            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(_ServerBaseUrl + "player"),
                Content = new StringContent(json)
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            { 
                var responseBody = await response.Content.ReadAsStringAsync();
        }
        }

        public static async Task NewTeamAsync(NewTeamData team)
        {
            var js = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            var json = JsonConvert.SerializeObject(team, js);

            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(_ServerBaseUrl + "team"),
                Content = new StringContent(json)
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }
        public static async Task NewTournamentAsync(NewTournamentData tournament)
        {
            var js = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            var json = JsonConvert.SerializeObject(tournament, js);

            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(_ServerBaseUrl + "tournament"),
                Content = new StringContent(json)
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task UpdateMatchAsync(UpdateMatchOngoing match)
        {
            var js = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            var json = JsonConvert.SerializeObject(match, js);

            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("PUT"),
                RequestUri = new Uri(_ServerBaseUrl + "match/" + match.Id),
                Content = new StringContent(json)
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task UpdateLiveMatchAsync(int matchId, string action)
        {
            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("PUT"),
                RequestUri = new Uri(_ServerBaseUrl + "livematch/" + matchId + "?action=" + action),
                Content = new StringContent("")
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }


        public static async Task UpdateAreaTextAsync(string area, string text)
        {
            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(_ServerBaseUrl + "test/" + area + "/" + text),
                Content = new StringContent("")
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }
    }
}