using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls; 

namespace CybersecurityChatbot
{
    public class MainWindow : Window
    {
        // The chatbot that owns all conversational logic
        private readonly ChatBot _chatBot;

        // Step 2.2: The manager engine handling CRUD and business workflow orchestration
        private readonly TaskManager _taskManager;

        public MainWindow()
        {
            // Build the WPF visual tree defined in MainWindow.xaml
            InitializeComponent();

            // Create the chatbot 
            _chatBot = new ChatBot();

            
            _taskManager = new TaskManager();

            // Play the audio greeting wave file on startup
            PlayVoiceGreeting();

            // Populate the ASCII art header from ChatBot
            LoadAsciiArt();

            // Automatically load saved tasks at application startup
            RefreshTaskGrid();

            // Show the chatbot's text greeting in the chat area
            Loaded += async (s, e) =>
            {
                await TypeBotMessage(_chatBot.GetGreeting());
            };
        }

        
        // Keeps the visual DataGrid display perfectly synchronized with the underlying JSON file storage layers.
        
        private void RefreshTaskGrid()
        {
            if (dgTasks != null)
            {
                dgTasks.ItemsSource = null;
                dgTasks.ItemsSource = _taskManager.GetAllTasks();
            }
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

        private async Task TypeBotMessage(string fullMessage)
        {
            Paragraph paragraph = new Paragraph
            {
                TextAlignment = TextAlignment.Left
            };

            Run label = new Run("CYBER BOT\n")
            {
                Foreground = Brushes.Cyan,
                FontWeight = FontWeights.Bold,
                FontSize = 12
            };

            Run text = new Run("")
            {
                Foreground = Brushes.White
            };

            paragraph.Inlines.Add(label);
            paragraph.Inlines.Add(text);

            ChatDisplay.Document.Blocks.Add(paragraph);

            foreach (char c in fullMessage)
            {
                text.Text += c;

                ChatDisplay.ScrollToEnd();

                await Task.Delay(15);
            }

            ChatDisplay.ScrollToEnd();
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
        private async void SendButton_Click(
              object sender,
              RoutedEventArgs e)
        {
            await SendMessage();
        }

        // Handles the Enter key in the input box — delegates to SendMessage.       
        private async void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            // Only act on the Enter key; ignore all other keys
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                await SendMessage();
            }
        }

        /// Reads the user's input, passes it to ChatBot, and displays both sides.     
        private async Task SendMessage()
        {
            // Read and trim the input field; do nothing if it is blank
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            // Echo the user's message to the chat display
            AppendMessage("You", input);

            // Clear the input field ready for the next message
            UserInput.Clear();

            await Task.Delay(300);

            // Fetch the chat string processing computation payload
            string botResponse = _chatBot.ProcessInput(input);
            await TypeBotMessage(botResponse);

            // Step 2.1: Intercept the conversation to capture automation phrases
            if (botResponse.Contains("Task added with the description"))
            {
                // Instantly sync layout interface view window
                RefreshTaskGrid();
            }
        }

        private void AppendMessage(string sender, string message)
        {
            Paragraph paragraph = new Paragraph();

            if (sender == "You")
            {
                paragraph.TextAlignment = TextAlignment.Right;

                Run label = new Run("YOU\n")
                {
                    Foreground = Brushes.Cyan,
                    FontWeight = FontWeights.Bold,
                    FontSize = 12
                };

                Run text = new Run(message)
                {
                    Foreground = Brushes.Black
                };

                BorderlessBubble(paragraph, text, "#00FFFF");

                if (paragraph.Inlines.FirstInline != null)
                    paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, label);
                else
                    paragraph.Inlines.Add(label);
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

                if (paragraph.Inlines.FirstInline != null)
                    paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, label);
                else
                    paragraph.Inlines.Add(label);
            }

            ChatDisplay.Document.Blocks.Add(paragraph);
            ChatDisplay.ScrollToEnd();
        }

        
        // Handles the manual creation input sequence loop.
        
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            string description = txtTaskDescription.Text.Trim();
            string reminder = txtTaskReminder.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please provide a title and description.", "Input Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Route execution commands downward
            string message = _taskManager.AddTask(title, description, reminder);

            // Clean data panel fields
            txtTaskTitle.Clear();
            txtTaskDescription.Clear();
            txtTaskReminder.Clear();

            RefreshTaskGrid();
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
        // Modifies state trackers to declare an action complete.
        
        private void btnMarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is CyberTask selectedTask)
            {
                _taskManager.MarkAsComplete(selectedTask.Id);
                RefreshTaskGrid();
            }
            else
            {
                MessageBox.Show("Please highlight an entry row inside the task manager grid to complete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
        //Erases data items out of database sets permanently.
        
        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is CyberTask selectedTask)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedTask.Title}'?", "Confirm Record Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _taskManager.DeleteTask(selectedTask.Id);
                    RefreshTaskGrid();
                }
            }
            else
            {
                MessageBox.Show("Please select a valid entry grid element line to remove.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
