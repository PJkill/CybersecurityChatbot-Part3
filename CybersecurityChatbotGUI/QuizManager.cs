using System;
using System.Collections.Generic;

namespace CybersecurityChatbotGUI
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private bool _quizActive = false;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others."
                },
                new QuizQuestion
                {
                    Question = "A strong password should contain:",
                    Options = new List<string> { "Only numbers", "Only letters", "A mix of letters, numbers, and symbols", "Your birthdate" },
                    CorrectIndex = 2,
                    Explanation = "A mix of characters makes passwords harder to crack."
                },
                new QuizQuestion
                {
                    Question = "True or False: It's safe to use the same password for all accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Using unique passwords for each account prevents a single breach from compromising everything."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A type of fishing", "A scam to steal personal information", "A computer virus", "A social media platform" },
                    CorrectIndex = 1,
                    Explanation = "Phishing is a scam where attackers trick you into giving away personal information."
                },
                new QuizQuestion
                {
                    Question = "What does 2FA stand for?",
                    Options = new List<string> { "Two Factor Authentication", "Two Form Access", "Temporary File Access", "Total Firewall Access" },
                    CorrectIndex = 0,
                    Explanation = "2FA adds an extra layer of security by requiring a second form of verification."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public WiFi is always safe to use for banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Public WiFi is not secure. Avoid entering sensitive information on public networks."
                },
                new QuizQuestion
                {
                    Question = "What is a strong password practice?",
                    Options = new List<string> { "Using your name", "Using 'password123'", "Using a mix of characters and a password manager", "Writing it on a sticky note" },
                    CorrectIndex = 2,
                    Explanation = "Use unique, complex passwords and a password manager for security."
                },
                new QuizQuestion
                {
                    Question = "What is ransomware?",
                    Options = new List<string> { "A type of virus that locks your files", "A password manager", "A social media platform", "A bank security feature" },
                    CorrectIndex = 0,
                    Explanation = "Ransomware locks your files and demands payment to unlock them."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should share your password with close friends.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Never share passwords with anyone. Use a password manager for shared access."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "Building social networks", "Manipulating people to gain confidential information", "Engineering social media", "A type of computer virus" },
                    CorrectIndex = 1,
                    Explanation = "Social engineering is a manipulation technique used to trick people into giving up confidential information."
                },
                new QuizQuestion
                {
                    Question = "True or False: Anti-virus software is unnecessary if you use a firewall.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Anti-virus and firewall protect against different threats. Use both for maximum protection."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you think you've been hacked?",
                    Options = new List<string> { "Ignore it", "Change passwords immediately", "Wait and see", "Tell everyone" },
                    CorrectIndex = 1,
                    Explanation = "Immediately change passwords, run antivirus, and contact relevant authorities."
                },
                new QuizQuestion
                {
                    Question = "True or False: Strong passwords should be at least 12 characters long.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 0,
                    Explanation = "Longer passwords are more secure. Aim for at least 12 characters."
                },
                new QuizQuestion
                {
                    Question = "What does SSL/TLS protect?",
                    Options = new List<string> { "Email attachments", "Data in transit", "Password storage", "Social media" },
                    CorrectIndex = 1,
                    Explanation = "SSL/TLS protects data as it travels between your device and the website."
                },
                new QuizQuestion
                {
                    Question = "True or False: Free Wi-Fi in cafes is secure for banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Public Wi-Fi is not secure. Avoid banking or entering sensitive information on public networks."
                }
            };
        }

        public QuizQuestion GetNextQuestion()
        {
            if (!_quizActive || _currentQuestionIndex >= _questions.Count)
                return null;
            return _questions[_currentQuestionIndex];
        }

        public string SubmitAnswer(int selectedIndex)
        {
            var q = _questions[_currentQuestionIndex];
            bool correct = selectedIndex == q.CorrectIndex;
            if (correct) _score++;

            string result = correct ? "✅ Correct!" : $"❌ Incorrect. The answer was: {q.Options[q.CorrectIndex]}";
            result += $"\n{q.Explanation}";

            _currentQuestionIndex++;

            if (_currentQuestionIndex >= _questions.Count)
            {
                _quizActive = false;
                double percentage = (double)_score / _questions.Count * 100;
                result += $"\n\n🏆 Quiz complete! Your score: {_score}/{_questions.Count} ({percentage:F1}%)";
                if (percentage >= 80)
                    result += "\n🌟 Great job! You're a cybersecurity pro!";
                else if (percentage >= 50)
                    result += "\n📚 Good effort! Keep learning to stay safe online!";
                else
                    result += "\n📖 Keep studying! Cybersecurity knowledge is important for staying safe online.";
            }

            return result;
        }

        public void StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            _quizActive = true;
        }

        public bool IsQuizActive() => _quizActive;
        public int GetProgress() => _currentQuestionIndex;
        public int GetTotalQuestions() => _questions.Count;
        public int GetScore() => _score;
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
    }
}