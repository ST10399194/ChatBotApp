using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CybersecurityChatbot
{
    
    public partial class MainWindow : Window
    {
        // The chatbot that owns all conversational logic
        private readonly ChatBot _chatBot;

       
        public MainWindow()
        {
            // Build the WPF visual tree defined in MainWindow.xaml
            InitializeComponent();

            // Create the chatbot 
            _chatBot = new ChatBot();

            // Play the audio greeting wave file on startup
            PlayVoiceGreeting();

            // Populate the ASCII art header from ChatBot
            LoadAsciiArt();

            // Show the chatbot's text greeting in the chat area
            AppendMessage("Bot", _chatBot.GetGreeting());
        }

        
        private void PlayVoiceGreeting()
        {
            try
            {
                // Create a player pointed at the greeting sound file
                SoundPlayer player = new SoundPlayer("greeting.wav");

                // Play asynchronously so the UI is not blocked
                player.Play();
            }
            catch
            {
                // File missing or audio subsystem unavailable — not fatal, carry on
            }
        }

        
        // Asks the ChatBot for the ASCII art string and writes it to the header control.       
        private void LoadAsciiArt()
        {
            // Retrieve the pre-built ASCII banner from ChatBot
            AsciiArtDisplay.Text = _chatBot.GetAsciiArt();
        }

        
        // Handles the Send button click — delegates to SendMessage.
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Forward the click to the shared send routine
            SendMessage();
        }

        
        // Handles the Enter key in the input box — delegates to SendMessage.       
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            // Only act on the Enter key; ignore all other keys
            if (e.Key == Key.Enter)
                SendMessage();
        }

        
        
        /// Reads the user's input, passes it to ChatBot, and displays both sides.     
        private void SendMessage()
        {
            // Read and trim the input field; do nothing if it is blank
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            // Echo the user's message to the chat display
            AppendMessage("You", input);

            // Clear the input field ready for the next message
            UserInput.Clear();

            // Delegate all response logic to ChatBot
            string response = _chatBot.ProcessInput(input);

            // Display the bot's reply
            AppendMessage("Bot", response);
        }

       
        private void AppendMessage(string sender, string message)
        {
            // Append the formatted line to the chat text block
            ChatDisplay.Text += $"{sender}: {message}\n\n";

            // Keep the latest message visible by scrolling the container to its end
            ChatScrollViewer.ScrollToEnd();
        }
    }
}
