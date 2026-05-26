// GUI for Cybersecurity Chatbot - Part 2
// Author: Phumudzo Masibigiri | ST10475290 | Part 2

using System;
using System.Windows;
using System.Windows.Controls;
using System.Media;

namespace CybersecurityChatbotGUI
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine _chatbot;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Display ASCII art
            AsciiArtBlock.Text = GetAsciiArt();

            _chatbot = new ChatbotEngine();
            AddToConversation("Bot", "Hello! I'm your Cybersecurity Awareness Assistant. What would you like to know?");
            PlayVoiceGreeting();
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
            string botResponse = _chatbot.GetResponse(userInput);
            AddToConversation("Bot", botResponse);

            UserInputBox.Clear();
            UserInputBox.Focus();
        }

        private void AddToConversation(string senderName, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");
            ConversationList.Items.Add($"[{timestamp}] {senderName}: {message}");
            if (ConversationList.Items.Count > 0)
                ConversationList.ScrollIntoView(ConversationList.Items[ConversationList.Items.Count - 1]);
        }
    }
}