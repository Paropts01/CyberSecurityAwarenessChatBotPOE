using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;

namespace CyberSecurityAwarenessChatBot
{
    public class Chatbot
    {
        private string? userName;
        private string? favouriteTopic;

        public void SetUserName(string name) => userName = name;
        public string GetUserName() => userName ?? "friend";

        public void SetFavouriteTopic(string topic) => favouriteTopic = topic;
        public string GetFavouriteTopic() => favouriteTopic ?? "";
    }
}
