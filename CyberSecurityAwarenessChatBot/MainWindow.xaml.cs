// MainWindow.xaml.cs - Code-behind for the CyberSecurity Chatbot UI
// Handles user input, chatbot responses, and UI interactions

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberSecurityAwarenessChatBot
{
    public partial class MainWindow : Window
    {
        // Private fields for the chatbot's core components
        private Chatbot chatbot;                       // Manages user name and favorite topic
        private CyberSecurityChatBot cyberBot;         // Provides keyword-based cybersecurity responses
        private RandomResponseManager randomResponse;  // Gives random tips on specific topics
        private ConversationManager conversationManager; // Tracks current topic and follow-up requests
        private bool awaitingName = true;              // Flag to indicate if we're still waiting for user's name

        // Constructor: initializes components and sets up the chat
        public MainWindow()
        {
            InitializeComponent();

            // Set up the chat display document with comfortable line height
            ChatDisplay.Document = new FlowDocument();
            ChatDisplay.Document.LineHeight = 1.2;

            // Instantiate helper classes
            chatbot = new Chatbot();
            cyberBot = new CyberSecurityChatBot();
            randomResponse = new RandomResponseManager();
            conversationManager = new ConversationManager();

            // Play a greeting sound and display welcome message
            AudioPlayer.PlayGreeting();
            DisplayWelcomeMessage();
        }

        // Displays the initial welcome message with typing animation effect
        private async void DisplayWelcomeMessage()
        {
            await TypingStyle.TypeText(ChatDisplay, "Bot", "Hello! Welcome to the Cyber Security Awareness Chat Bot!", Brushes.DarkBlue);
            await TypingStyle.TypeText(ChatDisplay, "Bot", "What is your name?", Brushes.DarkBlue);
        }

        // Handles the Send button click and Enter key press (via UserInput_KeyDown)
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get and validate user input
                string input = UserInput.Text.Trim();
                if (!cyberBot.ValidateInput(input))
                {
                    MessageBox.Show("Please enter a message.", "Input Error");
                    return;
                }

                // Display user's message in the chat
                await TypingStyle.TypeText(ChatDisplay, "You", input, Brushes.DarkSlateBlue);

                // Exit command handling
                if (input.ToLower() == "exit" || input.ToLower() == "quit" || input.ToLower() == "goodbye")
                {
                    string userName = chatbot.GetUserName() ?? "friend";
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Goodbye, {userName}! Stay safe online!", Brushes.DarkBlue);
                    await Task.Delay(1000);
                    Application.Current.Shutdown();
                    return;
                }

                // First interaction: get and store user's name
                if (awaitingName)
                {
                    chatbot.SetUserName(input);
                    await ShowTemporaryThinking(1500);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Nice to meet you, {chatbot.GetUserName()}!", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Here are the cybersecurity topics I can help you with:\n- Password Safety\n- Privacy Protection\n- Scam Detection\n- Phishing Tips", Brushes.DarkBlue);
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "What would you like to talk about today? (Type 'exit' to quit)", Brushes.DarkBlue);
                    awaitingName = false;
                    UserInput.Clear();
                    return;
                }

                // ========== NEW PART 3 NLP COMMANDS (HIGHEST PRIORITY) ==========
                // These commands are checked before any cybersecurity-topic logic

                // TASK COMMANDS
                if (input.ToLower().Contains("add task") ||
                    input.ToLower().Contains("new task") ||
                    input.ToLower().Contains("create task"))
                {
                    await ShowTemporaryThinking(800);
                    var taskWindow = new TaskWindow();
                    taskWindow.Owner = this;
                    taskWindow.ShowDialog();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "Task window opened. You can manage your cybersecurity tasks there.", Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // QUIZ COMMANDS
                if (input.ToLower().Contains("quiz") ||
                    input.ToLower().Contains("start quiz") ||
                    input.ToLower().Contains("take quiz"))
                {
                    await ShowTemporaryThinking(800);
                    var quizWindow = new QuizWindow();
                    quizWindow.Owner = this;
                    quizWindow.ShowDialog();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "Quiz completed! Keep learning to stay safe online!", Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ACTIVITY LOG COMMANDS
                if (input.ToLower().Contains("activity log") ||
                    input.ToLower().Contains("show log") ||
                    input.ToLower().Contains("what have you done"))
                {
                    await ShowTemporaryThinking(1000);
                    string log = ActivityLogger.GetActivitySummary();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", log, Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // HELP COMMANDS
                if (input.ToLower().Contains("help") ||
                    input.ToLower().Contains("what can you do") ||
                    input.ToLower().Contains("commands"))
                {
                    await ShowTemporaryThinking(800);
                    string help = @"Here's what I can help you with:

🔹 Topic Advice: Ask about passwords, scams, privacy, or phishing.
🔹 Tips: Say 'give me a [topic] tip'.
🔹 Tasks: Say 'add task' to manage your cybersecurity tasks.
🔹 Quiz: Say 'start quiz' to test your knowledge.
🔹 Activity Log: Say 'show log' to see recent actions.
🔹 Follow-ups: Say 'tell me more' for additional information.

What would you like to do?";
                    await TypingStyle.TypeText(ChatDisplay, "Bot", help, Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ========== 1. TIP DETECTION ==========
                // If user asks for a "tip", extract the topic and give a random response
                if (input.ToLower().Contains("tip"))
                {
                    await ShowTemporaryThinking(1200);
                    string detectedTopic = null;
                    string lower = input.ToLower();
                    if (lower.Contains("password")) detectedTopic = "password";
                    else if (lower.Contains("scam")) detectedTopic = "scam";
                    else if (lower.Contains("privacy")) detectedTopic = "privacy";
                    else if (lower.Contains("phishing")) detectedTopic = "phishing";

                    if (detectedTopic != null && randomResponse.SupportsTopic(detectedTopic))
                    {
                        string tip = randomResponse.GetRandomResponse(detectedTopic);
                        string prefix = GetFriendlyPrefix();
                        await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + tip, Brushes.DarkBlue);
                        conversationManager.SetCurrentTopic(detectedTopic);
                        UserInput.Clear();
                        return;
                    }
                    else
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot", "Please mention a topic with your tip request, e.g., 'give me a password tip'.", Brushes.DarkBlue);
                        UserInput.Clear();
                        return;
                    }
                }

                // ========== 2. SENTIMENT DETECTION ==========
                // Detect emotional tone and respond with empathy, then optionally give a tip
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

                // ========== 3. FOLLOW-UP REQUEST ==========
                // If user asks "tell me more", give a different tip on the same topic
                if (conversationManager.IsFollowUpRequest(input))
                {
                    await ShowTemporaryThinking(1200);
                    string currentTopic = conversationManager.GetCurrentTopic();
                    if (!string.IsNullOrEmpty(currentTopic) && randomResponse.SupportsTopic(currentTopic))
                    {
                        string newTip = randomResponse.GetRandomResponse(currentTopic);
                        string prefix = GetFriendlyPrefix();
                        await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " Here's another tip: " + newTip, Brushes.DarkBlue);
                    }
                    else
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot", "What topic would you like to learn more about? (Password, Scam, Privacy, Phishing)", Brushes.DarkBlue);
                    }
                    UserInput.Clear();
                    return;
                }

                // ========== 4. MEMORY: USER'S FAVORITE TOPIC ==========
                // If user expresses interest in a topic, remember it for later
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

                // ========== 5. KEYWORD RECOGNITION ==========
                // Provide detailed answers for cybersecurity keywords (e.g., "password", "phishing")
                if (cyberBot.ContainsKeyword(input))
                {
                    await ShowTemporaryThinking(1200);
                    string response = cyberBot.GetKeywordResponse(input);
                    string prefix = GetFriendlyPrefix();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + response, Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ========== 6. DEFAULT FALLBACK ==========
                // When no pattern matches, ask user to rephrase
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

        // Allows user to press Enter to send message instead of clicking Send button
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, null);
                e.Handled = true;   // Suppress default beep sound
            }
        }

        // Returns a random friendly prefix using the user's name for personalization
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

        // Displays a temporary "Thinking..." message while processing, then removes it after delay
        private async Task ShowTemporaryThinking(int delayMs = 1500)
        {
            Paragraph tempParagraph = new Paragraph();
            Run tempRun = new Run("System: Thinking...");
            tempRun.Foreground = Brushes.DarkSlateBlue;
            tempRun.FontWeight = FontWeights.Bold;
            tempParagraph.Inlines.Add(tempRun);
            ChatDisplay.Document.Blocks.Add(tempParagraph);
            ChatDisplay.ScrollToEnd();
            await Task.Delay(delayMs);
            if (ChatDisplay.Document.Blocks.Contains(tempParagraph))
                ChatDisplay.Document.Blocks.Remove(tempParagraph);
        }

        // ========== NEW PART 3 BUTTON CLICK HANDLERS ==========

        /// <summary>
        /// Opens the Task Assistant window.
        /// </summary>
        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskWindow taskWindow = new TaskWindow();
                taskWindow.Owner = this;   // set owner to main window
                taskWindow.Show();
                taskWindow.Activate();     // bring to front
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening TaskWindow:\n{ex.Message}\n\n{ex.StackTrace}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Opens the Cybersecurity Quiz window.
        /// </summary>
        private void btnQuiz_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new QuizWindow();
            quizWindow.Owner = this;
            quizWindow.ShowDialog();
        }

        /// <summary>
        /// Displays the Activity Log in a message box.
        /// </summary>
        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            string log = ActivityLogger.GetActivitySummary();
            MessageBox.Show(log, "Activity Log", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Displays a help menu with all available commands.
        /// </summary>
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string help = @"🎯 CyberSecurity ChatBot Help

Commands you can use:
• Type 'task' or 'add task' - Open Task Assistant
• Type 'quiz' or 'start quiz' - Start Cybersecurity Quiz
• Type 'log' or 'activity log' - View recent activities
• Type 'help' - Show this help message
• Type 'exit' - Close the application

You can also:
• Ask about cybersecurity topics (password, scam, privacy, phishing)
• Say 'tell me more' for follow-up info
• Share your feelings (worried, curious, frustrated)

Stay safe online! 🛡️";
            MessageBox.Show(help, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // These empty methods are referenced in the XAML but not used.
        private void ChatDisplay_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Intentionally empty
        }

        private void ChatDisplay_TextChanged_1(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Intentionally empty
        }

        private void btnQuiz(object sender, RoutedEventArgs e)
        {

        }
    }
}