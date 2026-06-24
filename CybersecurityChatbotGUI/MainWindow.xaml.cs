// MainWindow – WPF GUI with Chat, Tasks, Quiz, and Activity Log tabs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Media;
using System.Collections.Generic;

namespace CybersecurityChatbotGUI
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine _chatbot;
        private TaskManager _taskManager;
        private QuizManager _quizManager;
        private ActivityLogger _logger;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AsciiArtBlock.Text = GetAsciiArt();

            _chatbot = new ChatbotEngine();
            _taskManager = new TaskManager();
            _quizManager = new QuizManager();
            _logger = new ActivityLogger();

            AddToConversation("Bot", "Hello! I'm your Cybersecurity Awareness Assistant.");
            AddToConversation("Bot", "I can help with: Chat, Tasks, Quiz, and Activity Log.");
            AddToConversation("Bot", "Type 'help' to see what I can do, or use the tabs above.");

            PlayVoiceGreeting();
            LoadTasks();
            UpdateActivityLog();
        }

        private string GetAsciiArt()
        {
            return @"
    ╔══════════════════════════════════════════════════════════╗
    ║                CYBERSECURITY AWARENESS                   ║
    ║                     PROTECT YOURSELF                     ║
    ║                    .--.   .--.                           ║
    ║                   /    \ /    \                          ║
    ║                  |    LOCK     |                         ║
    ║                  |    SAFE     |                         ║
    ║                   \          /                           ║
    ║                    `------'                              ║
    ║            'STAY SAFE, STAY SECURE ONLINE'               ║
    ╚══════════════════════════════════════════════════════════╝";
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (System.IO.File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Play();
                }
            }
            catch { /* ignore */ }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                AddToConversation("System", "Please type a message.");
                UserInputBox.Clear();
                return;
            }

            AddToConversation("You", userInput);

            string response = HandleSpecialCommands(userInput);
            if (string.IsNullOrEmpty(response))
            {
                response = _chatbot.GetResponse(userInput);
            }

            AddToConversation("Bot", response);
            _logger.AddLog($"User: {userInput} | Bot responded");

            UserInputBox.Clear();
            UserInputBox.Focus();
        }

        private string HandleSpecialCommands(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("add task") || lower.Contains("new task") || lower.Contains("create task"))
            {
                _logger.AddLog($"NLP command: '{input}'");
                return "I'll help you add a task. Go to the 'Task Assistant' tab and fill in the details.";
            }

            if (lower.Contains("view tasks") || lower.Contains("show tasks") || lower.Contains("list tasks"))
            {
                _logger.AddLog($"NLP command: '{input}'");
                return "Go to the 'Task Assistant' tab to view all your tasks.";
            }

            if (lower.Contains("quiz") || lower.Contains("game") || lower.Contains("test"))
            {
                _logger.AddLog($"NLP command: '{input}'");
                return "Go to the 'Cybersecurity Quiz' tab to test your knowledge!";
            }

            if (lower.Contains("activity log") || lower.Contains("what have you done") || lower.Contains("show log"))
            {
                _logger.AddLog($"NLP command: '{input}'");
                return _logger.GetLogSummary();
            }

            return "";
        }

        private void AddToConversation(string senderName, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");
            ConversationList.Items.Add($"[{timestamp}] {senderName}: {message}");
            if (ConversationList.Items.Count > 0)
                ConversationList.ScrollIntoView(ConversationList.Items[ConversationList.Items.Count - 1]);
        }

        // ====== TASK ASSISTANT METHODS ======

        private void LoadTasks()
        {
            try
            {
                var tasks = _taskManager.GetTasks();
                dgTasks.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}\nMake sure MySQL is running.");
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            string description = txtTaskDescription.Text.Trim();
            string reminder = txtReminder.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            try
            {
                _taskManager.AddTask(title, description, reminder);
                _logger.AddLog($"Task added: '{title}' with reminder: {reminder}");
                MessageBox.Show($"Task '{title}' added successfully!");

                txtTaskTitle.Clear();
                txtTaskDescription.Clear();
                txtReminder.Clear();
                LoadTasks();
                UpdateActivityLog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}");
            }
        }

        private void RefreshTasks_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int id = Convert.ToInt32(btn.Tag);

            try
            {
                _taskManager.CompleteTask(id);
                _logger.AddLog($"Task {id} marked as completed");
                LoadTasks();
                UpdateActivityLog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error completing task: {ex.Message}");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int id = Convert.ToInt32(btn.Tag);

            if (MessageBox.Show($"Delete task {id}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _taskManager.DeleteTask(id);
                    _logger.AddLog($"Task {id} deleted");
                    LoadTasks();
                    UpdateActivityLog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting task: {ex.Message}");
                }
            }
        }

        // ====== QUIZ METHODS ======

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.StartQuiz();
            _logger.AddLog($"Quiz started on {DateTime.Now}");

            var question = _quizManager.GetNextQuestion();
            if (question != null)
            {
                txtQuizQuestion.Text = question.Question;
                cmbQuizOptions.ItemsSource = question.Options;
                cmbQuizOptions.SelectedIndex = -1;
                txtQuizResult.Text = "";
                txtQuizProgress.Text = $"Question: 1/{_quizManager.GetTotalQuestions()}";
                _logger.AddLog($"Quiz question shown: {question.Question}");
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (!_quizManager.IsQuizActive())
            {
                txtQuizResult.Text = "Please start the quiz first!";
                return;
            }

            int selectedIndex = cmbQuizOptions.SelectedIndex;
            if (selectedIndex == -1)
            {
                txtQuizResult.Text = "Please select an answer first.";
                return;
            }

            string result = _quizManager.SubmitAnswer(selectedIndex);
            txtQuizResult.Text = result;
            _logger.AddLog($"Quiz answer submitted: {result}");

            var nextQuestion = _quizManager.GetNextQuestion();
            if (nextQuestion != null)
            {
                txtQuizQuestion.Text = nextQuestion.Question;
                cmbQuizOptions.ItemsSource = nextQuestion.Options;
                cmbQuizOptions.SelectedIndex = -1;
                txtQuizProgress.Text = $"Question: {_quizManager.GetProgress() + 1}/{_quizManager.GetTotalQuestions()}";
            }
            else if (!_quizManager.IsQuizActive())
            {
                txtQuizQuestion.Text = "Quiz completed! Click 'Start Quiz' to try again.";
                cmbQuizOptions.ItemsSource = null;
                _logger.AddLog($"Quiz completed. Final score: {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}");
                txtQuizProgress.Text = $"Score: {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}";
            }
        }

        // ====== ACTIVITY LOG METHODS ======

        private void UpdateActivityLog()
        {
            var logs = _logger.GetRecentLogs(10);
            lstActivityLog.Items.Clear();
            foreach (var log in logs)
            {
                lstActivityLog.Items.Add(log);
            }
        }

        private void RefreshLog_Click(object sender, RoutedEventArgs e)
        {
            UpdateActivityLog();
            _logger.AddLog("Activity log refreshed");
            MessageBox.Show("Activity log refreshed!");
        }
    }
}
// End of MainWindow – POE complete