using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;

        // Constructor: Initializes the backend storage helper unit
        public TaskManager()
        {
            _storage = new TaskStorageHelper();
        }

        // Passes new task details to storage, logs the event, and returns a confirmation string
        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);

            // Log action to your project's activity logger
            ActivityLogger.Log($"Added task: {title}");

            return $"Successfully added task: '{title}'";
        }

        // Retrieves the complete collection of stored cyber tasks
        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        // Marks a specific task complete by its ID and logs the modification
        public void MarkAsComplete(int id)
        {
            _storage.MarkAsComplete(id);

            // Log action to your project's activity logger
            ActivityLogger.Log($"Marked task ID {id} as complete");
        }

        // Removes a task entirely from storage using its ID and logs the deletion
        public void DeleteTask(int id)
        {
            _storage.DeleteTask(id);

            // Log action to your project's activity logger
            ActivityLogger.Log($"Deleted task ID {id}");
        }
    }





