using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;

namespace CyberSecurityAwarenessChatBot
{
    // Simple class to store and retrieve user-specific information:
    // the user's name and their favorite cybersecurity topic.
    public class Chatbot
    {
        // Backing fields for user data (nullable to allow uninitialized state)
        private string? userName;       // Stores the user's name once provided
        private string? favouriteTopic; // Stores the topic the user prefers (e.g., "password", "scam")

        // Sets the user's name
        public void SetUserName(string name) => userName = name;

        // Returns the user's name, or "friend" if not yet set
        public string GetUserName() => userName ?? "friend";

        // Sets the user's favorite topic
        public void SetFavouriteTopic(string topic) => favouriteTopic = topic;

        // Returns the favorite topic, or empty string if none set
        public string GetFavouriteTopic() => favouriteTopic ?? "";
    }
}
