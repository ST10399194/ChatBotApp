using System;
using System.IO;
using System.Media;

namespace ChatBotApp
{
    public class AudioPlayer
    {
        public static void PlayGreeting(string filePath = "greeting.wav")
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"(Audio file not found: {filePath})");
                return;
            }

            try
            {
                using var player = new SoundPlayer(filePath);
                player.Play(); // Or player.Play() for async
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Audio error: {ex.Message}");
            }
        }
    }
}