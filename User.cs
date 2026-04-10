using System;

namespace ChatBotApp;

public class User
{
    //placeholder for user name, to personalize the bot's responses
public string Name { get; set; }

// Constructor to initialize the user's name when creating a User object
public User(string name)
{
    // Assign the provided name to the Name property
    Name = name;
}
}