using System;
using System.Collections.Generic;
using System.Threading;

namespace ChatBotApp;

public class Chatbot
{
    // Stores the current user so we can personalise responses throughout the chat
    private User user;

    // Dictionary mapping keyword triggers to arrays of possible responses.
    // Multiple responses per topic add variety — one is picked at random each time.
    // {name} is a placeholder that gets replaced with the user's actual name at runtime.
    private static readonly Dictionary<string, string[]> responses = new()
    {
        ["how are you"] = new[]
        {
            "I'm like a fridge... always running! But joking aside, I'm functioning perfectly and ready to help keep you safe online, {name}.",
            "All systems operational, {name}! Think of me as your always-on cybersecurity companion."
        },

        ["purpose"] = new[]
        {
            "Great question, {name}! My purpose is to be your personal cybersecurity guide. " +
            "I can help you with topics like phishing scams, password safety, malware, social engineering, VPNs, and much more. Just ask!"
        },

        ["password"] = new[]
        {
            "Passwords are your first line of defence, {name}! Here's what makes one strong:\n\n" +
            "  • Length: At least 12–16 characters\n" +
            "  • Complexity: Mix UPPERCASE, lowercase, numbers and symbols (e.g. P@ssw0rd!93)\n" +
            "  • Uniqueness: Never reuse passwords across sites\n" +
            "  • Avoid the obvious: No birthdays, pet names, or 'password123'\n\n" +
            "Pro tip: Use a password manager like Bitwarden or 1Password to generate and store them securely!"
        },

        ["phishing"] = new[]
        {
            "Phishing is one of the most common cyber threats out there, {name}. Here's what to watch for:\n\n" +
            "  • Suspicious sender addresses (e.g. support@paypa1.com instead of paypal.com)\n" +
            "  • Urgent language like 'Your account will be closed in 24 hours!'\n" +
            "  • Links that don't match the company's real domain\n" +
            "  • Unexpected attachments — even from people you know\n\n" +
            "Golden rule: When in doubt, go directly to the website instead of clicking any link in an email."
        },

        ["malware"] = new[]
        {
            "Malware is an umbrella term for malicious software, {name}. Here are the main types:\n\n" +
            "  • Viruses: Attach to files and spread when opened\n" +
            "  • Ransomware: Locks your files and demands payment to unlock them\n" +
            "  • Spyware: Secretly monitors your activity and steals data\n" +
            "  • Trojans: Disguise themselves as legitimate software\n\n" +
            "Protection tips: Keep your OS and antivirus updated, avoid pirated software, " +
            "and never plug in unknown USB drives."
        },

        ["vpn"] = new[]
        {
            "A VPN (Virtual Private Network) creates an encrypted tunnel for your internet traffic, {name}.\n\n" +
            "  • It hides your IP address from websites and your ISP\n" +
            "  • Protects you on public Wi-Fi (cafes, airports, hotels)\n" +
            "  • Allows you to bypass geographic content restrictions\n\n" +
            "Important: Not all VPNs are equal. Avoid free VPNs — they often log and sell your data. " +
            "Reputable options include ProtonVPN, Mullvad, and ExpressVPN."
        },

        ["two factor"] = new[]
        {
            "Two-factor authentication (2FA) adds a critical second layer of security, {name}!\n\n" +
            "  • Even if someone steals your password, they still can't log in without the second factor\n" +
            "  • Types of 2FA:\n" +
            "     - SMS codes (convenient but can be SIM-swapped)\n" +
            "     - Authenticator apps like Google Authenticator or Authy (much safer)\n" +
            "     - Hardware keys like YubiKey (most secure)\n\n" +
            "Enable 2FA on your email, banking, and social media accounts first — " +
            "those are the highest value targets."
        },

        ["social engineering"] = new[]
        {
            "Social engineering is when attackers manipulate people rather than hacking systems, {name}.\n\n" +
            "  • Pretexting: Creating a fake scenario to extract info ('Hi, I'm from IT — what's your password?')\n" +
            "  • Baiting: Leaving infected USB drives in car parks hoping someone picks them up\n" +
            "  • Tailgating: Following an employee into a secure area\n" +
            "  • Vishing: Voice phishing calls impersonating banks or government agencies\n\n" +
            "Defence: Always verify identities independently. No legitimate organisation will " +
            "pressure you for sensitive info on the spot."
        },

        ["safe browsing"] = new[]
        {
            "Here are key safe browsing habits for you, {name}:\n\n" +
            "  • Look for HTTPS (the padlock icon) before entering any personal info\n" +
            "  • Use a browser with strong privacy defaults (Firefox, Brave)\n" +
            "  • Install an ad blocker like uBlock Origin — many ads can carry malware\n" +
            "  • Keep your browser and extensions updated\n" +
            "  • Be cautious on public Wi-Fi — use a VPN if possible\n" +
            "  • Avoid clicking pop-ups that claim your device is infected"
        },

        // Triggered when the user expresses gratitude
        ["thank you"] = new[]
        {
            "You're very welcome, {name}! Staying informed is the first step to staying secure. Feel free to ask anything else!",
            "Happy to help, {name}! Cybersecurity can feel overwhelming, but you're doing great by asking the right questions.",
            "Anytime, {name}! That's what I'm here for. No question is too small when it comes to your safety online."
        },

        // Triggered when the user says bye / goodbye — also used for the exit farewell message
        ["bye"] = new[]
        {
            "Goodbye, {name}! Remember — strong passwords, think before you click, and stay safe out there!",
            "Take care, {name}! Cyber threats are always evolving, so stay curious and keep learning.",
            "See you later, {name}! You're already more cyber-aware than most people just by having this conversation."
        },
        // Triggered when the user says hello
        ["hello"] = new[]
        {
            "Hello, {name}! I'm your friendly cybersecurity bot. Ask me anything about staying safe online!",
            "Hi there, {name}! I'm here to help you navigate the world of cybersecurity. What would you like to know?"
        }
    };

