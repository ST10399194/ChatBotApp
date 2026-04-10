using System;
using System.Threading;


namespace ChatBotApp;

public class UI
{
public static void DisplayAsciiArt()
{
// Set text color to dark green for the main ASCII banner
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    //prints the ASCII art
    Console.WriteLine(@"
                                                                                                                              
       _____    _____      _____       _____        ______        _____               _____          _____   _________________ 
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
    '   )/           '             '    '        '    )/       '     '            '    '          '    '         '            
");
 // Subtitle tagline — printed in dark cyan to complement the green banner
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("                         Your personal cybersecurity awareness companion.\n");
// Brief pause so the banner registers visually before the program continues
        Thread.Sleep(800);

        // Always reset color after styled output so nothing downstream is affected
        Console.ResetColor();
}
}