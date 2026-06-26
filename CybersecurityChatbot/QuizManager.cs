using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    class QuizManager
    {
      
            private List<QuizQuestion> _questions;
            private int _currentIndex = 0;
            private int _score = 0;

            public QuizManager()
            {
                PopulateQuestions();
            }

            public QuizQuestion GetCurrentQuestion()
            {
                if (IsFinished()) return null;
                return _questions[_currentIndex];
            }

            public bool SubmitAnswer(string answer)
            {
                var current = GetCurrentQuestion();
                if (current == null) return false;

                bool isCorrect = answer.Trim().Equals(current.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase);

                if (isCorrect)
                {
                    _score++;
                }

                _currentIndex++;
                return isCorrect;
            }

            public string GetFeedback(bool correct)
            {
                // Rewind slightly to get the explanation for the question just answered
                int activeIndex = _currentIndex - 1;
                if (activeIndex < 0 || activeIndex >= _questions.Count) return string.Empty;

                string status = correct ? "Correct!" : "Incorrect.";
                return $"{status} {_questions[activeIndex].Explanation}";
            }

            public bool IsFinished()
            {
                return _currentIndex >= _questions.Count;
            }

            public string GetFinalScore()
            {
                return $"{_score} / {_questions.Count}";
            }

            public string GetFinalMessage()
            {
                double percentage = ((double)_score / _questions.Count) * 100;
                return percentage >= 75 ? "Great Job! Expert Defender!" : "Keep learning and stay vigilant!";
            }

            public void ResetQuiz()
            {
                _currentIndex = 0;
                _score = 0;
            }

            private void PopulateQuestions()
            {
                _questions = new List<QuizQuestion>
            {
                // 1. Phishing (Multiple Choice)
                new QuizQuestion {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with password", "Delete the email", "Report email as phishing", "Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Reporting phishing emails helps security teams block future attacks.",
                    IsTrueFalse = false
                },
                new QuizQuestion {
                    Question = "An email from 'support@paypa1-security.com' states your account is locked. This is likely:",
                    Options = new List<string> { "Official support notice", "A typo by PayPal", "A phishing attempt using typosquatting", "A system test" },
                    CorrectAnswer = "C",
                    Explanation = "Look closely at domains; slight alterations mimic real brands to steal credentials.",
                    IsTrueFalse = false
                },
                // 2. Password Safety (True/False & Multiple Choice)
                new QuizQuestion {
                    Question = "Reusing the same strong master password across multiple systems is perfectly safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "If one site suffers a breach, credentials can be credential-stuffed into other profiles.",
                    IsTrueFalse = true
                },
                new QuizQuestion {
                    Question = "Which of the following creates the most resilient password configuration?",
                    Options = new List<string> { "Your birth year + name", "A long passphrase of random words", "P@ssword123!", "Changing a letter to a number" },
                    CorrectAnswer = "B",
                    Explanation = "Length beats complexity; a long sequence of words is difficult for computers to brute-force.",
                    IsTrueFalse = false
                },
                // 3. Safe Browsing (Multiple Choice)
                new QuizQuestion {
                    Question = "What does the HTTPS 'S' prefix extension represent on web locations?",
                    Options = new List<string> { "Secure (encrypted layout connection)", "Speed acceleration protocol", "Standard compliance", "Server routing" },
                    CorrectAnswer = "A",
                    Explanation = "HTTPS encrypts the data package in transit between your browser client and servers.",
                    IsTrueFalse = false
                },
                new QuizQuestion {
                    Question = "When accessing financial portals over open public Wi-Fi networks, you should:",
                    Options = new List<string> { "Trust the provider network", "Use a Virtual Private Network (VPN)", "Turn off anti-malware", "Clear browser tabs" },
                    CorrectAnswer = "B",
                    Explanation = "Public hot-spots expose plain data packets to interception via man-in-the-middle exploits.",
                    IsTrueFalse = false
                },
                // 4. Social Engineering (True/False)
                new QuizQuestion {
                    Question = "A phone agent claiming to be IT support asking for remote access is a common social engineering tactic.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Vishing actors use high-pressure phone authority tactics to install access entry implants.",
                    IsTrueFalse = true
                },
                new QuizQuestion {
                    Question = "Social engineering entirely targets network software flaws rather than human psychology.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Social engineering exploits psychological biases like fear, urgency, and trust.",
                    IsTrueFalse = true
                },
                // 5. Two-Factor Authentication (Multiple Choice)
                new QuizQuestion {
                    Question = "Which 2FA variant offers the highest structural resilience against intercept manipulation?",
                    Options = new List<string> { "SMS text messaging codes", "Email delivery links", "Hardware security keys (FIDO2)", "Voice validation calls" },
                    CorrectAnswer = "C",
                    Explanation = "Hardware keys physical binding blocks bypass network redirection and SIM-swapping hacks.",
                    IsTrueFalse = false
                },
                new QuizQuestion {
                    Question = "If you receive a 2FA prompt you did not request, it implies:",
                    Options = new List<string> { "A random app bug", "Your password is compromised", "Your session timed out", "An automated update" },
                    CorrectAnswer = "B",
                    Explanation = "An unauthorized prompt indicates an actor successfully input your password credentials.",
                    IsTrueFalse = false
                },
                // 6. Malware and Ransomware (True/False)
                new QuizQuestion {
                    Question = "Ransomware targets data visibility configurations by encrypting locally stored file directories.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Ransomware holds operating systems hostage by encrypting data and demanding payments.",
                    IsTrueFalse = true
                },
                new QuizQuestion {
                    Question = "Anti-virus platforms do not require update definitions once installed properly.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "New malicious signatures emerge daily; systems must consistently update defenses.",
                    IsTrueFalse = true
                },
                // 7. Privacy Settings (Multiple Choice)
                new QuizQuestion {
                    Question = "Sharing details like pet names or high schools publicly on social platforms risks compromising:",
                    Options = new List<string> { "Network bandwidth", "Account security questions", "Data storage quotas", "Browser cache" },
                    CorrectAnswer = "B",
                    Explanation = "Threat actors gather public profile footprints to bypass identity verification questions.",
                    IsTrueFalse = false
                },
                new QuizQuestion {
                    Question = "App permissions requesting access to contact maps or geolocations should be:",
                    Options = new List<string> { "Approved globally", "Limited strictly to necessary features", "Ignored entirely", "Enabled via admin modes" },
                    CorrectAnswer = "B",
                    Explanation = "Minimize attack footprint vectors by auditing and restricting high-risk phone tool accesses.",
                    IsTrueFalse = false
                },
                // 8. Data Backup (Multiple Choice)
                new QuizQuestion {
                    Question = "According to the industry 3-2-1 backup strategy, you need:",
                    Options = new List<string> { "3 copies, 2 different media, 1 offsite storage", "3 offsite storages, 2 clouds, 1 drive", "3 updates every 2 weeks on 1 network", "3 server arrays inside 1 room" },
                    CorrectAnswer = "A",
                    Explanation = "Diversifying structural formats ensures data stays resilient against physical fires or localized malware.",
                    IsTrueFalse = false
                },
                new QuizQuestion {
                    Question = "Keeping a local backup disk constantly mounted and connected to your production terminal is:",
                    Options = new List { "Ideal for system speed", "Risky, as ransomware can encrypt it too", "Safe from all malware", "Recommended for security" },
                    CorrectAnswer = "B",
                    Explanation = "Connected peripheral storage leaves recovery nodes vulnerable to replication infections.",
                    IsTrueFalse = false
                }
                };
            }
        }
    }




