using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessChatBot
{
    
    // Simple natural language processor that detects intents from user input.
    
    public static class NLPProcessor
    {
        
        //Detects the intent of the user's input.
        // Returns one of: "TASK", "QUIZ", "SUMMARY", "PASSWORD", "PHISHING", or "UNKNOWN".
        
        public static string DetectIntent(string input)
        {
            input = input.ToLower();

            // Task-related keywords
            if (ContainsAny(input,
                "task",
                "add task",
                "new task",
                "create task",
                "remind",
                "reminder",
                "remember",
                "schedule",
                "need to",
                "must remember",
                "todo",
                "to do"))
            {
                return "TASK";
            }

            // Quiz-related keywords
            if (ContainsAny(input,
                "quiz",
                "test",
                "question",
                "challenge",
                "start quiz",
                "take quiz"))
            {
                return "QUIZ";
            }

            // Activity log / summary
            if (ContainsAny(input,
                "activity",
                "activity log",
                "show log",
                "what have you done",
                "history",
                "recent actions"))
            {
                return "SUMMARY";
            }

            // Password topics
            if (ContainsAny(input,
                "password",
                "passphrase",
                "credentials"))
            {
                return "PASSWORD";
            }

            // Phishing / scam topics
            if (ContainsAny(input,
                "phishing",
                "scam",
                "fake email",
                "suspicious email"))
            {
                return "PHISHING";
            }

            return "UNKNOWN";
        }

        //Helper: checks if the text contains any of the given keywords.
        private static bool ContainsAny(string text, params string[] keywords)
        {
            return keywords.Any(keyword => text.Contains(keyword));
        }
    }
}