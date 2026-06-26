using System;
using System.Windows;
using System.Windows.Controls;

namespace CyberSecurityAwarenessChatBot
{
    public partial class QuizWindow : Window
    {
        private QuizManager quizManager;
        private string _userName;

        public QuizWindow(string userName)
        {
            InitializeComponent();
            _userName = userName;
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

            txtProgress.Text = $"Question {quizManager.GetCurrentQuestionIndex() + 1} of {quizManager.GetTotalQuestions()}";
            txtScore.Text = $"Score: {quizManager.GetScore()}";
            txtQuestion.Text = question.Question;

            spOptions.Children.Clear();
            for (int i = 0; i < question.Options.Count; i++)
            {
                var btn = new Button
                {
                    Content = $"{(char)('A' + i)}. {question.Options[i]}",
                    Tag = i,
                    FontSize = 16,
                    FontWeight = FontWeights.Medium,
                    Background = System.Windows.Media.Brushes.LightGray,
                    Foreground = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(1),
                    BorderBrush = System.Windows.Media.Brushes.Gray,
                    Height = 40,
                    Margin = new Thickness(0, 5, 0, 5),
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Padding = new Thickness(15, 0, 0, 0),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                btn.Click += OptionButton_Click;
                spOptions.Children.Add(btn);
            }

            btnNext.IsEnabled = false;
            txtFeedback.Text = "Select an option above to continue.";
            txtFeedback.Foreground = System.Windows.Media.Brushes.DarkSlateBlue;
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            int selectedIndex = (int)btn.Tag;
            string feedback;
            bool isCorrect;
            quizManager.SubmitAnswer(selectedIndex, out feedback, out isCorrect);

            txtFeedback.Text = feedback;
            txtFeedback.Foreground = isCorrect ?
                System.Windows.Media.Brushes.Green :
                System.Windows.Media.Brushes.Red;

            txtScore.Text = $"Score: {quizManager.GetScore()}";

            foreach (var child in spOptions.Children)
            {
                if (child is Button optionBtn)
                {
                    optionBtn.IsEnabled = false;
                    int optionIndex = (int)optionBtn.Tag;
                    if (optionIndex == quizManager.GetCurrentQuestionIndex() - 1)
                        optionBtn.Background = isCorrect ?
                            System.Windows.Media.Brushes.LightGreen :
                            System.Windows.Media.Brushes.LightCoral;
                    else if (optionIndex == quizManager.GetCorrectAnswerIndex() && !isCorrect)
                        optionBtn.Background = System.Windows.Media.Brushes.LightGreen;
                }
            }

            btnNext.IsEnabled = true;
        }

        private void ShowQuizComplete()
        {
            // ===== PLAY THE CLAPPING SOUND =====
            AudioPlayer.PlayClap();

            txtQuestion.Text = "🎉 Quiz Complete!";
            spOptions.Children.Clear();

            string finalMessage = quizManager.GetFinalMessage();
            string personalizedMessage = $"Well done, {_userName}!\n{finalMessage}";
            txtFeedback.Text = personalizedMessage;
            txtFeedback.Foreground = System.Windows.Media.Brushes.DarkSlateBlue;

            btnNext.IsEnabled = false;
            txtProgress.Text = $"Completed! Total Questions: {quizManager.GetTotalQuestions()}";
            txtScore.Text = $"Final Score: {quizManager.GetScore()}/{quizManager.GetTotalQuestions()}";

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

    // Extension methods (unchanged)
    public static class QuizManagerExtensions
    {
        public static int GetCurrentQuestionIndex(this QuizManager manager)
        {
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
