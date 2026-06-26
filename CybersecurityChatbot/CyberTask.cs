using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class CyberTask
    {
        public int Id {get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CreatedAt { get; set; } 
    }
}
