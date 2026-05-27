using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityAwarenessChatBot
{
    // Analyzes user input to detect emotional tone (sentiment) and returns appropriate empathetic responses.
    public static class SentimentDetector
    {
        // Maps keywords to sentiment categories (e.g., "worried", "curious", "frustrated").
        private static readonly Dictionary<string, string> SentimentKeywords = new()
        {
            { "worried", "worried" }, { "afraid", "worried" }, { "scared", "worried" }, { "concerned", "worried" },
            { "nervous", "worried" }, { "anxious", "worried" }, { "curious", "curious" }, { "interested", "curious" },
            { "want to learn", "curious" }, { "tell me", "curious" }, { "frustrated", "frustrated" }, { "annoyed", "frustrated" },
            { "confused", "frustrated" }, { "difficult", "frustrated" }, { "hard", "frustrated" }
        };

        // Scans the user's input for sentiment keywords.
        // Returns the sentiment category if found, otherwise empty string.
        public static string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            string lower = input.ToLower();
            foreach (var kvp in SentimentKeywords)
                if (lower.Contains(kvp.Key)) return kvp.Value;
            return "";
        }

        // Generates an empathetic response based on detected sentiment and optional topic.
        // If topic is provided, the response will reference it.
        public static string GetSentimentResponse(string sentiment, string topic = null)
        {
            return sentiment switch
            {
                "worried" => "I understand your concern. Cybersecurity can feel overwhelming, but small steps make a big difference. " +
                             (topic != null ? $"Let me give you a helpful tip about {topic}." : "Let me share a useful tip."),
                "curious" => "That's great! Curiosity helps you stay safe online. " +
                             (topic != null ? $"Here's something important about {topic}." : "Let me share an important fact."),
                "frustrated" => "I know it can be frustrating. Let's break it down simply. " +
                                (topic != null ? $"Here's an easy tip for {topic}." : "Here's a simple tip to help."),
                _ => ""
            };
        }
    }
}
