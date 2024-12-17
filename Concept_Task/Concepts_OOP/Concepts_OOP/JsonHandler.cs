using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts_OOP
{
    public class JsonHandler
    {
        private readonly string _filePath;

        public JsonHandler(string filePath)
        {
            _filePath = filePath;
        }

        // Method to write tasks to JSON and save to file
        public void WriteToJson(Task newTask)
        {
            // Load existing tasks from the file
            List<Task> existingTasks = LoadFromJson();

            // Add new tasks to the existing ones, avoiding duplicates

            if (!existingTasks.Exists(task => task.Id == newTask.Id))
            {
                existingTasks.Add(newTask);
            }


            // Serialize and write to file
            string jsonData = JsonConvert.SerializeObject(existingTasks, Formatting.Indented);

            try
            {
                File.WriteAllText(_filePath, jsonData);
                Console.WriteLine("Tasks successfully saved to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to file: {ex.Message}");
            }
        }

        // Method to load tasks from JSON
        public List<Task> LoadFromJson()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File not found.");
                return new List<Task>();
            }

            string jsonData = File.ReadAllText(_filePath);


            return JsonConvert.DeserializeObject<List<Task>>(jsonData) ?? new List<Task>();


        }
        public void OverwriteJson(List<Task> tasks)
        {
          
        // Serialize the updated list and write it back to the JSON file
        string jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(_filePath, jsonData);
        }
    }
}