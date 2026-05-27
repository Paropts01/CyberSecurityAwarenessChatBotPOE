using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityAwarenessChatBot
{
    public class RandomResponseManager
    {
        private readonly Dictionary<string, List<string>> _responsePool;

        public RandomResponseManager()
        {
            _responsePool = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use a strong password with at least 12 characters, including numbers, symbols, and both cases.",
                    "Never reuse passwords across different sites – use a password manager.",
                    "Enable two-factor authentication (2FA) wherever possible.",
                    "Avoid using personal info like your name or birthdate in passwords."
                },
                ["scam"] = new List<string>
                {
                    "If an offer sounds too good to be true, it probably is.",
                    "Never click on suspicious links – hover over them first.",
                    "Scammers often create urgency. Take time to think.",
                    "Report phishing attempts using official contact info."
                },
                ["privacy"] = new List<string>
                {
                    "Adjust your social media privacy settings.",
                    "Use a VPN on public Wi‑Fi to encrypt your data.",
                    "Regularly review app permissions on your phone.",
                    "Cover your webcam when not in use."
                },
                ["phishing"] = new List<string>
                {
                    "Check the sender's email address carefully.",
                    "Look for poor grammar or urgent demands.",
                    "Never enter login details after clicking a link in an email.",
                    "Use email filters and report suspicious messages."
                }
            };
        }

        public string GetRandomResponse(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return null;
            string key = topic.ToLower();
            if (_responsePool.TryGetValue(key, out var responses))
            {
                Random rng = new Random();
                return responses[rng.Next(responses.Count)];
            }
            return null;
        }

        public bool SupportsTopic(string topic) => _responsePool.ContainsKey(topic?.ToLower() ?? "");
    }
}

