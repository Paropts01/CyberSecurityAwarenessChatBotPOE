using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessChatBot
{
    public static class NLPProcessor
    {
        public static string DetectIntent(string input)
        {
            input = input.ToLower();

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

            if (ContainsAny(input,
                "password",
                "passphrase",
                "credentials"))
            {
                return "PASSWORD";
            }

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

        private static bool ContainsAny(string text, params string[] keywords)
        {
            return keywords.Any(keyword => text.Contains(keyword));
        }
    }
}