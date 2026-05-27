using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityAwarenessChatBot
{
    // Manages a collection of random cybersecurity tips organized by topic.
    // Provides method to get a random response for a given topic.
    public class RandomResponseManager
    {
        // Dictionary that maps topic names (case-insensitive) to a list of response strings.
        // Each topic has multiple pre-written tips so the bot can give variety.
        private Dictionary<string, List<string>> topicResponses;

        // Constructor: Initializes the dictionary with four cybersecurity topics and multiple tips each.
        public RandomResponseManager()
        {
            // Use case-insensitive key comparison so "Password" and "password" work the same.
            topicResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                // Password safety tips: strong passwords, password managers, 2FA, etc.
                ["password"] = new List<string>
                {
                    " Password Safety: Use a unique, complex password for every account. Avoid dictionary words, names, or birthdates. A strong password contains at least 12 characters with uppercase, lowercase, numbers, and symbols. Consider using a passphrase like 'Correct-Horse-Battery-Staple' – it's long but memorable. Enable Two-Factor Authentication (2FA) whenever possible – this adds a second layer of protection even if your password is stolen.",

                    " Password Management: Never write down passwords or share them via email/text. Use a reputable password manager (e.g., Bitwarden, LastPass) to generate and store strong passwords. Change passwords immediately if you suspect a breach. Also, avoid reusing passwords across banking, social media, or work accounts – a single breach could compromise everything.",

                    " Extra Password Tips: Beware of phishing sites that mimic login pages – always check the URL. Set up account recovery options (phone/email) in case you forget your password. Regularly review saved passwords in your browser and remove old/unused ones. For critical accounts (email, bank), change passwords every 3–6 months."
                },

                // Scam detection tips: recognising scams, financial scams, phone scams.
                ["scam"] = new List<string>
                {
                    " Recognising Scams: Scammers often create urgency – 'Your account will be closed in 24 hours!' Never click links in unsolicited emails or SMS. Verify by calling the official company number (not the one in the message). Common red flags: poor grammar, generic greetings ('Dear Customer'), requests for gift cards or wire transfers.",

                    " Financial Scams: Be wary of 'too good to be true' investments, lottery winnings, or overpayment refunds. Never send money to someone you've only met online – romance and crypto scams are rising. Legitimate companies will never demand payment via iTunes cards, Bitcoin, or Western Union. Report scams to your local authorities and the FTC.",

                    " Phone & Tech Support Scams: If someone calls claiming to be from Microsoft, your bank, or the police – hang up. Call back using a verified number from a statement or official website. Do not allow remote access to your computer unless you initiated contact with a trusted company. Install ad-blockers to avoid fake 'Your PC is infected' pop-ups."
                },

                // Privacy protection tips: app permissions, social media settings, data breaches.
                ["privacy"] = new List<string>
                {
                    " Protecting Your Privacy: Review app permissions on your phone – many request access to contacts, location, or microphone unnecessarily. Disable ad tracking in your device settings. Use a VPN on public Wi‑Fi to encrypt your traffic. Regularly clear cookies and browser history, or use private browsing mode.",

                    " Social Media Privacy: Set profiles to 'Friends Only' and avoid posting your birthdate, address, or travel plans. Disable geotagging on photos. Be cautious about online quizzes – they often collect your data. Use different email addresses for different services (e.g., one for shopping, one for banking).",

                    "Data Breaches: Check if your email has been compromised using 'Have I Been Pwned'. If yes, change passwords immediately. Enable login alerts from your email and social media providers. Consider using email aliases (like 'yourname+service@gmail.com') to track which services sell your data. Use encrypted messaging apps (Signal, WhatsApp) for sensitive conversations."
                },

                // Phishing tips: email phishing, fake websites, smishing/vishing.
                ["phishing"] = new List<string>
                {
                    " Spotting Phishing Emails: Hover over links before clicking – the actual URL often differs from displayed text. Look for misspellings like 'Amaz0n' or 'PayPaI'. Phishing emails often create fear ('Your account is suspended') or greed ('You won a prize'). Never open attachments from unknown senders – they may contain malware.",

                    " Fake Websites: Always check the address bar for 'https://' and a padlock icon – but even these can be faked. Type the website address manually instead of clicking a link in an email. Be extra cautious with shortened URLs (bit.ly, tinyurl) – use a preview tool first. If a deal seems too good to be true (e.g., 90% off), it's likely a phishing trap.",

                    " Smishing & Vishing: SMS phishing ('smishing') uses fake bank alerts or delivery notifications. Never reply with personal info or click links in texts. Vishing (voice phishing) uses automated calls claiming to be from the IRS or your bank – hang up and call back using a verified number. Enable your carrier's spam protection."
                }
            };
        }

        // Checks if a given topic exists in the dictionary (case-insensitive).
        // Returns true if the topic is supported (password, scam, privacy, phishing).
        public bool SupportsTopic(string topic)
        {
            return topicResponses.ContainsKey(topic);
        }

        // Returns a random tip for the requested topic.
        // If the topic is not supported, returns a fallback message asking user to try supported topics.
        public string GetRandomResponse(string topic)
        {
            if (!SupportsTopic(topic))
                return $"I don't have detailed information about '{topic}' yet. Try asking about passwords, scams, privacy, or phishing.";

            // Select a random index from the list of responses for that topic.
            Random rand = new Random();
            int index = rand.Next(topicResponses[topic].Count);
            return topicResponses[topic][index];
        }
    }
}