    // Constructor — receives the User object from Program.cs
    public Chatbot(User user)
    {
        this.user = user;
    }

    // Simulates a typing effect by printing one character at a time with a short delay.
    
    private static void TypeEffect(string message, int delay = 18)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    // Main chat loop — greets the user, shows available topics, then
    // continuously reads input and routes it to Respond() until the user exits.
    public void StartChat()
    {
        // Personalised greeting using the typing effect
        Console.ForegroundColor = ConsoleColor.Yellow;
        TypeEffect($"\n  Hello {user.Name}! Welcome to the Cybersecurity Bot.");
        TypeEffect("  I'm here to help keep you safe online. Ask me anything!\n");

        // Show available topics as a quick reference
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("  Topics I can help with:");
        Console.WriteLine("  password  |  phishing  |  malware  |  vpn  |  two factor");
        Console.WriteLine("  social engineering  |  safe browsing  |  purpose\n");

        Console.ResetColor();
        Console.WriteLine("  Type 'exit' or 'bye' to end the conversation.\n");
        Console.WriteLine("  " + new string('─', 53));

        // Keep looping until the user exits
        while (true)
        {
            Console.WriteLine();

            // Print the YOU prompt label in green so it stands out from the bot
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  YOU  ");
            Console.ResetColor();
            Console.Write("> ");

            // Read and normalise input (lowercase + trimmed)
            string input = Console.ReadLine()?.ToLower().Trim();

            // Guard against empty input
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("  Please enter a question or topic.");
                Console.ResetColor();
                continue;
            }

            // Exit condition — catches both 'exit' and anything containing 'bye'
            if (input == "exit" || input.Contains("bye"))
            {
                // Pick a random farewell from the "bye" responses array
                var byeOptions = responses["bye"];
                string byeMsg = byeOptions[new Random().Next(byeOptions.Length)];

                Console.ForegroundColor = ConsoleColor.Yellow;
                TypeEffect($"\n  {byeMsg.Replace("{name}", user.Name)}");
                Console.ResetColor();
                break;
            }

            // Pass all other input to the response handler
            Respond(input);
        }
    }

    // Finds a matching response for the user's input and prints it
    private void Respond(string input)
    {
        Console.WriteLine();

        // Shows an animated thinking to stimulate a conversation
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  BOT  > thinking");
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(300);
            Console.Write(".");
        }
        Thread.Sleep(300);

        // Wipe the thinking line before printing the real response
        Console.Write("\r" + new string(' ', 30) + "\r");

        // Print the BOT label in cyan so it's visually distinct from the YOU label
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("  BOT  ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("> ");
        Console.ResetColor();
        Console.WriteLine();

        // Search the dictionary for the first keyword that appears in the user's input
        string responseText = null;
        foreach (var kvp in responses)
        {
            if (input.Contains(kvp.Key))
            {
                // Randomly pick one of the available responses for this keyword
                var options = kvp.Value;
                responseText = options[new Random().Next(options.Length)];
                break;
            }
        }

        // Fallback response if no keyword matched
        if (responseText == null)
        {
            responseText = $"I don't have specific info on that yet, {user.Name}. " +
                           "Try asking about: password, phishing, malware, vpn, two factor, " +
                           "social engineering, safe browsing, thank you!";
        }

        // Replace the {name} placeholder with the actual user name 
        responseText = responseText.Replace("{name}", user.Name);

        // Print each line of the response with the typing effect and consistent indentation
        Console.ForegroundColor = ConsoleColor.White;
        foreach (string line in responseText.Split('\n'))
        {
            Console.Write("         ");
            TypeEffect(line, 12);
        }

        // Reset colour and draw a separator line after each bot response
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  " + new string('─', 53));
    }
}