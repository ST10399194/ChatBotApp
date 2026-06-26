using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public static class ActivityLogger
    {
        private static readonly List<string> _logs = new List<string>();

        public static void Log(string action)
        {
            _logs.Add(action);
        }

        public static string GetRecentLog()
        {
            if (_logs.Count == 0)
                return "No recent actions recorded yet.";

            StringBuilder sb = new StringBuilder("Here's a summary of recent actions:\n");
            for (int i = 0; i < _logs.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {_logs[i]}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
