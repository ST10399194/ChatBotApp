using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    
    // Maps keyword triggers to pools of responses and returns a random,   
    public class KeywordResponder
    {

        // Each keyword maps to a pool of responses; multiple entries ensure variety
        private readonly Dictionary<string, List<string>> _responses;

        // Remembers the last index used per keyword so the same response is never repeated twice in a row
        private readonly Dictionary<string, int> _lastIndexUsed;

        // Shared random instance — one instance is enough and avoids seed collisions
        private readonly Random _random;

       
        //Initialises all response pools and supporting state.     
        public KeywordResponder()
        {
            // Initialise the response dictionary that holds all keyword → response pools
            _responses = new Dictionary<string, List<string>>();

            // Initialise the tracker that prevents back-to-back identical responses
            _lastIndexUsed = new Dictionary<string, int>();

            // Single Random instance shared across all picks
            _random = new Random();

            // Populate all keyword → response mappings
            PopulateResponses();
        }

        public string GetResponse(string input)
        {
            // Normalise to lowercase once so every comparison is case-insensitive
            string lowerInput = input.ToLower();

            // Walk every registered keyword and return on the first match
            foreach (string key in _responses.Keys)
            {
                if (lowerInput.Contains(key.ToLower()))
                    return PickResponse(key);  // Random, non-repeating pick from the pool
            }

            // No keyword matched — build a helpful fallback listing all topics
            string topicList = string.Join(", ", _responses.Keys);
            return $"I'm not sure how to help with that just yet. Try asking about: {topicList}.";
        }

       
        // Returns every registered keyword. Used to build the "what can I ask?" topic list.      
        public List<string> GetAllKeywords()
        {
            // Project the dictionary keys into a plain list for easy display
            return _responses.Keys.ToList();
        }

        
        private string PickResponse(string key)
        {
            List<string> pool = _responses[key];

            // Single-entry pool — no choice to make, return immediately
            if (pool.Count == 1)
                return pool[0];

            // Retrieve the last index used for this keyword (-1 if it has never been used)
            int lastIndex = _lastIndexUsed.ContainsKey(key) ? _lastIndexUsed[key] : -1;

            // Keep rolling until we land on a different index than last time
            int newIndex;
            do
            {
                newIndex = _random.Next(pool.Count);
            }
            while (newIndex == lastIndex);

            // Record the chosen index so the next call can avoid it
            _lastIndexUsed[key] = newIndex;

            // Return the chosen response from the pool
            return pool[newIndex];
        }

        
        private void PopulateResponses()
        {

            // Responds to "hello" / "hi"
            _responses["hello"] = new List<string>
            {
                "Hello, {name}! I'm your friendly cybersecurity bot. Ask me anything about staying safe online!",
                "Hi there, {name}! I'm here to help you navigate the world of cybersecurity. What would you like to know?"
            };

            // Responds to "how are you"
            _responses["how are you"] = new List<string>
            {
                "I'm like a fridge — always running! But joking aside, I'm functioning perfectly and ready to keep you safe online, {name}.",
                "All systems operational, {name}! Think of me as your always-on cybersecurity companion."
            };

            // Responds to questions about what this bot does
            _responses["purpose"] = new List<string>
            {
                "My purpose is to be your personal cybersecurity guide, {name}! " +
                "I can help with phishing, passwords, malware, social engineering, VPNs, and much more. Just ask!"
            };

            
            // Password safety
            _responses["password"] = new List<string>
            {
                "Strong passwords are your first line of defence! Aim for at least 12 characters mixing uppercase, lowercase, numbers, and symbols. " +
                "'T!ger$89Lemon' works far better than 'password123'.",

                "Never reuse the same password across multiple sites. If one service is breached, attackers will try your credentials everywhere. " +
                "A password manager like Bitwarden or 1Password makes this effortless.",

                "A passphrase can be both memorable and secure. 'Coffee!Runs@Monday3am' is long, random, and harder to crack than a short complex string.",

                "Avoid passwords based on personal info — birthdays, pet names, favourite teams. These are the first things attackers try.",

                "Here's what makes a strong password, {name}:\n\n" +
                "  • Length: At least 12–16 characters\n" +
                "  • Complexity: Mix UPPERCASE, lowercase, numbers and symbols\n" +
                "  • Uniqueness: Never reuse passwords across sites\n" +
                "  • Avoid the obvious: No birthdays, pet names, or 'password123'"
            };

            // Phishing awareness
            _responses["phishing"] = new List<string>
            {
                "Phishing emails create false urgency — 'Your account will be suspended in 24 hours!' Before clicking anything, hover over the link and check the real URL.",

                "Look closely at sender addresses. Attackers spoof companies using domains like 'support@paypa1.com' (note the '1'). " +
                "When in doubt, go directly to the website instead of clicking email links.",

                "Spear phishing targets you personally using social media info. If an unexpected email asks you to take urgent action, " +
                "verify it by calling the organisation on a number you find yourself.",

                "Attachments in phishing emails can install malware even if you don't run them. " +
                "Be suspicious of unexpected PDFs, Word docs, or ZIP files — even from people you know.",

                "Here's what to watch for, {name}:\n\n" +
                "  • Suspicious sender addresses (e.g. support@paypa1.com)\n" +
                "  • Urgent language like 'Your account will be closed in 24 hours!'\n" +
                "  • Links that don't match the company's real domain\n" +
                "  • Unexpected attachments — even from people you know\n\n" +
                "Golden rule: When in doubt, go directly to the website."
            };

            // Malware types and prevention
            _responses["malware"] = new List<string>
            {
                "Malware covers many threats: viruses spread via files, ransomware encrypts your data for payment, " +
                "spyware monitors you silently, and trojans disguise themselves as legitimate software. Keep your antivirus updated.",

                "Ransomware can be devastating. Follow the 3-2-1 backup rule: 3 copies of your data, " +
                "on 2 different media types, with 1 stored offsite or in the cloud.",

                "Avoid downloading software from unofficial sources. Pirated games and cracked apps are a common delivery method for trojans and keyloggers.",

                "Never plug in an unknown USB drive — malicious drives can execute code automatically the moment they're inserted.",

                "Here are the main malware types, {name}:\n\n" +
                "  • Viruses: Attach to files and spread when opened\n" +
                "  • Ransomware: Locks your files and demands payment\n" +
                "  • Spyware: Secretly monitors your activity\n" +
                "  • Trojans: Disguise themselves as legitimate software\n\n" +
                "Protection tips: Keep your OS and antivirus updated, avoid pirated software, and never plug in unknown USB drives."
            };

            // Online privacy
            _responses["privacy"] = new List<string>
            {
                "Review your social media privacy settings every few months — platforms often change their defaults in ways that expose more of your data.",

                "Be mindful of data shared with apps. Many free services monetise your behaviour and location. " +
                "Ask yourself: does this app really need access to my contacts and camera?",

                "Use encrypted messaging apps like Signal for sensitive conversations. End-to-end encryption means only you and the recipient can read messages.",

                "Your digital footprint is larger than you think. Search your own name, opt out of data broker sites, " +
                "and use a separate email alias for online signups to reduce exposure."
            };

            // Scam recognition
            _responses["scam"] = new List<string>
            {
                "If an offer sounds too good to be true, it almost certainly is. Lottery wins, unexpected inheritances, " +
                "and 'exclusive' investment opportunities are classic scam hooks.",

                "Scammers impersonate banks, governments, and tech support. Legitimate organisations will never call you " +
                "and demand remote access or gift card payments.",

                "Romance scams are on the rise. If someone you met online becomes emotionally intense quickly and then asks for money, " +
                "stop all contact and report it to the platform immediately.",

                "Never send money via wire transfer, cryptocurrency, or gift cards to someone you haven't met in person. " +
                "These methods are favoured by scammers because they are virtually irreversible."
            };

            // VPN usage
            _responses["vpn"] = new List<string>
            {
                "A VPN encrypts your internet traffic and masks your IP address — especially valuable on public Wi-Fi " +
                "where attackers can intercept unprotected data.",

                "Not all VPNs are trustworthy. Free VPN services often log and sell your activity. " +
                "Reputable paid options include ProtonVPN, Mullvad, and ExpressVPN.",

                "A VPN is not a silver bullet. It protects your traffic to the VPN server but does not protect you from phishing, malware, or weak passwords.",

                "A VPN creates an encrypted tunnel for your traffic, {name}.\n\n" +
                "  • Hides your IP from websites and your ISP\n" +
                "  • Protects you on public Wi-Fi\n" +
                "  • Allows bypassing geographic content restrictions\n\n" +
                "Avoid free VPNs — they often log and sell your data."
            };

            // Two-factor authentication
            _responses["two factor"] = new List<string>
            {
                "2FA adds a critical second lock. Even if a hacker steals your password, they cannot log in without the second factor. " +
                "Enable it everywhere — especially email and banking.",

                "SMS-based 2FA is vulnerable to SIM-swapping. Use an authenticator app like Authy or Google Authenticator for stronger protection.",

                "Hardware security keys like YubiKey offer the gold standard — they are phishing-resistant and cryptographically verify the real site.",

                "Prioritise enabling 2FA on your email account above all else. Your inbox is the master key to every other account via password resets.",

                "Two-factor authentication adds a second layer of security, {name}!\n\n" +
                "  • SMS codes — convenient but can be SIM-swapped\n" +
                "  • Authenticator apps (Google Authenticator, Authy) — much safer\n" +
                "  • Hardware keys (YubiKey) — most secure\n\n" +
                "Enable 2FA on email, banking, and social media first — those are the highest-value targets."
            };

            // Social engineering tactics
            _responses["social engineering"] = new List<string>
            {
                "Social engineering exploits human psychology. Attackers use urgency, fear, and authority to pressure people " +
                "into handing over information before they have time to think critically.",

                "Pretexting is when an attacker creates a fake scenario to gain your trust — for example, " +
                "pretending to be IT support who needs your login 'to fix a critical issue'. Always verify identities independently.",

                "Tailgating is a physical attack where someone follows an employee through a secured door. " +
                "Always ensure doors close behind you and challenge anyone you don't recognise in restricted areas.",

                "Vishing involves phone calls impersonating banks or government agencies. " +
                "If a call feels pressured or asks for personal details, hang up and call the organisation back using a number from their official website.",

                "Attackers manipulate people rather than hacking systems, {name}.\n\n" +
                "  • Pretexting: Fake scenarios to extract info\n" +
                "  • Baiting: Infected USB drives left in public hoping someone picks them up\n" +
                "  • Tailgating: Following staff into secure areas\n" +
                "  • Vishing: Voice phishing calls impersonating banks or government\n\n" +
                "Defence: Always verify identities independently — no legitimate organisation will pressure you for sensitive info on the spot."
            };

            // Safe browsing habits
            _responses["safe browsing"] = new List<string>
            {
                "Always check for HTTPS and a padlock icon before entering personal or payment info. HTTP sites transmit data in plain text.",

                "Install uBlock Origin. Beyond blocking ads, it protects you from malvertising — malicious ads that can infect your device just by being displayed.",

                "Keep your browser and extensions up to date. Outdated components are a common attack vector. Also audit installed extensions regularly.",

                "Be sceptical of pop-ups claiming your device is infected or that you've won a prize — these are almost always scams.",

                "Key safe browsing habits, {name}:\n\n" +
                "  • Look for HTTPS before entering any personal info\n" +
                "  • Use a privacy-first browser (Firefox, Brave)\n" +
                "  • Install uBlock Origin — many ads can carry malware\n" +
                "  • Keep your browser and extensions updated\n" +
                "  • Use a VPN on public Wi-Fi\n" +
                "  • Avoid pop-ups claiming your device is infected"
            };

            // Data breach response
            _responses["breach"] = new List<string>
            {
                "If your data was exposed, change the affected password immediately — and any other accounts using the same password. " +
                "Check haveibeenpwned.com to see if your email appears in known breaches.",

                "Companies must notify you of breaches but notifications can be slow. " +
                "Set up breach alerts at haveibeenpwned.com so you hear about it sooner.",

                "After a breach, be extra vigilant for phishing emails. Attackers who obtained your email will follow up " +
                "with targeted phishing using your real name and details to seem credible."
            };

            // Firewall basics
            _responses["firewall"] = new List<string>
            {
                "A firewall acts as a gatekeeper between your device and the internet, blocking unauthorised connections. " +
                "Ensure your OS firewall is enabled — Windows Defender Firewall and macOS's built-in firewall are solid starting points.",

                "For businesses, a hardware firewall at the network perimeter provides an additional layer — " +
                "blocking entire categories of malicious traffic before it reaches your machines.",

                "A firewall is not a complete solution on its own. It works best alongside antivirus software, regular updates, and strong authentication."
            };

            // Software updates / patching
            _responses["update"] = new List<string>
            {
                "Software updates often contain critical security patches. Delaying them leaves known vulnerabilities open. " +
                "Enable automatic updates wherever possible — especially for your OS, browser, and antivirus.",

                "The WannaCry ransomware attack in 2017 infected hundreds of thousands of machines by exploiting a vulnerability " +
                "Microsoft had already patched months earlier. Keeping systems updated is one of the most impactful things you can do.",

                "Don't forget firmware updates for your router and smart home devices. Unpatched routers are a common entry point for attackers."
            };

           
            // Responds to "thank you" / "thanks"
            _responses["thank you"] = new List<string>
            {
                "You're very welcome, {name}! Staying informed is the first step to staying secure. Feel free to ask anything else!",
                "Happy to help, {name}! Cybersecurity can feel overwhelming, but you're asking the right questions.",
                "Anytime, {name}! That's what I'm here for — no question is too small when it comes to your safety online."
            };

            // Responds to "bye" / "goodbye" — also used for the exit farewell message
            _responses["bye"] = new List<string>
            {
                "Goodbye, {name}! Remember: strong passwords, think before you click, and stay safe out there!",
                "Take care, {name}! Cyber threats are always evolving, so stay curious and keep learning.",
                "See you later, {name}! You're already more cyber-aware than most just by having this conversation."
            };
        }
    }
}