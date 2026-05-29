using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    
    public class MemoryStore
    {
        // The user's name, captured when they introduce themselves 
        public string UserName { get; set; } = "";

        public string FavouriteTopic { get; set; } = "";

        // All cybersecurity topics the bot can recognise and remember from user input.
        private readonly List<string> _knownTopics = new List<string>
        {
            "password",
            "phishing",
            "malware",
            "privacy",
            "scam",
            "vpn",
            "two factor",
            "social engineering",
            "safe browsing",
            "breach",
            "firewall",
            "update"
        };

        // Phrases that signal the user is expressing interest in a topic.
        private readonly List<string> _interestPhrases = new List<string>
        {
            "interested in",
            "want to know about",
            "want to learn about",
            "curious about",
            "tell me about",
            "i like",
            "i love",
            "my favourite",
            "i care about",
            "worried about",
            "concerned about"
        };

        // Phrases used by TryCaptureName() to extract the user's name from input.
        private readonly List<string> _namePhrases = new List<string>
        {
            "my name is",
            "i am",
            "i'm",
            "call me",
            "you can call me"
        };

        public void TryCaptureName(string input)
        {
            // Normalise to lowercase for consistent phrase matching
            string lower = input.ToLower();

            // Loop through each name-capture phrase
            foreach (string phrase in _namePhrases)
            {
                // Check whether this phrase appears in the input
                if (lower.Contains(phrase))
                {
                    // Find where the phrase ends so we can read what comes after it
                    int index = lower.IndexOf(phrase) + phrase.Length;

                    // Extract everything after the phrase and clean up whitespace
                    string remainder = input.Substring(index).Trim();

                    // Take only the first word (the name) and remove punctuation
                    string name = remainder.Split(' ')[0].Trim('.', ',', '!', '?');

                    // Only save if the extracted word is non-empty and looks like a name
                    if (!string.IsNullOrWhiteSpace(name) && name.Length > 1)
                    {
                        // Capitalise the first letter so "sarah" becomes "Sarah"
                        UserName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                        return; 
                    }
                }
            }
        }

   
        public void DetectTopic(string input)
        {
            // Normalise to lowercase for consistent matching
            string lower = input.ToLower();

          
            foreach (string phrase in _interestPhrases)
            {
                // Check whether this interest phrase appears in the input
                if (lower.Contains(phrase))
                {
                    // Now check whether a known topic also appears in the same input
                    foreach (string topic in _knownTopics)
                    {
                        if (lower.Contains(topic))
                        {
                            // Both interest phrase and topic found — save and return
                            FavouriteTopic = topic;
                            return;
                        }
                    }
                }
            }

            // Second pass: no interest phrase, but a topic was still mentioned directly
            foreach (string topic in _knownTopics)
            {
                if (lower.Contains(topic))
                {
                    // Save the first matched topic and stop scanning
                    FavouriteTopic = topic;
                    return;
                }
            }
        }

        public string InjectName(string response)
        {
            // Use the stored name if available, otherwise fall back to "there"
            string nameToUse = string.IsNullOrEmpty(UserName) ? "there" : UserName;

            // Replace every occurrence of the placeholder with the chosen name
            return response.Replace("{name}", nameToUse);
        }

        public string GetOpener()
        {
            // Both name and topic known 
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FavouriteTopic))
                return $"{UserName}, as someone interested in {FavouriteTopic}, here's something relevant: ";

            // Topic known but not the name 
            if (!string.IsNullOrEmpty(FavouriteTopic))
                return $"As someone interested in {FavouriteTopic}, here's something relevant: ";

            // Name known but no topic stored yet 
            if (!string.IsNullOrEmpty(UserName))
                return $"{UserName}, here's a tip for you: ";

            // Nothing stored yet — return empty so the response is not prefixed awkwardly
            return "";
        }

        //Returns a summary of what the bot currently remembers about the user.
        public string GetMemorySummary()
        {
            // Build a summary depending on what has been stored
            bool hasName = !string.IsNullOrEmpty(UserName);
            bool hasTopic = !string.IsNullOrEmpty(FavouriteTopic);

            // Both stored 
            if (hasName && hasTopic)
                return $"I remember that your name is {UserName} and you're interested in {FavouriteTopic}.";

            // Only name stored
            if (hasName)
                return $"I remember your name is {UserName}, but I don't know your favourite topic yet.";

            // Only topic stored
            if (hasTopic)
                return $"I remember you're interested in {FavouriteTopic}, but I don't know your name yet.";

            // Nothing stored at all
            return "I don't know much about you yet! You can tell me your name or what topic you're most interested in.";
        }
    }
}