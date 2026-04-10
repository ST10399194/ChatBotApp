using System;
using System.IO;
using System.Media;

namespace ChatBotApp
{
    public class AudioPlayer
    {
        //plays audio file
        public static void PlayGreeting(string filePath = "greeting.wav")
        {
            //checks if the sudio file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"(Audio file not found: {filePath})");
                return; //exit early if the file is missing
            }

            try
            {
            // create a soundplayer instance for the given file
                using var player = new SoundPlayer(filePath);
                player.PlaySync(); 
            }
            catch (InvalidOperationException ex)
            {
                //handles errors if the audio desnt play
                Console.WriteLine($"Audio error: {ex.Message}");
            }
        }
    }
}