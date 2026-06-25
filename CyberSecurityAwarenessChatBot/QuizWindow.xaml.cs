using System;
using System.Windows;
using System.Windows.Controls;

namespace CyberSecurityAwarenessChatBot
{
    public partial class QuizWindow : Window
    {
        private QuizManager quizManager;

        public QuizWindow()
        {
            InitializeComponent();
            quizManager = new QuizManager();
            ActivityLogger.AddActivity("Quiz started");
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (!quizManager.HasNextQuestion())
            {
                ShowQuizComplete();
                return;
            }

            var question = quizManager.GetCurrentQuestion();
            if (question == null)
            {
                ShowQuizComplete();
                return;
            }

            // Update progress
            txtProgress.Text = $"Question {quizManager.GetCurrentQuestionIndex() + 1} of {quizManager.GetTotalQuestions()}";
            txtScore.Text = $"Score: {quizManager.GetScore()}";

            // Display question
            txtQuestion.Text = question.Question;

            // Clear and create option buttons
            spOptions.Children.Clear();
            for (int i = 0; i < question.Options.Count; i++)
            {
                var btn = new Button
                {
                    Content = $"{(char)('A' + i)}. {question.Options[i]}",
                    Tag = i,
                    FontSize = 14,
                    FontWeight = FontWeights.Medium,
                    Background = System.Windows.Media.Brushes.LightGray,
                    Foreground = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(1),
                    BorderBrush = System.Windows.Media.Brushes.Gray,
                    Height = 35,
                    Margin = new Thickness(0, 3, 0, 3),
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Padding = new Thickness(10, 0, 0, 0),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                btn.Click += OptionButton_Click;
                spOptions.Children.Add(btn);
            }

            // Disable next button until answered
            btnNext.IsEnabled = false;
            txtFeedback.Text = "Select an option above to continue.";
            txtFeedback.Foreground = System.Windows.Media.Brushes.DarkSlateBlue;
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            int selectedIndex = (int)btn.Tag;

            // Submit answer and get feedback
            string feedback;
            bool isCorrect;
            quizManager.SubmitAnswer(selectedIndex, out feedback, out isCorrect);

            // Display feedback
            txtFeedback.Text = feedback;
            txtFeedback.Foreground = isCorrect ?
                System.Windows.Media.Brushes.Green :
                System.Windows.Media.Brushes.Red;

            // Update score
            txtScore.Text = $"Score: {quizManager.GetScore()}";

            // Disable all option buttons after answer
            foreach (var child in spOptions.Children)
            {
                if (child is Button optionBtn)
                {
                    optionBtn.IsEnabled = false;
                    // Color correct/incorrect answers
                    int optionIndex = (int)optionBtn.Tag;
                    if (optionIndex == quizManager.GetCurrentQuestionIndex() - 1)
                    {
                        // This was the selected answer
                        optionBtn.Background = isCorrect ?
                            System.Windows.Media.Brushes.LightGreen :
                            System.Windows.Media.Brushes.LightCoral;
                    }
                    else if (optionIndex == quizManager.GetCorrectAnswerIndex() && !isCorrect)
                    {
                        // Show the correct answer if user got it wrong
                        optionBtn.Background = System.Windows.Media.Brushes.LightGreen;
                    }
                }
            }

            // Enable next button
            btnNext.IsEnabled = true;
        }

        private void ShowQuizComplete()
        {
            txtQuestion.Text = "🎉 Quiz Complete!";
            spOptions.Children.Clear();

            string finalMessage = quizManager.GetFinalMessage();
            txtFeedback.Text = finalMessage;
            txtFeedback.Foreground = System.Windows.Media.Brushes.DarkSlateBlue;

            btnNext.IsEnabled = false;
            txtProgress.Text = $"Completed! Total Questions: {quizManager.GetTotalQuestions()}";
            txtScore.Text = $"Final Score: {quizManager.GetScore()}/{quizManager.GetTotalQuestions()}";

            // --- Activity Log: Quiz completed with score ---
            ActivityLogger.AddActivity($"Quiz completed - Score: {quizManager.GetScore()}/{quizManager.GetTotalQuestions()}");
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            DisplayQuestion();
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            quizManager.ResetQuiz();
            ActivityLogger.AddActivity("Quiz restarted");
            DisplayQuestion();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Extension methods for QuizManager
    public static class QuizManagerExtensions
    {
        public static int GetCurrentQuestionIndex(this QuizManager manager)
        {
            // Reflection to get current index - alternatively, expose a property
            var field = manager.GetType().GetField("currentQuestionIndex",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (int)field.GetValue(manager);
        }

        public static int GetCorrectAnswerIndex(this QuizManager manager)
        {
            var field = manager.GetType().GetField("questions",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var questions = field.GetValue(manager) as System.Collections.IList;
            if (questions != null)
            {
                var question = questions[GetCurrentQuestionIndex(manager) - 1];
                var prop = question.GetType().GetProperty("CorrectAnswerIndex");
                return (int)prop.GetValue(question);
            }
            return -1;
        }
    }
}
