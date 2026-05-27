using CyberSecurityAwarenessChatBot;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CyberSecurityAwarenessChatBot
{
    public partial class MainWindow : Window
    {
        private Chatbot chatbot;
        private CyberSecurityChatBot cyberBot;
        private RandomResponseManager randomResponse;
        private ConversationManager conversationManager;
        private bool awaitingName = true;

        public MainWindow()
        {
            InitializeComponent();
            chatbot = new Chatbot();
            cyberBot = new CyberSecurityChatBot();
            randomResponse = new RandomResponseManager();
            conversationManager = new ConversationManager();
            AudioPlayer.PlayGreeting();   // plays Welcome.wav
            DisplayWelcomeMessage();
        }

        private async void DisplayWelcomeMessage()
        {
            await TypingStyle.TypeText(ChatDisplay, "Bot", "Hello! I'm your Cybersecurity Assistant.", Brushes.DarkBlue);
            await TypingStyle.TypeText(ChatDisplay, "Bot", "What's your name?", Brushes.DarkBlue);
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = UserInput.Text.Trim();
                if (!cyberBot.ValidateInput(input))
                {
                    MessageBox.Show("Please enter a message.", "Input Error");
                    return;
                }

                // Show user message (DarkGoldenrod)
                await TypingStyle.TypeText(ChatDisplay, "You", input, Brushes.DarkGoldenrod);

                // Exit command
                if (input.ToLower() == "exit" || input.ToLower() == "quit" || input.ToLower() == "goodbye")
                {
                    string userName = chatbot.GetUserName() ?? "friend";
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Goodbye, {userName}! Stay safe online!", Brushes.DarkBlue);
                    await Task.Delay(1000);
                    Application.Current.Shutdown();
                    return;
                }

                // First: get user name
                if (awaitingName)
                {
                    chatbot.SetUserName(input);
                    await ShowTemporaryThinking(1500);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Nice to meet you, {chatbot.GetUserName()}!", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Thanks, {chatbot.GetUserName()}! Here are the cybersecurity topics I can help you with:", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "- Password Safety", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "- Privacy Protection", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "- Scam Detection", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "- Phishing Tips", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "What would you like to talk about today? (Type 'exit' to quit)", Brushes.DarkBlue);
                    awaitingName = false;
                    UserInput.Clear();
                    return;
                }

                // Sentiment detection (empathethic)
                string sentiment = SentimentDetector.DetectSentiment(input);
                if (!string.IsNullOrEmpty(sentiment))
                {
                    await ShowTemporaryThinking(1200);
                    string detectedTopic = null;
                    string lower = input.ToLower();
                    if (lower.Contains("password")) detectedTopic = "password";
                    else if (lower.Contains("scam")) detectedTopic = "scam";
                    else if (lower.Contains("privacy")) detectedTopic = "privacy";
                    else if (lower.Contains("phishing")) detectedTopic = "phishing";

                    string empathyMsg = SentimentDetector.GetSentimentResponse(sentiment, detectedTopic);
                    if (!string.IsNullOrEmpty(empathyMsg))
                        await TypingStyle.TypeText(ChatDisplay, "Bot", empathyMsg, Brushes.DarkBlue);

                    if (detectedTopic != null && randomResponse.SupportsTopic(detectedTopic))
                    {
                        string tip = randomResponse.GetRandomResponse(detectedTopic);
                        string prefix = GetFriendlyPrefix();
                        await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + tip, Brushes.DarkBlue);
                        conversationManager.SetCurrentTopic(detectedTopic);
                    }
                    UserInput.Clear();
                    return;
                }

                // Follow-up (conversation flow)
                if (conversationManager.IsFollowUpRequest(input))
                {
                    await ShowTemporaryThinking(1200);
                    string followUp = conversationManager.GetFollowUpResponse(randomResponse);
                    if (string.IsNullOrEmpty(followUp))
                        followUp = "What topic would you like to learn more about? (Password, Scam, Privacy, Phishing)";
                    string prefix = GetFriendlyPrefix();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + followUp, Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // Memory: set favourite topic
                string lowerInput = input.ToLower();
                if ((lowerInput.Contains("interested in") || lowerInput.Contains("i like") || lowerInput.Contains("my favourite")) &&
                    (lowerInput.Contains("password") || lowerInput.Contains("privacy") || lowerInput.Contains("scam") || lowerInput.Contains("phishing")))
                {
                    string newFav = null;
                    if (lowerInput.Contains("password")) newFav = "password";
                    else if (lowerInput.Contains("privacy")) newFav = "privacy";
                    else if (lowerInput.Contains("scam")) newFav = "scam";
                    else if (lowerInput.Contains("phishing")) newFav = "phishing";

                    if (newFav != null)
                    {
                        chatbot.SetFavouriteTopic(newFav);
                        await ShowTemporaryThinking(1000);
                        await TypingStyle.TypeText(ChatDisplay, "Bot", $"Great, {chatbot.GetUserName()}! I'll remember that you're interested in {newFav}. It's a crucial part of staying safe online.", Brushes.DarkBlue);
                        UserInput.Clear();
                        return;
                    }
                }

                // Keyword recognition with random responses
                if (cyberBot.ContainsKeyword(input))
                {
                    await ShowTemporaryThinking(1200);
                    string matchedTopic = null;
                    string lower = input.ToLower();
                    if (lower.Contains("password")) matchedTopic = "password";
                    else if (lower.Contains("scam")) matchedTopic = "scam";
                    else if (lower.Contains("privacy")) matchedTopic = "privacy";
                    else if (lower.Contains("phishing")) matchedTopic = "phishing";

                    if (matchedTopic != null && randomResponse.SupportsTopic(matchedTopic))
                    {
                        string prefix = GetFriendlyPrefix();
                        string tip = randomResponse.GetRandomResponse(matchedTopic);
                        await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + tip, Brushes.DarkBlue);
                        conversationManager.SetCurrentTopic(matchedTopic);

                        // Recall favourite topic occasionally
                        string fav = chatbot.GetFavouriteTopic();
                        if (!string.IsNullOrEmpty(fav) && fav == matchedTopic && new Random().Next(0, 3) == 0)
                        {
                            await Task.Delay(500);
                            string recallPrefix = GetFriendlyPrefix();
                            string recallTip = randomResponse.GetRandomResponse(fav);
                            await TypingStyle.TypeText(ChatDisplay, "Bot", recallPrefix + " As someone interested in " + fav + ", you might also like: " + recallTip, Brushes.DarkBlue);
                        }
                    }
                    else
                    {
                        string prefix = GetFriendlyPrefix();
                        string response = cyberBot.GetKeywordResponse(input);
                        await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + response, Brushes.DarkBlue);
                    }
                    UserInput.Clear();
                    return;
                }

                // Default / unknown input
                await ShowTemporaryThinking(1000);
                string name = chatbot.GetUserName() ?? "friend";
                string defaultMsg = $"I'm not sure I understand, {name}. Could you try rephrasing? Ask about passwords, scams, privacy, or phishing.";
                await TypingStyle.TypeText(ChatDisplay, "Bot", defaultMsg, Brushes.DarkBlue);
                UserInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Press Enter to send
        private void UserInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                SendButton_Click(sender, null);
        }

        // Friendly personalised prefix using user's name
        private string GetFriendlyPrefix()
        {
            string name = chatbot.GetUserName();
            if (string.IsNullOrEmpty(name)) name = "friend";

            string[] prefixes = new[]
            {
                $"That's a great question, {name}!",
                $"I like your curiosity, {name}.",
                $"Good thinking, {name}!",
                $"Thanks for asking, {name}.",
                $"Here's something useful for you, {name}:",
                $"You're on the right track, {name}!",
                $"Great question, {name} – let me help.",
                $"I appreciate you learning about cybersecurity, {name}."
            };
            Random rnd = new Random();
            return prefixes[rnd.Next(prefixes.Length)];
        }

        // Temporary "Thinking..." message that disappears
        private async Task ShowTemporaryThinking(int delayMs = 1500)
        {
            Paragraph tempParagraph = new Paragraph();
            Run tempRun = new Run("System: Thinking...");
            tempRun.Foreground = Brushes.Orange;
            tempRun.FontWeight = FontWeights.Bold;
            tempParagraph.Inlines.Add(tempRun);
            ChatDisplay.Document.Blocks.Add(tempParagraph);
            ChatDisplay.ScrollToEnd();
            await Task.Delay(delayMs);
            if (ChatDisplay.Document.Blocks.Contains(tempParagraph))
                ChatDisplay.Document.Blocks.Remove(tempParagraph);
        }
    }
}