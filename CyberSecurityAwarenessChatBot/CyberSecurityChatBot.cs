using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityAwarenessChatBot
{
    public class CyberSecurityChatBot
    {
        private Dictionary<string, string[]> topics;
        private RandomResponseManager randomResponseManager;
        private ConversationManager conversationManager;

        public CyberSecurityChatBot()
        {
            topics = new Dictionary<string, string[]>
            {
                { "password", new string[] { "A password is your digital door key. In cybersecurity, a good key must be long and complex." } },
                { "privacy",  new string[] { "Privacy keeps your personal information locked away. Encryption helps protect your data." } },
                { "scam",     new string[] { "A scam is a digital trick designed to fool you. Scammers hack your emotions." } }
            };
            randomResponseManager = new RandomResponseManager();
            conversationManager = new ConversationManager();
        }

        public bool ValidateInput(string input) => !string.IsNullOrWhiteSpace(input);

        public bool ContainsKeyword(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            string lower = input.ToLower();
            foreach (var topic in topics)
                if (lower.Contains(topic.Key)) return true;
            return false;
        }

        public string GetKeywordResponse(string input)
        {
            if (string.IsNullOrEmpty(input)) return "Please say something about cybersecurity.";
            string lower = input.ToLower();
            foreach (var topic in topics)
            {
                if (lower.Contains(topic.Key))
                {
                    conversationManager.SetCurrentTopic(topic.Key);
                    string randomResp = randomResponseManager.GetRandomResponse(topic.Key);
                    if (!string.IsNullOrEmpty(randomResp)) return randomResp;
                    return topic.Value[0];
                }
            }
            return "I'm here to help with passwords, privacy, and scams.";
        }

        public string GetFollowUpResponse()
        {
            string response = conversationManager.GetFollowUpResponse(randomResponseManager);
            return string.IsNullOrEmpty(response) ? "Please ask about a cybersecurity topic first." : response;
        }
    }
}
