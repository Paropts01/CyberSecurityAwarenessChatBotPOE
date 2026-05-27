using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityAwarenessChatBot
{
    public static class SentimentDetector
    {
        private static readonly Dictionary<string, string> SentimentKeywords = new()
        {
            { "worried", "worried" }, { "afraid", "worried" }, { "scared", "worried" }, { "concerned", "worried" },
            { "nervous", "worried" }, { "anxious", "worried" }, { "curious", "curious" }, { "interested", "curious" },
            { "want to learn", "curious" }, { "tell me", "curious" }, { "frustrated", "frustrated" }, { "annoyed", "frustrated" },
            { "confused", "frustrated" }, { "difficult", "frustrated" }, { "hard", "frustrated" }
        };

        public static string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            string lower = input.ToLower();
            foreach (var kvp in SentimentKeywords)
                if (lower.Contains(kvp.Key)) return kvp.Value;
            return "";
        }

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
