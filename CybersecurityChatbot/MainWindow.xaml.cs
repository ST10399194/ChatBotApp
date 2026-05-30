using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

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
        private void BorderlessBubble(
            Paragraph paragraph,
            Run text,
            string color)
        {
            text.Background =
                (SolidColorBrush)new BrushConverter().ConvertFromString(color);

            paragraph.Inlines.Add(text);
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
            private void AppendMessage(string sender, string message)
        {
            Paragraph paragraph = new Paragraph();

            if (sender == "You")
            {
                paragraph.TextAlignment = TextAlignment.Right;

                Run label = new Run("YOU\n")
                {
                    Foreground = Brushes.Cyan,
                    FontWeight = FontWeights.Bold
                };

                Run text = new Run(message)
                {
                    Foreground = Brushes.Black
                };

                BorderlessBubble(paragraph, text, "#00FFFF");

                paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, label);
            }
            else
            {
                paragraph.TextAlignment = TextAlignment.Left;

                Run label = new Run("CYBER BOT\n")
                {
                    Foreground = Brushes.Cyan,
                    FontWeight = FontWeights.Bold
                };

                Run text = new Run(message)
                {
                    Foreground = Brushes.White
                };

                BorderlessBubble(paragraph, text, "#1E3A5F");

                paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, label);
            }

            ChatDisplay.Document.Blocks.Add(paragraph);
            ChatDisplay.ScrollToEnd();
        }
    }
    }

