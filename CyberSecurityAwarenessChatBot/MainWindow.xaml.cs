using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Timers;

namespace CyberSecurityAwarenessChatBot
{
   
    // Main window of the Cyber Security Awareness Chat Bot.
    // Handles UI events, message dispatching, task creation flow, and reminder checks.
    
    public partial class MainWindow : Window
    {
        // ------------------- Core chatbot components -------------------
        private Chatbot chatbot;
        private CyberSecurityChatBot cyberBot;
        private RandomResponseManager randomResponse;
        private ConversationManager conversationManager;
        private bool awaitingName = true;

        // ------------------- Task management -------------------
        private TaskManager? taskManager;
        private enum TaskCreationState { None, AwaitingTitle, AwaitingDescription, AwaitingReminder }
        private TaskCreationState _taskState = TaskCreationState.None;
        private string _pendingTaskTitle = "";
        private string _pendingTaskDescription = "";

        // Reminder timer
        private System.Timers.Timer? _reminderTimer;

        /// <summary>Initialises the main window, components, and starts the reminder timer.</summary>
        public MainWindow()
        {
            InitializeComponent();

            ChatDisplay.Document = new FlowDocument();
            ChatDisplay.Document.LineHeight = 1.2;

            chatbot = new Chatbot();
            cyberBot = new CyberSecurityChatBot();
            randomResponse = new RandomResponseManager();
            conversationManager = new ConversationManager();

            // Safely initialise TaskManager (may fail if MySQL is unavailable)
            try
            {
                taskManager = new TaskManager();
            }
            catch (Exception ex)
            {
                taskManager = null;
                MessageBox.Show(
                    $"Task Manager could not connect to the database.\n" +
                    $"Tasks and reminders will be unavailable.\n\nError: {ex.Message}",
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            AudioPlayer.PlayGreeting();
            DisplayWelcomeMessage();

            // Set up a timer to check for due reminders every minute
            if (taskManager != null)
            {
                _reminderTimer = new System.Timers.Timer(60000);
                _reminderTimer.Elapsed += CheckReminders;
                _reminderTimer.AutoReset = true;
                _reminderTimer.Start();
                CheckReminders(null, null); // immediate check on startup
            }
        }

        //Displays the initial welcome message and asks for the user's name.
        private async void DisplayWelcomeMessage()
        {
            await TypingStyle.TypeText(ChatDisplay, "Bot", "Hello! Welcome to the Cyber Security Awareness Chat Bot!", Brushes.DarkBlue);
            await TypingStyle.TypeText(ChatDisplay, "Bot", "What is your name?", Brushes.DarkBlue);
        }

        //Checks for tasks with reminders due today and displays them.
        private async void CheckReminders(object? sender, ElapsedEventArgs? e)
        {
            if (taskManager == null) return;

            var dueTasks = taskManager.GetTasksWithReminderToday();
            if (dueTasks.Count > 0)
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    string userName = chatbot.GetUserName() ?? "User";
                    foreach (var task in dueTasks)
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            $"🔔 Reminder, {userName}! Your task '{task.Title}' is due today! {(string.IsNullOrEmpty(task.Description) ? "" : $"Details: {task.Description}")}",
                            Brushes.DarkRed);
                    }
                });
            }
        }

        // ------------------- Send button and main message handling -------------------
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

                // Display the user's message in the chat
                await TypingStyle.TypeText(ChatDisplay, "You", input, Brushes.DarkSlateBlue);

                // ----- Exit command -----
                if (input.ToLower() == "exit" || input.ToLower() == "quit" || input.ToLower() == "goodbye")
                {
                    string userName = chatbot.GetUserName() ?? "friend";
                    await TypingStyle.TypeText(ChatDisplay, "Bot", $"Goodbye, {userName}! Stay safe online!", Brushes.DarkBlue);
                    await Task.Delay(1000);
                    Application.Current.Shutdown();
                    return;
                }

                // ----- First interaction: get name -----
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

                // ===== TASK CREATION STATE =====
                if (_taskState != TaskCreationState.None)
                {
                    await HandleTaskCreationFlow(input);
                    UserInput.Clear();
                    return;
                }

                string lowerInput = input.ToLower();

                // ----- "remind me to ..." or "set a reminder" -----
                if (lowerInput.Contains("remind me to") || lowerInput.Contains("set a reminder"))
                {
                    if (taskManager == null)
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            "Sorry, the task manager is unavailable because the database is not connected. Please check your MySQL server.",
                            Brushes.DarkBlue);
                        UserInput.Clear();
                        return;
                    }

                    string title = "";
                    if (lowerInput.Contains("remind me to"))
                    {
                        int idx = lowerInput.IndexOf("remind me to") + "remind me to".Length;
                        if (idx < input.Length)
                            title = input.Substring(idx).Trim();
                    }
                    else if (lowerInput.Contains("set a reminder"))
                    {
                        int idx = lowerInput.IndexOf("set a reminder") + "set a reminder".Length;
                        if (idx < input.Length)
                            title = input.Substring(idx).Trim();
                    }

                    if (!string.IsNullOrEmpty(title))
                    {
                        _pendingTaskTitle = title;
                        _taskState = TaskCreationState.AwaitingDescription;
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            $"I'll create a task titled '{title}'. Please provide a short description (or type 'skip').",
                            Brushes.DarkBlue);
                        UserInput.Clear();
                        return;
                    }
                    else
                    {
                        _taskState = TaskCreationState.AwaitingTitle;
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            "What task would you like to set a reminder for? Please enter a title.",
                            Brushes.DarkBlue);
                        UserInput.Clear();
                        return;
                    }
                }

                // ----- "add task", "new task", "create task" -----
                if (lowerInput.Contains("add task") || lowerInput.Contains("new task") || lowerInput.Contains("create task"))
                {
                    if (taskManager == null)
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            "Sorry, the task manager is unavailable because the database is not connected. Please check your MySQL server.",
                            Brushes.DarkBlue);
                        UserInput.Clear();
                        return;
                    }

                    int colonIdx = input.IndexOf(':');
                    if (colonIdx > 0 && colonIdx < input.Length - 1)
                    {
                        string title = input.Substring(colonIdx + 1).Trim();
                        if (!string.IsNullOrEmpty(title))
                        {
                            _pendingTaskTitle = title;
                            _taskState = TaskCreationState.AwaitingDescription;
                            await TypingStyle.TypeText(ChatDisplay, "Bot",
                                $"Got it! I'll create a task titled '{title}'. Please provide a short description (or type 'skip').",
                                Brushes.DarkBlue);
                            UserInput.Clear();
                            return;
                        }
                    }

                    _taskState = TaskCreationState.AwaitingTitle;
                    await TypingStyle.TypeText(ChatDisplay, "Bot",
                        "Let's add a cybersecurity task. What is the title of the task?",
                        Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ----- Quiz -----
                if (lowerInput.Contains("quiz") || lowerInput.Contains("start quiz") || lowerInput.Contains("take quiz"))
                {
                    await ShowTemporaryThinking(800);
                    string userName = chatbot.GetUserName() ?? "User";
                    var quizWindow = new QuizWindow(userName);
                    quizWindow.Owner = this;
                    quizWindow.ShowDialog();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", "Quiz completed! Keep learning to stay safe online!", Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ----- Activity Log -----
                if (lowerInput.Contains("activity log") || lowerInput.Contains("show log") || lowerInput.Contains("what have you done"))
                {
                    await ShowTemporaryThinking(1000);
                    string log = ActivityLogger.GetActivitySummary();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", log, Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ----- Help -----
                if (lowerInput.Contains("help") || lowerInput.Contains("what can you do") || lowerInput.Contains("commands"))
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

                // ----- Tip Detection -----
                if (lowerInput.Contains("tip"))
                {
                    await ShowTemporaryThinking(1200);
                    string detectedTopic = null;
                    if (lowerInput.Contains("password")) detectedTopic = "password";
                    else if (lowerInput.Contains("scam")) detectedTopic = "scam";
                    else if (lowerInput.Contains("privacy")) detectedTopic = "privacy";
                    else if (lowerInput.Contains("phishing")) detectedTopic = "phishing";

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

                // ----- Sentiment Detection -----
                string sentiment = SentimentDetector.DetectSentiment(input);
                if (!string.IsNullOrEmpty(sentiment))
                {
                    await ShowTemporaryThinking(1200);
                    string detectedTopic = null;
                    if (lowerInput.Contains("password")) detectedTopic = "password";
                    else if (lowerInput.Contains("scam")) detectedTopic = "scam";
                    else if (lowerInput.Contains("privacy")) detectedTopic = "privacy";
                    else if (lowerInput.Contains("phishing")) detectedTopic = "phishing";

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

                // ----- Follow-up -----
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

                // ----- Memory: Favourite Topic -----
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

                // ----- Keyword Recognition -----
                if (cyberBot.ContainsKeyword(input))
                {
                    await ShowTemporaryThinking(1200);
                    string response = cyberBot.GetKeywordResponse(input);
                    string prefix = GetFriendlyPrefix();
                    await TypingStyle.TypeText(ChatDisplay, "Bot", prefix + " " + response, Brushes.DarkBlue);
                    UserInput.Clear();
                    return;
                }

                // ----- Default Fallback -----
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

        // ===== TASK CREATION FLOW (multi‑step wizard) =====
        private async Task HandleTaskCreationFlow(string input)
        {
            if (taskManager == null)
            {
                await TypingStyle.TypeText(ChatDisplay, "Bot",
                    "Task manager is unavailable. Please check your database connection.",
                    Brushes.DarkBlue);
                _taskState = TaskCreationState.None;
                return;
            }

            switch (_taskState)
            {
                case TaskCreationState.AwaitingTitle:
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot", "Title cannot be empty. Please enter a title.", Brushes.DarkBlue);
                        return;
                    }
                    _pendingTaskTitle = input.Trim();
                    _taskState = TaskCreationState.AwaitingDescription;
                    await TypingStyle.TypeText(ChatDisplay, "Bot",
                        $"Task title set to '{_pendingTaskTitle}'. Now enter a description (or type 'skip').",
                        Brushes.DarkBlue);
                    break;

                case TaskCreationState.AwaitingDescription:
                    if (input.ToLower() == "skip")
                        _pendingTaskDescription = "";
                    else
                        _pendingTaskDescription = input.Trim();
                    _taskState = TaskCreationState.AwaitingReminder;
                    await TypingStyle.TypeText(ChatDisplay, "Bot",
                        "Would you like to set a reminder? Enter a date (e.g., '2026-07-01') or type 'no' to skip.",
                        Brushes.DarkBlue);
                    break;

                case TaskCreationState.AwaitingReminder:
                    string reminderDate = null;
                    if (input.ToLower() == "no")
                    {
                        reminderDate = null;
                    }
                    else if (DateTime.TryParse(input, out DateTime parsedDate))
                    {
                        reminderDate = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            "I didn't understand that date. Please enter a valid date (e.g., '2026-07-01') or type 'no'.",
                            Brushes.DarkBlue);
                        return;
                    }

                    int id = taskManager.AddTask(_pendingTaskTitle, _pendingTaskDescription, reminderDate);
                    if (id > 0)
                    {
                        ActivityLogger.AddActivity($"Task added via chat: '{_pendingTaskTitle}'" +
                            (reminderDate != null ? $" (Reminder: {reminderDate})" : ""));
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            $"✅ Task '{_pendingTaskTitle}' added successfully! {(reminderDate != null ? $"I'll remind you on {reminderDate}." : "")}",
                            Brushes.DarkBlue);
                    }
                    else
                    {
                        await TypingStyle.TypeText(ChatDisplay, "Bot",
                            "Sorry, I couldn't save the task. Please try again later.",
                            Brushes.DarkBlue);
                    }

                    _taskState = TaskCreationState.None;
                    _pendingTaskTitle = "";
                    _pendingTaskDescription = "";
                    break;
            }
        }

        // ------------------- UI Helpers -------------------

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, null);
                e.Handled = true;
            }
        }

        //Returns a friendly, randomised prefix to make bot responses more personal.
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

        //Shows a temporary "Thinking..." message for a short delay.
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

        // ------------------- Button Handlers -------------------

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            if (taskManager == null)
            {
                MessageBox.Show("Task Manager is unavailable because the database is not connected. Please check your MySQL server.",
                                "Database Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string userName = chatbot.GetUserName() ?? "User";
                TaskWindow taskWindow = new TaskWindow(userName);
                taskWindow.Owner = this;
                taskWindow.Show();
                taskWindow.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening TaskWindow:\n{ex.Message}\n\n{ex.StackTrace}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnQuiz_Click(object sender, RoutedEventArgs e)
        {
            string userName = chatbot.GetUserName() ?? "User";
            var quizWindow = new QuizWindow(userName);
            quizWindow.Owner = this;
            quizWindow.ShowDialog();
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            string log = ActivityLogger.GetActivitySummary();
            MessageBox.Show(log, "Activity Log", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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

        // Empty event handlers (kept to avoid XAML errors)
        private void ChatDisplay_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void ChatDisplay_TextChanged_1(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void btnQuiz(object sender, RoutedEventArgs e) { }

        //Clean up the reminder timer when the window closes.
        protected override void OnClosed(EventArgs e)
        {
            _reminderTimer?.Stop();
            _reminderTimer?.Dispose();
            base.OnClosed(e);
        }
    }
}
