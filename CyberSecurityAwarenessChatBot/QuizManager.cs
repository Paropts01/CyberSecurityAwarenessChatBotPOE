using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessChatBot
{
    public class QuizManager
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int score;
        private bool isQuizActive;

        public QuizManager()
        {
            InitializeQuestions();
            ResetQuiz();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                // ========== MULTIPLE CHOICE QUESTIONS ==========
                new QuizQuestion
                {
                    Question = "What is a strong password practice?",
                    Options = new List<string> { "Use the same password for all accounts", "Use 'password123'", "Use a mix of upper/lowercase, numbers, and symbols", "Never change your password" },
                    CorrectAnswerIndex = 2,
                    Explanation = "A strong password should be complex and unique for each account."
                },
                new QuizQuestion
                {
                    Question = "What is the best way to protect against phishing emails?",
                    Options = new List<string> { "Reply with your credentials", "Click all links", "Hover over links and check the sender's email", "Forward the email to friends" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Always verify links by hovering over them and checking the sender's email before clicking."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you suspect a scam?",
                    Options = new List<string> { "Ignore it", "Share it on social media", "Report it to authorities", "Send money to verify your identity" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Report suspicious activity to authorities and your bank immediately."
                },
                new QuizQuestion
                {
                    Question = "Is it safe to use public Wi-Fi for banking?",
                    Options = new List<string> { "Yes, it's always safe", "No, use a VPN or mobile data", "Only if you have antivirus software", "It depends on the location" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi is not secure for financial transactions. Use a VPN or mobile data instead."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "A type of password manager", "A second layer of security", "A way to delete passwords", "A browser extension" },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra security layer by requiring a second form of verification."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is a sign of a phishing attempt?",
                    Options = new List<string> { "Professional design and grammar", "Urgent language and misspellings", "Friendly tone", "Correct sender domain" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing emails often use urgent language, poor grammar, and misspelled domains."
                },
                new QuizQuestion
                {
                    Question = "How often should you change your passwords for critical accounts?",
                    Options = new List<string> { "Never", "Every 1-2 years", "Every 3-6 months", "When you're forced to" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Regularly changing passwords every 3-6 months helps maintain security."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you receive an unsolicited attachment in an email?",
                    Options = new List<string> { "Open it", "Forward it", "Delete it and report as spam", "Save it to your computer" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Never open unsolicited attachments; they may contain malware."
                },
                new QuizQuestion
                {
                    Question = "Is it safe to use a password manager?",
                    Options = new List<string> { "No, they are always compromised", "Yes, if using a reputable one", "Only if free", "They are the same as saving passwords in a browser" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Reputable password managers are safe and recommended for storing complex passwords securely."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "A type of computer virus", "Manipulating people into revealing information", "A programming language", "A type of hardware" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering exploits human psychology to gain access to sensitive information."
                },

                // ========== TRUE/FALSE QUESTIONS (added for variety) ==========
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1, // False
                    Explanation = "Reusing passwords is dangerous – if one account is breached, all others become vulnerable."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should always click on links in emails from your bank to verify your account.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1, // False
                    Explanation = "Never click links in unsolicited emails. Always type the bank's official URL directly into your browser."
                },
                new QuizQuestion
                {
                    Question = "True or False: Antivirus software is the only protection you need against cyber threats.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1, // False
                    Explanation = "Antivirus is important, but you also need strong passwords, 2FA, safe browsing habits, and regular updates."
                }
            };
        }

        public void ResetQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            isQuizActive = true;
            // Shuffle questions for variety
            Random rnd = new Random();
            questions = questions.OrderBy(q => rnd.Next()).ToList();
        }

        public bool IsQuizActive() => isQuizActive;

        public bool HasNextQuestion()
        {
            return currentQuestionIndex < questions.Count;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                return questions[currentQuestionIndex];
            }
            return null;
        }

        public bool SubmitAnswer(int answerIndex, out string feedback, out bool isCorrect)
        {
            var question = GetCurrentQuestion();
            if (question == null)
            {
                feedback = "No question available.";
                isCorrect = false;
                return false;
            }

            isCorrect = answerIndex == question.CorrectAnswerIndex;
            if (isCorrect)
            {
                score++;
                feedback = $"✅ Correct! {question.Explanation}";
            }
            else
            {
                feedback = $"❌ Incorrect. The correct answer was: {question.Options[question.CorrectAnswerIndex]}\n{question.Explanation}";
            }

            currentQuestionIndex++;
            return true;
        }

        public int GetScore() => score;

        public int GetTotalQuestions() => questions.Count;

        public string GetFinalMessage()
        {
            int total = GetTotalQuestions();
            int scorePercent = (score * 100) / total;

            if (scorePercent >= 80)
                return $" Great job! You scored {score}/{total} ({scorePercent}%)! You're a cybersecurity pro!";
            else if (scorePercent >= 60)
                return $" Good effort! You scored {score}/{total} ({scorePercent}%). Keep learning to become a cybersecurity expert!";
            else
                return $" Keep learning! You scored {score}/{total} ({scorePercent}%). Review the topics we've discussed to improve your knowledge!";
        }
    }

    public class QuizQuestion
    {
        public string? Question { get; set; }
        public List<string>? Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string? Explanation { get; set; }
    }
}
