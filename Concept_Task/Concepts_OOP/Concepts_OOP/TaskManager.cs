using Concepts_OOP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts_OOP
{
  
    public class TaskManager
    {
        private Task[] taskArray = new Task[0];
        private int count = 0;
        private string filePath = @"C:\Users\HP\source\repos\Concepts_OOP\Concepts_OOP\tasks.json";

        // Declare jsonHandler without initialization here
        private JsonHandler jsonHandler;

        // Constructor to initialize jsonHandler
        public TaskManager()
        {
            jsonHandler = new JsonHandler(filePath);
        }


      //------------------
       public void AddTask(Task task)
 {
     int count = 0;
     foreach (var _ in taskArray)
     {
         count++;
     }

     int SizeOfGetLength = count;

     if (this.count == SizeOfGetLength)
     {
         int newSize = SizeOfGetLength == 0 ? 1 : SizeOfGetLength * 2;
         Task[] newArray = new Task[newSize];
         for (int i = 0; i < this.count; i++)
         {
             newArray[i] = taskArray[i];
         }
         taskArray = newArray;
     }

     taskArray[this.count] = task;
     this.count++;

     jsonHandler.WriteToJson(task);
 }

 ///update function
// New UpdateTask method
 public bool UpdateTask(int id, int? newPriority = null, string newStatus = null)
 {
     List<Task> loadedTasks = jsonHandler.LoadFromJson();
     Task taskToUpdate = loadedTasks.FirstOrDefault(t => t.Id == id);

     if (taskToUpdate == null)
     {
         Console.WriteLine($"Task with ID {id} not found.");
         return false;
     }

     if (newPriority.HasValue)
     {
         taskToUpdate.Priority = newPriority.Value;
         Console.WriteLine($"Updated Task ID {id}'s Priority to {newPriority.Value}");
     }

     if (!string.IsNullOrWhiteSpace(newStatus))
     {
         taskToUpdate.Status = newStatus;
         Console.WriteLine($"Updated Task ID {id}'s Status to {newStatus}");
     }

     // Save the updated list back to the JSON file
     jsonHandler.OverwriteJson(loadedTasks);

     return true;
 }
 //------------------------------------------------------------------------
 public bool RemoveTaskById(int id)
 {
     List<Task> loadedTasks = jsonHandler.LoadFromJson();
     int count = 0;
     foreach (var task in loadedTasks)
     {
         if (task != null)
             count++;
     }

     int indexToRemove = -1;
     for (int i = 0; i < count; i++)
     {
         if (loadedTasks[i].Id == id)
         {
             indexToRemove = i;
             break;
         }
     }

     // If the task is found, remove it manually
     if (indexToRemove != -1)
     {
         loadedTasks = RemoveAtCustom(loadedTasks, indexToRemove);

         jsonHandler.OverwriteJson(loadedTasks);

         Console.WriteLine($"Task with ID {id} has been removed.");
         return true;
     }
     else
     {
         Console.WriteLine($"Task with ID {id} not found.");
         return false;
     }
 }


 // Custom function to remove an item from a list at a specific index
 private List<Task> RemoveAtCustom(List<Task> tasks, int indexToRemove)
 {
     List<Task> newList = new List<Task>();

     int count = 0;
     foreach (var task in tasks)
     {
         if (task != null)
             count++;
     }

     for (int i = 0; i < count; i++)
     {
         if (i != indexToRemove)
         {
             newList.Add(tasks[i]);
         }
     }

     return newList;
 }
 //load from file



      //-------------------


        public Task[] LoadedTasks()
        {
            List<Task> loadedTasks = jsonHandler.LoadFromJson();
            Task[] tasks = loadedTasks.ToArray();
            int count = 0;
            foreach (var _ in taskArray)
            {
                count++;
            }

            int SizeOfGetLength = count;
            if (tasks == null)
                tasks = taskArray;

            return tasks;
        }

        public void DisplayTasks(Task[] tasks)
        {
            if (tasks == null)
            {
                Console.WriteLine("No tasks to display.");
                return;
            }

            int size = 0;

            // Calculate size of tasks manually
            foreach (var _ in tasks)
            {
                size++;
            }

            for (int i = 0; i < size; i++)
            {
                if (tasks[i] != null)
                {
                    Console.WriteLine($"Task ID: {tasks[i].Id}, Description: {tasks[i].Description}, Due Date: {tasks[i].DueDate.FormattedDate}, Priority: {tasks[i].Priority}, Status: {tasks[i].Status}");
                }
            }
        }


        public Task[] SortByPriority()
        {
            Task[] loadedTasks = LoadedTasks();
            int size = 0;

            // Calculate the size of loadedTasks manually
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            if (size == 0) return new Task[0]; // Return an empty array if no tasks

            Task[] sortedTasks = new Task[size];

            // Copy loadedTasks into sortedTasks
            for (int i = 0; i < size; i++)
            {
                sortedTasks[i] = loadedTasks[i];
            }

            // Sort tasks by priority using bubble sort
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (sortedTasks[i].Priority > sortedTasks[j].Priority)
                    {
                        var temp = sortedTasks[i];
                        sortedTasks[i] = sortedTasks[j];
                        sortedTasks[j] = temp;
                    }
                }
            }

            return sortedTasks;
        }


        public Task[] SortByDueDate()
        {
            Task[] loadedTasks = LoadedTasks();
            int size = 0;

            // Calculate the size of loadedTasks manually
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            if (size == 0) return new Task[0]; // Return an empty array if no tasks

            Task[] sortedList = new Task[size]; // Initialize sortedList
            for (int i = 0; i < size; i++)
            {
                sortedList[i] = loadedTasks[i]; // Copy loadedTasks into sortedList
            }

            // Sort tasks by due date using bubble sort
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (sortedList[i].DueDate.Year > sortedList[j].DueDate.Year ||
                        (sortedList[i].DueDate.Year == sortedList[j].DueDate.Year && sortedList[i].DueDate.Month > sortedList[j].DueDate.Month) ||
                        (sortedList[i].DueDate.Year == sortedList[j].DueDate.Year && sortedList[i].DueDate.Month == sortedList[j].DueDate.Month && sortedList[i].DueDate.Day > sortedList[j].DueDate.Day))
                    {
                        var temp = sortedList[i];
                        sortedList[i] = sortedList[j];
                        sortedList[j] = temp;
                    }
                }
            }

            return sortedList;
        }

        public Task[] FilterByStatus(string status)
        {
            Task[] loadedTasks = LoadedTasks(); // Load tasks from JSON
            int size = 0;

            // Calculate the size of loadedTasks manually
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            Task[] filteredList = new Task[size];
            int filteredCount = 0;

            for (int i = 0; i < size; i++)
            {
                if (loadedTasks[i].Status == status)
                {
                    filteredList[filteredCount++] = loadedTasks[i];
                }
            }

            Task[] resizedArray = new Task[filteredCount];
            for (int i = 0; i < filteredCount; i++)
            {
                resizedArray[i] = filteredList[i];
            }

            return resizedArray;
        }



        public Task[] FilterByPriority(int priority)
        {
            Task[] loadedTasks = LoadedTasks(); // Load tasks from JSON
            int size = 0;

            // Calculate the size of loadedTasks manually
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            Task[] filteredList = new Task[size];
            int filteredCount = 0;

            for (int i = 0; i < size; i++)
            {
                if (loadedTasks[i].Priority == priority)
                {
                    filteredList[filteredCount++] = loadedTasks[i];
                }
            }

            Task[] resizedArray = new Task[filteredCount];
            for (int i = 0; i < filteredCount; i++)
            {
                resizedArray[i] = filteredList[i];
            }

            return resizedArray;
        }



        public Task[] FilterByDueDate(DateTimeRetrieval dueDate)
        {
            Task[] loadedTasks = LoadedTasks(); // Load tasks from JSON
            int size = 0;

            // Calculate the size of loadedTasks manually
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            Task[] filteredList = new Task[size];
            int filteredCount = 0;

            for (int i = 0; i < size; i++)
            {
                // Directly compare date components
                if (loadedTasks[i].DueDate.Year == dueDate.Year &&
                    loadedTasks[i].DueDate.Month == dueDate.Month &&
                    loadedTasks[i].DueDate.Day == dueDate.Day)
                {
                    filteredList[filteredCount++] = loadedTasks[i];
                }
            }

            // Resize the filtered list to the exact number of matching tasks
            Task[] resizedArray = new Task[filteredCount];
            for (int i = 0; i < filteredCount; i++)
            {
                resizedArray[i] = filteredList[i];
            }

            return resizedArray;
        }




        public Task[] HighlightNearingDeadlines(DateTimeRetrieval currentDate)
        {
            Task[] loadedTasks = LoadedTasks(); // Load tasks from JSON
            int size = 0;

            // Calculate the size of loadedTasks dynamically
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            Task[] filteredList = new Task[size];
            int filteredCount = 0;

            for (int i = 0; i < size; i++)
            {
                // Compare task due dates with the current date
                if (loadedTasks[i].DueDate.Year > currentDate.Year ||
                    (loadedTasks[i].DueDate.Year == currentDate.Year && loadedTasks[i].DueDate.Month > currentDate.Month) ||
                    (loadedTasks[i].DueDate.Year == currentDate.Year && loadedTasks[i].DueDate.Month == currentDate.Month && loadedTasks[i].DueDate.Day >= currentDate.Day))
                {
                    filteredList[filteredCount++] = loadedTasks[i];
                }
            }

            // Resize the filtered list to the exact number of matching tasks
            Task[] resizedArray = new Task[filteredCount];
            for (int i = 0; i < filteredCount; i++)
            {
                resizedArray[i] = filteredList[i];
            }

            return resizedArray;
        }



        public Task[] OverDueDeadlines(DateTimeRetrieval currentDate)
        {
            Task[] loadedTasks = LoadedTasks(); // Load tasks from JSON
            int size = 0;

            // Calculate the size of loadedTasks dynamically
            foreach (var _ in loadedTasks)
            {
                size++;
            }

            Task[] filteredList = new Task[size];
            int filteredCount = 0;

            for (int i = 0; i < size; i++)
            {
                // Compare task due dates with the current date
                if (loadedTasks[i].DueDate.Year < currentDate.Year ||
                    (loadedTasks[i].DueDate.Year == currentDate.Year && loadedTasks[i].DueDate.Month < currentDate.Month) ||
                    (loadedTasks[i].DueDate.Year == currentDate.Year && loadedTasks[i].DueDate.Month == currentDate.Month && loadedTasks[i].DueDate.Day < currentDate.Day))
                {
                    filteredList[filteredCount++] = loadedTasks[i];
                }
            }

            // Resize the filtered list to the exact number of matching tasks
            Task[] resizedArray = new Task[filteredCount];
            for (int i = 0; i < filteredCount; i++)
            {
                resizedArray[i] = filteredList[i];
            }

            return resizedArray;
        }

    }

}
