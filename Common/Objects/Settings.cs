using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common.Objects
{
    public class Settings
    {
        public bool rememberMe { get; set; } = false; // Remember me login
        public string dbHost { get; set; } = ""; // Database host
        public string dbName { get; set; } = ""; // Database name
        public string dbUser { get; set; } = ""; // Database user
        public string dbPass { get; set; } = ""; // Database password
        public string twitchUsername { get; set; } = ""; // Twitch bots name
        public string accessToken { get; set; } = ""; // Twitch Access Token
        public string targetTwitch { get; set; } = ""; // Twitch target channel
        public string targetTwitchId { get; set; } = ""; // Twitch target channel ID
        public string clientID { get; set; } = ""; // Twitch api client ID
        public string clientSecret { get; set; } = ""; // Twitch api client secret
        public string refreshToken { get; set; } = ""; // Twitch Refresh token
        public int viewerInterval { get; set; } = 10000; // Reload Viewers from twitch api interval
        public int pointInterval { get; set; } = 5000; // How fast should the viewers get points in MS
        public bool rRoulleteChallengeMe { get; set; } = true; // Russian Roullete, allow targeting the broadcaster
        public bool rpsChallengeMe { get; set; } = true; // Rock Paper Siccors, allow targeting the broadcaster

    }
}
