using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityAwarenessChatBot
{
    public class ConversationManager
    {
        private string? _currentTopic;
        private int _followUpCount;

        public ConversationManager()
        {
            _currentTopic = null;
            _followUpCount = 0;
        }

        public void SetCurrentTopic(string topic)
        {
            _currentTopic = topic?.ToLower();
            _followUpCount = 0;
        }

        public bool IsFollowUpRequest(string userInput)
        {
            if (string.IsNullOrEmpty(_currentTopic)) return false;
            string input = userInput.ToLower();
            return input.Contains("more") || input.Contains("another") || input.Contains("explain") ||
                   input.Contains("tell me more") || input.Contains("continue") || input.Contains("elaborate");
        }

        public string GetFollowUpResponse(RandomResponseManager randomResponse)
        {
            if (string.IsNullOrEmpty(_currentTopic)) return null;
            if (_followUpCount >= 3)
                return "That's all I have on this topic for now. Feel free to ask about another cybersecurity area!";

            _followUpCount++;
            string response = randomResponse.GetRandomResponse(_currentTopic);
            if (response != null)
                return "Here's another tip: " + response;

            return _currentTopic switch
            {
                "password" => "Also, avoid writing down passwords – use a secure password manager instead.",
                "scam" => "Another sign of a scam: they ask for payment via gift cards or wire transfer.",
                "privacy" => "Remember to log out of accounts on shared devices.",
                "phishing" => "Forward suspicious emails to report@phishing.gov.",
                _ => "Could you ask more specifically about cybersecurity?"
            };
        }

        public void Reset()
        {
            _currentTopic = null;
            _followUpCount = 0;
        }
    }
}
