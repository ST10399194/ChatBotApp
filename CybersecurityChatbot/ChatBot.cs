using System;

namespace CybersecurityChatbot
{
    
    public class ChatBot
    {
      
        // Handles keyword , response matching for all cybersecurity topics
        private readonly KeywordResponder _keywords;

        // Detects the user's emotional tone and returns empathetic openers
        private readonly SentimentDetector _sentiment;

        // Stores the user's name and favourite topic for personalisation
        private readonly MemoryStore _memory;

        // Used to pick a random fallback response when nothing else matches
        private readonly Random _random;

        // True until the user provides their name on the very first message
        private bool _awaitingName;

        // Stores the last matched topic so follow-up phrases can reuse it
        private string _lastTopic;

        // Shown when no pipeline step matched the user's input
        private readonly string[] _fallbacks =
        {
            "I'm not sure I understand — can you try asking differently?",
            "Hmm, I don't know about that yet. Try asking about passwords, phishing, or malware!",
            "Sorry, I didn't catch that. Try asking about privacy, VPNs, or two-factor authentication.",
            "That one's got me stumped! Ask me about a cybersecurity topic and I'll do my best to help."
        };

   
        public ChatBot()
        {
            // Create the keyword responder that owns all topics
            _keywords = new KeywordResponder();

            // Create the sentiment detector that reads emotional tone
            _sentiment = new SentimentDetector();

            // Create the memory store that tracks name and favourite topic
            _memory = new MemoryStore();

            // Shared random instance for fallback selection
            _random = new Random();

            // Bot starts in name-capture mode
            _awaitingName = true;

            // No topic has been discussed yet
            _lastTopic = "";
        }

      
        public string GetAsciiArt()
        {
            // Full-width ASCII banner
            return
@"_____    _____      _____       _____        ______        _____               _____          _____   _________________  
  ___|\    \  |\    \    /    /| ___|\     \   ___|\     \   ___|\    \         ___|\     \    ____|\    \ /                 \ 
 /    /\    \ | \    \  /    / ||    |\     \ |     \     \ |    |\    \       |    |\     \  /     /\    \\______     ______/ 
|    |  |    ||  \____\/    /  /|    | |     ||     ,_____/||    | |    |      |    | |     |/     /  \    \  \( /    /  )/    
|    |  |____| \ |    /    /  / |    | /_ _ / |     \--'\_|/|    |/____/       |    | /_ _ /|     |    |    |  ' |   |   '     
|    |   ____   \|___/    /  /  |    |\    \  |     /___/|  |    |\    \       |    |\    \ |     |    |    |    |   |         
|    |  |    |      /    /  /   |    | |    | |     \____|\ |    | |    |      |    | |    ||\     \  /    /|   /   //         
|\ ___\/    /|     /____/  /    |____|/____/| |____ '     /||____| |____|      |____|/____/|| \_____\/____/ |  /___//          
| |   /____/ |    |`    | /     |    /     || |    /_____/ ||    | |    |      |    /     || \ |    ||    | / |`   |           
 \|___|    | /    |_____|/      |____|_____|/ |____|     | /|____| |____|      |____|_____|/  \|____||____|/  |____|           
   \( |____|/        )/           \(    )/      \( |_____|/   \(     )/          \(    )/        \(    )/       \(             
    '   )/           '             '    '        '    )/       '     '            '    '          '    '         '";
        }

        
        public string GetGreeting()
        {
            // Ask for the user's name — the first reply will be caught by Step 1
            return "Hello! I'm your cybersecurity assistant. Before we begin, what's your name?";
        }

    
        public string ProcessInput(string input)
        {
   
            input = input.Trim();

         
            if (string.IsNullOrEmpty(input))
                return "Please type something so I can help you!";

            // Normalise to lowercase once — reused by all steps below
            string lower = input.ToLower();

    
            if (_awaitingName)
            {
                return CaptureNameAndWelcome(input);
            }

            if (IsFollowUp(lower))
            {
                return HandleFollowUp();
            }

            string specialResponse = HandleSpecialPhrases(lower);
            if (specialResponse != null)
            {
                return specialResponse;
            }

            // Detect the user's emotional tone. If it is not Neutral, retrieve an empathetic opener that will be prepended to whatever response follows.
            Sentiment sentiment = _sentiment.DetectSentiment(input);
            string sentimentOpener = sentiment != Sentiment.Neutral
                                        ? _sentiment.GetSentimentResponse(sentiment)
                                        : "";

            // Also check whether a topic tip is available for the detected emotion
            string topicTip = _sentiment.GetTopicTip(input);

            // If the user expressed an emotion AND the input matched a topic tip,        
            if (sentiment != Sentiment.Neutral && !string.IsNullOrEmpty(topicTip))
            {
                return _memory.InjectName(sentimentOpener + topicTip);
            }

         
            // Ask KeywordResponder for a response. It returns a fallback string
            string keywordResponse = _keywords.GetResponse(input);
            bool keywordMatched = !keywordResponse.StartsWith("I'm not sure");

            if (keywordMatched)
            {
                
                _lastTopic = input;

                // Update memory with the topic the user just asked about
                _memory.DetectTopic(input);

                // Also try to capture a name if the user mentioned one mid-conversation
                _memory.TryCaptureName(input);

                string assembled = sentimentOpener + _memory.GetOpener() + keywordResponse;
                return _memory.InjectName(assembled);
            }

            string fallback = _fallbacks[_random.Next(_fallbacks.Length)];
            return _memory.InjectName(fallback);
        }

        private string CaptureNameAndWelcome(string input)
        {
            // Store the name
            string name = input.Length > 1
                ? char.ToUpper(input[0]) + input.Substring(1).ToLower()
                : input.ToUpper();

            // Save the captured name to memory for use throughout the conversation
            _memory.UserName = name;

            // Flip the flag — all subsequent messages bypass Step 1
            _awaitingName = false;

            // Return a warm, personalised welcome that lists the available topics
            return $"Welcome, {name}! I'm here to help you stay safe online.\n\n" +
                   $"You can ask me about:\n" +
                   $"  • Passwords & two-factor authentication\n" +
                   $"  • Phishing & social engineering\n" +
                   $"  • Malware, firewalls & updates\n" +
                   $"  • Privacy, VPNs & safe browsing\n" +
                   $"  • Scams & data breaches\n\n" +
                   $"What would you like to know, {name}?";
        }

        private bool IsFollowUp(string lower)
        {
            // Only treat as a follow-up if there is actually a previous topic to revisit
            if (string.IsNullOrEmpty(_lastTopic))
                return false;

            // Check for any recognised follow-up phrase in the input
            return lower.Contains("tell me more")
                || lower.Contains("explain more")
                || lower.Contains("more info")
                || lower.Contains("go on")
                || lower.Contains("continue");
        }

        private string HandleFollowUp()
        {
            // Get another response from the last topic's pool
            string followUpResponse = _keywords.GetResponse(_lastTopic);

            // Inject the user's name and return
            return _memory.InjectName(followUpResponse);
        }


        private string HandleSpecialPhrases(string lower)
        {
            // "how are you" 
            if (lower.Contains("how are you"))
                return $"I'm running at full capacity, {_memory.UserName}! Ready to help you stay safe online.";

            // "what can you do" 
            if (lower.Contains("what can you do") || lower.Contains("what can i ask"))
            {
                string topics = string.Join(", ", _keywords.GetAllKeywords());
                return $"I can help you with: {topics}. Just ask about any of these topics!";
            }

            // "purpose"
            if (lower.Contains("purpose") || lower.Contains("what are you"))
                return "My purpose is to be your personal cybersecurity guide — helping you understand threats, " +
                       "protect your accounts, and stay safe online. Ask me anything!";

            
            if (lower.Contains("what do you remember") || lower.Contains("what do you know about me"))
                return _memory.GetMemorySummary();

            // "my name is" / "call me" mid-conversation name update
            if (lower.Contains("my name is") || lower.Contains("call me") || lower.Contains("i'm"))
            {
                // Attempt to extract and save the updated name
                _memory.TryCaptureName(lower);

                // Confirm the update if a name was captured
                if (!string.IsNullOrEmpty(_memory.UserName))
                    return $"Got it — I'll call you {_memory.UserName} from now on!";
            }

            // No special phrase matched
            return null;
        }
        public string ProcessInput(string userInput)
        {
            // Standardize string input parsing casing
            string input = userInput.ToLower().Trim();

            // CHECK FOR LOG / SUMMARY INTENT
          
            if (input.Contains("show activity log") || input.Contains("what have you done") ||
                input.Contains("what did you do") || input.Contains("show log") || input.Contains("recent actions"))
            {
                return ActivityLogger.GetRecentLog();
            }

            
            //  CHECK FOR TASK INTENT
           
            if (input.Contains("add task") || input.Contains("add a task") || input.Contains("create task") ||
                input.Contains("i need to") || input.Contains("enable") || input.Contains("set up"))
            {
                string taskDescription = ExtractTaskDescription(userInput);

                // Add default reminder state flag text
                _taskManager.AddTask(taskDescription, "Created via conversational NLP stream.", "None");
                ActivityLogger.Log($"Task added: '{taskDescription}' (no reminder set).");

                return $"Task added: '{taskDescription}.' Would you like to set a reminder for this task?";
            }

            
            // CHECK FOR REMINDER INTENT
           
            if (input.Contains("remind me") || input.Contains("reminder") || input.Contains("set a reminder") ||
                input.Contains("remind me to") || input.Contains("don't forget"))
            {
                string reminderDetails = ExtractReminderDetails(userInput);

                _taskManager.AddTask("Reminder Task", reminderDetails, "Tomorrow");
                ActivityLogger.Log($"Reminder set for '{reminderDetails}' tomorrow.");

                return $"Reminder set for '{reminderDetails}' on tomorrow's date.";
            }

            //  CHECK FOR QUIZ INTENT
           
            if (input.Contains("start quiz") || input.Contains("take quiz") || input.Contains("test my knowledge") ||
                input.Contains("quiz me") || input.Contains("play the game"))
            {
                return "Understood. Launching the Cybersecurity Evaluation Quiz! Please navigate to the 'Security Quiz' tab on the right side of your panel panel area to begin.";
            }

            
            // FALL THROUGH TO EXISTING CYBERSECURITY TOPICS / FALLBACK
            
            if (input.Contains("password"))
                return "Security Protocol Note: Always keep passwords above 12 characters and use randomized phrase strings.";
            if (input.Contains("phishing"))
                return "Security Protocol Note: Check domain records carefully before providing session credentials.";
            if (input.Contains("2fa") || input.Contains("two-factor"))
                return "Security Protocol Note: Multi-factor authentication adds cryptographic barriers against session hijacking.";
            if (input.Contains("malware"))
                return "Security Protocol Note: Isolate infected segments immediately from standard network trees.";

            // Fallback response pattern matching
            return "I did not quite understand that. Try asking me to 'add a task', 'set a reminder', 'show activity log', or 'start quiz'.";
        }

       
        // NLP STRING MANIPULATION EXTRACTION HELPERS
        

        private string ExtractTaskDescription(string fullInput)
        {
            string lower = fullInput.ToLower();
            string keyPhrase = "";

            if (lower.Contains("add a task to ")) keyPhrase = "add a task to ";
            else if (lower.Contains("add task to ")) keyPhrase = "add task to ";
            else if (lower.Contains("i need to ")) keyPhrase = "i need to ";
            else if (lower.Contains("create task ")) keyPhrase = "create task ";

            if (!string.IsNullOrEmpty(keyPhrase))
            {
                int startIndex = lower.IndexOf(keyPhrase) + keyPhrase.Length;
                if (startIndex < fullInput.Length)
                {
                    string extracted = fullInput.Substring(startIndex).Trim();
                    return CapitalizeFirstLetter(extracted);
                }
            }

            return "Configure Security Optimization Setting"; // Fallback text value
        }

        private string ExtractReminderDetails(string fullInput)
        {
            string lower = fullInput.ToLower();
            string keyPhrase = "";

            if (lower.Contains("remind me to ")) keyPhrase = "remind me to ";
            else if (lower.Contains("remind me ")) keyPhrase = "remind me ";

            if (!string.IsNullOrEmpty(keyPhrase))
            {
                int startIndex = lower.IndexOf(keyPhrase) + keyPhrase.Length;
                string segment = fullInput.Substring(startIndex).Trim();

                // Strip trailing timing punctuation identifiers like "tomorrow" to mirror assignment outputs
                if (segment.ToLower().EndsWith(" tomorrow.")) segment = segment.Substring(0, segment.Length - 10).Trim();
                else if (segment.ToLower().EndsWith(" tomorrow")) segment = segment.Substring(0, segment.Length - 9).Trim();

                return CapitalizeFirstLetter(segment);
            }

            return "Update security parameters"; // Fallback value
        }

        private string CapitalizeFirstLetter(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}

