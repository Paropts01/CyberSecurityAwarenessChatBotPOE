using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessChatBot
{
    // Provides detailed cybersecurity answers based on keyword detection.
    // Also validates user input and checks for relevant topics.
    public class CyberSecurityChatBot
    {
        // Stores comprehensive responses for each major cybersecurity topic.
        // Keys are topic names (case-insensitive), values are detailed advice strings.
        private Dictionary<string, string> keywordResponses;

        // Constructor: initializes the dictionary with four core topics.
        public CyberSecurityChatBot()
        {
            // Use case-insensitive comparison so "Password" and "password" both work.
            keywordResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = "Password Security: A strong password is your first defence. Use at least 12 characters, mixing uppercase, lowercase, numbers, and symbols. Never reuse passwords across different sites – if one account is breached, others become vulnerable. Consider using a passphrase like 'Blue-Horse-Sunny-Rain' which is easier to remember. Enable Two-Factor Authentication (2FA) on all important accounts (email, banking, social media). 2FA adds a second layer, such as a code sent to your phone, so even if someone steals your password, they can't log in. Also, use a reputable password manager (Bitwarden, 1Password) to generate and store unique passwords securely.",

                ["scam"] = "Recognising Scams: Scammers create a false sense of urgency – 'Your account will be closed in 24 hours!' or 'You've won a prize, claim now!' Always verify through official channels. Never click links in unsolicited emails or SMS. Look for red flags: poor grammar, generic greetings ('Dear Customer'), requests for gift cards, wire transfers, or cryptocurrency. If someone claims to be from your bank, hang up and call the number on the back of your card. Remember: legitimate companies never ask for your password or 2FA codes. Report scams to your local authorities and the FTC.",

                ["privacy"] = "Protecting Your Privacy Online: Start by reviewing app permissions on your phone – many apps request access to contacts, location, camera, or microphone unnecessarily. Disable ad personalisation in your Google/Apple account settings. Use a VPN on public Wi‑Fi to encrypt your internet traffic. On social media, set profiles to 'Friends Only' and avoid posting your birthdate, home address, or travel plans. Regularly clear your browser cookies and cache, or use private/incognito mode for sensitive searches. Check if your email has been exposed in a data breach using 'Have I Been Pwned' – if yes, change passwords immediately.",

                ["phishing"] = "Spotting Phishing Attacks: Phishing is a method where attackers impersonate legitimate organisations to steal your login credentials or personal data. Common signs: misspelled domain names (e.g., 'arnazon.com'), urgent language ('Your account will be suspended'), and suspicious attachments. Always hover over links before clicking to see the real URL. Never enter your password on a page you reached from an email link – type the official website address manually. Enable multi-factor authentication (MFA) – even if you fall for a phishing attempt, the attacker cannot log in without your second factor. Report phishing emails to the organisation being impersonated and to your email provider."
            };
        }

        // Checks if input is not null, empty, or whitespace.
        // Returns true if valid, false otherwise.
        public bool ValidateInput(string input) => !string.IsNullOrWhiteSpace(input);

        // Determines whether the input contains any of the keywords (password, scam, privacy, phishing).
        // Returns true if a keyword is found, false otherwise.
        public bool ContainsKeyword(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            string lower = input.ToLower();
            foreach (var topic in keywordResponses)
                if (lower.Contains(topic.Key)) return true;
            return false;
        }

        // Returns the detailed response for the first matching keyword found in the input.
        // If no keyword matches, returns a generic prompt asking the user to ask about cybersecurity topics.
        public string GetKeywordResponse(string input)
        {
            if (string.IsNullOrEmpty(input)) return "Please say something about cybersecurity.";
            string lower = input.ToLower();
            foreach (var topic in keywordResponses)
            {
                if (lower.Contains(topic.Key))
                    return topic.Value;
            }
            return "I'm here to help with passwords, scams, privacy, and phishing. Ask me about any of these topics!";
        }

        // Returns a generic follow-up message encouraging the user to ask for specific tips or say "tell me more".
        public string GetFollowUpResponse()
        {
            return "I can give you more details. Try asking for a tip on passwords, scams, privacy, or phishing, or say 'tell me more' after I answer a question.";
        }
    }
}