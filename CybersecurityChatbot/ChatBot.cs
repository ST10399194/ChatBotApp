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
    }
}