using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
      public class QuizQuestion
        {
            public string Question { get; set; }
            public List<string> Options { get; set; } // A, B, C, D for multiple choice
            public string CorrectAnswer { get; set; }  // e.g., 'C' or 'True'
            public string Explanation { get; set; }   // shown after answering
            public bool IsTrueFalse { get; set; }      // differentiates display mode
        }
    }



