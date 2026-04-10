using System;
//STUDENT ID: ST10399194 - PROG POE PART 1 - CHATBOT APPLICATION

namespace ChatBotApp;
class Program
{
    //main chat logic
    static void Main(string[] args)
    {   
        //Plays a welcome sound effect
        AudioPlayer.PlayGreeting();
        // displays the chatbot ASCII art   
        UI.DisplayAsciiArt();

        // sets the text coor to white and asks for name   
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Good Day!!! Please enter your name: ");
        string name = Console.ReadLine()?.Trim();

       // If the user doesn't enter a name, default to "User" to avoid null or empty values
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "User";
        }
        //creates ID card for the user with their name
        User user = new User(name);
        
        Chatbot chatbot = new Chatbot(user);    
        // starts the chatbot conversation loop
        chatbot.StartChat();
    }
}