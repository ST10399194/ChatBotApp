using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
   
    public enum Sentiment
    {
        Neutral,      // No emotion detected — plain informational input
        Worried,      // User feels scared, anxious, or unsafe
        Curious,      // User is inquisitive or exploring a topic
        Frustrated,   // User is confused, annoyed, or stuck
        Happy,        // User expressed satisfaction or excitement
        Excited       // User is enthusiastic or eager to learn
    }

  
    // Detects the emotional tone of user input by scanning for trigger words,then provides an empathetic opening sentence and a relevant cybersecurity tip.
    public class SentimentDetector
    {
        
        // Maps each Sentiment to the words/phrases that trigger it.
        private readonly Dictionary<Sentiment, List<string>> _triggers;

        // Maps each empathetic response to its matching Sentiment.        
        private readonly Dictionary<Sentiment, string> _empathyMap;

        // Maps known cybersecurity topic keywords to a short actionable tip.     
        private readonly Dictionary<string, string> _topicTips;

      
        public SentimentDetector()
        {
            // Build the trigger word dictionary
            _triggers = new Dictionary<Sentiment, List<string>>
            {
                // Words that signal the user is worried, scared, or anxious
                {
                    Sentiment.Worried,
                    new List<string> { "worried", "scared", "afraid", "anxious", "nervous", "unsafe", "stressed", "panicking", "fear" }
                },

                // Words that signal the user is curious or inquisitive
                {
                    Sentiment.Curious,
                    new List<string> { "curious", "wondering", "interested", "want to know", "how does", "what is", "explain", "tell me" }
                },

                // Words that signal the user is frustrated or confused
                {
                    Sentiment.Frustrated,
                    new List<string> { "frustrated", "annoyed", "confused", "don't understand", "not working", "stuck", "useless", "why is it", "hate" }
                },

                // Words that signal the user is happy or satisfied
                {
                    Sentiment.Happy,
                    new List<string> { "great", "thanks", "helpful", "awesome", "love it", "happy", "good", "nice", "glad" }
                },

                // Words that signal the user is excited or enthusiastic
                {
                    Sentiment.Excited,
                    new List<string> { "excited", "wow", "amazing", "can't wait", "thrilled", "pumped", "fantastic", "brilliant", "incredible" }
                }
            };

            // Builds the empathy map
            _empathyMap = new Dictionary<Sentiment, string>
            {
                // Reassure a worried user before delivering information
                { Sentiment.Worried,    "I understand your concerns — it's completely normal to feel that way. Here's what you should know: " },

                // Match the user's curiosity with enthusiasm
                { Sentiment.Curious,    "Great question — I love the curiosity! Let's dive in: " },

                // Acknowledge frustration and promise clarity
                { Sentiment.Frustrated, "I can see this is frustrating — don't worry, let me break it down clearly: " },

                // Build on the user's positive mood
                { Sentiment.Happy,      "I'm happy to hear that! Here's something else that might be useful: " },

                // Match and amplify the user's excitement
                { Sentiment.Excited,    "That enthusiasm is great to see! Let's keep that energy going — here's what you need to know: " },

                // Neutral gets an empty string so nothing is prepended to the response
                { Sentiment.Neutral,    "" }
            };

            // Build the topic tip dictionary 
            _topicTips = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Password safety tip
                { "password",           "Use at least 12 characters and mix letters, numbers, and symbols. A password manager like Bitwarden makes this effortless." },

                // Phishing awareness tip
                { "phishing",           "Always check the sender's address and hover over links before clicking — urgent emails asking for action are a red flag." },

                // Malware protection tip
                { "malware",            "Keep your antivirus updated and avoid downloading files or software from unofficial sources." },

                // Privacy settings tip
                { "privacy",            "Review your social media privacy settings every few months — platforms often quietly change their defaults." },

                // Scam recognition tip
                { "scam",               "If an offer sounds too good to be true, it almost certainly is. Never send money via gift cards or cryptocurrency to someone you haven't met." },

                // VPN usage tip
                { "vpn",                "A VPN encrypts your traffic and is especially useful on public Wi-Fi. Stick to reputable paid options like ProtonVPN or Mullvad." },

                // Two-factor authentication tip
                { "two factor",         "Enable 2FA on your email and banking accounts first — an authenticator app like Authy is safer than SMS codes." },

                // Social engineering awareness tip
                { "social engineering", "Always verify someone's identity independently before sharing any information, no matter how convincing they seem." },

                // Safe browsing tip
                { "safe browsing",      "Look for HTTPS before entering personal info, and install uBlock Origin to block malicious ads." },

                // Data breach response tip
                { "breach",             "Check haveibeenpwned.com to see if your email has appeared in a known breach, then change affected passwords immediately." },

                // Firewall tip
                { "firewall",           "Make sure your OS firewall is enabled — Windows Defender Firewall and macOS's built-in firewall are solid starting points." },

                // Software update tip
                { "update",             "Enable automatic updates for your OS, browser, and antivirus — most attacks exploit vulnerabilities that patches already fixed." }
            };
        }

       
        public Sentiment DetectSentiment(string input)
        {
            // Normalise once so every comparison is case-insensitive
            string lower = input.ToLower();

            // Loop through each Sentiment 
            foreach (KeyValuePair<Sentiment, List<string>> pair in _triggers)
            {
                // Check whether any trigger word from this Sentiment's list appears in the input
                bool anyMatch = pair.Value.Any(word => lower.Contains(word));

                // Return the first matching Sentiment — first match wins
                if (anyMatch)
                    return pair.Key;
            }

            // No trigger words found ,default to Neutral
            return Sentiment.Neutral;
        }

        public string GetSentimentResponse(Sentiment sentiment)
        {
            // Look up the empathy sentence for this Sentiment in the map
            _empathyMap.TryGetValue(sentiment, out string empathy);

            // Return the empathy string (empty string for Neutral)
            return empathy ?? string.Empty;
        }

      
        public string GetTopicTip(string input)
        {
            // Normalise to lowercase for consistent matching
            string lower = input.ToLower();

            // Loop through each topic keyword in the tip dictionary
            foreach (string topic in _topicTips.Keys)
            {
                // Check whether this topic keyword appears anywhere in the input
                if (lower.Contains(topic))
                {
                    // Retrieve the tip safely using TryGetValue
                    _topicTips.TryGetValue(topic, out string tip);

                    // Return the matched tip
                    return tip;
                }
            }

            // No topic matched 
            return null;
        }
    }
}