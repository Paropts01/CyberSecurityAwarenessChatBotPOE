using System;

namespace CyberSecurityAwarenessChatBot
{
    // Manages the ongoing conversation state: tracks the current topic being discussed
    // and determines if a user's message is a follow-up request.
    public class ConversationManager
    {
        // Stores the topic (e.g., "password", "scam", "privacy", "phishing") that the bot last provided information about.
        // Used to give relevant follow-up responses when the user asks for "more" or "another tip".
        private string currentTopic;   // Do NOT rename while debugging – stop app first

        // Constructor: initializes the conversation manager with no active topic.
        public ConversationManager()
        {
            currentTopic = null;
        }

        // Sets the current conversation topic (called after the bot gives an answer about a specific topic).
        public void SetCurrentTopic(string topic)
        {
            currentTopic = topic;
        }

        // Returns the current topic (so the bot knows what subject the user might want more info about).
        public string GetCurrentTopic()
        {
            return currentTopic;
        }

        // Examines the user's input to see if they are asking for additional information on the same topic.
        // Returns true if the input contains common follow-up phrases (e.g., "tell me more", "another tip").
        public bool IsFollowUpRequest(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            string lower = input.ToLower();
            return lower.Contains("tell me more") ||
                   lower.Contains("another tip") ||
                   lower.Contains("explain more") ||
                   lower.Contains("more details") ||
                   lower.Contains("more") ||
                   lower.Contains("go on");
        }

        // Generates a follow-up response for the current topic using the provided RandomResponseManager.
        // Returns a random tip for the current topic if one exists, otherwise returns null.
        public string GetFollowUpResponse(RandomResponseManager randomResponse)
        {
            // If no topic has been set, there's nothing to follow up on.
            if (string.IsNullOrEmpty(currentTopic))
                return null;

            // Ask the RandomResponseManager for a new tip on the current topic.
            if (randomResponse != null && randomResponse.SupportsTopic(currentTopic))
                return randomResponse.GetRandomResponse(currentTopic);

            return null;
        }
    }
}
