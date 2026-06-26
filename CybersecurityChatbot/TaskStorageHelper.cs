using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CybersecurityChatbot
{
    public class TaskStorageHelper
    {
        Private const string FilePath = “tasks.json”;


        // Reads the storage file and parses it into a list of tasks
        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<CyberTask>();
                }

                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<List<CyberTask>>(json) ?? new List<CyberTask>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                return new List<CyberTask>();
            }
        }

        // Converts the task list into JSON formatting and writes it to the disk
        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        // Assigns a unique ID, builds a new task object, and saves it to the file
        public void AddTask(string title, string description, string reminder)
        {
            List<CyberTask> tasks = LoadTasks();

            int nextId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;

            CyberTask newTask = new CyberTask
            {
                Id = nextId,
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            tasks.Add(newTask);
            SaveTasks(tasks);
        }

        // Finds a task by its ID and updates its completion status to true
        public void MarkAsComplete(int id)
        {
            List<CyberTask> tasks = LoadTasks();
            CyberTask taskToComplete = tasks.FirstOrDefault(t => t.Id == id);

            if (taskToComplete != null)
            {
                taskToComplete.IsComplete = true;
                SaveTasks(tasks);
            }
        }

        // Searches for a task matching the ID and deletes it from storage
        public void DeleteTask(int id)
        {
            List<CyberTask> tasks = LoadTasks();
            CyberTask taskToDelete = tasks.FirstOrDefault(t => t.Id == id);

            if (taskToDelete != null)
            {
                tasks.Remove(taskToDelete);
                SaveTasks(tasks);
            }
        }
     }
    







