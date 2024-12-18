using Concepts_OOP;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Task = Concepts_OOP.Task;
namespace Concepts_OOP;
using System;


class Program
{
    private static Task[] taskArray = new Task[0]; // Array to hold tasks
    private static int count = 0; // Task count

    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();
        JsonHandler jsonHandler = new JsonHandler("tasks.json");

        bool isRunning = true;

        while (isRunning)
        {
            // Displaying Menu
            Console.WriteLine("\n===== Task Manager =====");
            Console.WriteLine("1. Add a Task");
            Console.WriteLine("2. Delete a Task");
            Console.WriteLine("3. Update a Task");
            Console.WriteLine("4. Sort Tasks");
            Console.WriteLine("5. Filter Tasks");
            Console.WriteLine("6. Highlight Nearing Deadlines");
            Console.WriteLine("7. Show Overdue Tasks");
            Console.WriteLine("8. Display All Tasks");
            Console.WriteLine("9. Exit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask(taskManager);
                    break;
                case "2":
                    DeleteTask(taskManager);
                    break;
                case "3":
                    UpdateTask(taskManager);
                    break;
                case "4":
                    SortTasks(taskManager);
                    break;
                case "5":
                    FilterTasks(taskManager);
                    break;
                case "6":
                    HighlightNearingDeadlines(taskManager);
                    break;
                case "7":
                    ShowOverdueTasks(taskManager);
                    break;
                case "8":
                    DisplayAllTasks(taskManager);
                    break;
                case "9":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    static void AddTask(TaskManager taskManager)
    {
        Console.WriteLine("\nEnter Task Details:");

        Console.Write("Task ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Description: ");
        string description = Console.ReadLine();
        //------------------------------------------

        //Console.Write("Enter Due Date (dd mm yyyy): ");
        //string[] dateParts = Console.ReadLine().Split();
        //var dueDate = new DateTimeRetrieval(int.Parse(dateParts[2]), int.Parse(dateParts[1]), int.Parse(dateParts[0]));

        DateTimeRetrieval dueDate;
        while (true)
        {
            Console.Write("Enter Due Date (mm dd yyyy): ");
            string inputDate = Console.ReadLine();
            string[] dateParts = inputDate.Split();

            if (dateParts.Length == 3
                && int.TryParse(dateParts[0], out int month)
                && int.TryParse(dateParts[1], out int day)
                && int.TryParse(dateParts[2], out int year))
            {
                try
                {
                    // Attempt to create a valid DateTimeRetrieval object
                    dueDate = new DateTimeRetrieval(month, day, year);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Invalid date: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please ensure you enter a date in 'mm dd yyyy' format.");
            }
        }

        //////////----------------------

        Console.Write("Priority (1-5): ");
        int priority = int.Parse(Console.ReadLine());

        Task newTask = new Task(id, description, dueDate, priority, "Pending");
        taskManager.AddTask(newTask);

        Console.WriteLine("Task Added Successfully!");
    }

    static void DisplayAllTasks(TaskManager taskManager)
    {
        Console.WriteLine("===== All Tasks =====");
        Task[] loadedTasks = taskManager.LoadedTasks();
        if (loadedTasks.Length == 0)
        {
            Console.WriteLine("No tasks available.");
        }
        else
        {
            taskManager.DisplayTasks(loadedTasks);
        }
    }

    //remove by id
    static void DeleteTask(TaskManager taskManager)
    {
        Console.Write("\nEnter Task ID to Delete: ");
        int id = int.Parse(Console.ReadLine());

        bool success = taskManager.RemoveTaskById(id);
        if (success)
        {
            Console.WriteLine($"Task with ID {id} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Task with ID {id} not found.");
        }
    }

    //update by status and priority
    static void UpdateTask(TaskManager taskManager)
    {

        Console.Write("\nEnter Task ID to Update: ");
        int id = int.Parse(Console.ReadLine());

        Console.WriteLine("What would you like to update?");
        Console.WriteLine("1. Priority");
        Console.WriteLine("2. Status");
        Console.WriteLine("Choose an option: ");
        int updateChoice = int.Parse(Console.ReadLine());

        if (updateChoice == 1)
        {
            Console.Write("Enter new Priority (1-5): ");
            int newPriority = int.Parse(Console.ReadLine());
            bool success = taskManager.UpdateTask(id, newPriority);

            if (success)
                Console.WriteLine("Priority updated successfully.");
            else
                Console.WriteLine("Task ID not found.");
        }
        else if (updateChoice == 2)
        {
            Console.WriteLine("Enter new Status: ");
            Console.WriteLine("1. Completed");
            Console.WriteLine("2. Overdue");
            Console.WriteLine("Choose an option: ");
            int StatusChoice = int.Parse(Console.ReadLine());
            string newStatus = string.Empty;

            if (StatusChoice == 1)
            {
                newStatus = "Completed";
            }
            else if (StatusChoice == 2)
            {
                newStatus = "Overdue";
            }

            bool success = taskManager.UpdateTask(id, null, newStatus);

            if (success)
                Console.WriteLine("Status updated successfully.");
            else
                Console.WriteLine($"Task ID {id} not found.");
        }
        else
        {
            Console.WriteLine("Invalid choice. Returning to main menu.");
        }
    }

    // Sorting tasks by priority and due date
    static void SortTasks(TaskManager taskManager)
    {
        Console.WriteLine("\nChoose sorting option:");
        Console.WriteLine("1. Sort by Priority");
        Console.WriteLine("2. Sort by Due Date");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine();

        if (option == "1")
        {
            var sortedByPriority = taskManager.SortByPriority();
            taskManager.DisplayTasks(sortedByPriority);
        }
        else if (option == "2")
        {
            var sortedByDueDate = taskManager.SortByDueDate();
            taskManager.DisplayTasks(sortedByDueDate);
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }

    // Filtering tasks by various parameters
    static void FilterTasks(TaskManager taskManager)
    {
        Console.WriteLine("\nChoose filtering option:");
        Console.WriteLine("1. Filter by Status");
        Console.WriteLine("2. Filter by Priority");
        Console.WriteLine("3. Filter by Due Date");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine();

        if (option == "1")
        {
            Console.Write("\nEnter Status to Filter: ");
            string status = Console.ReadLine();
            var filteredByStatus = taskManager.FilterByStatus(status);
            taskManager.DisplayTasks(filteredByStatus);
        }
        else if (option == "2")
        {
            Console.Write("\nEnter Priority to Filter (1-5): ");
            int priority = int.Parse(Console.ReadLine());
            var filteredByPriority = taskManager.FilterByPriority(priority);
            taskManager.DisplayTasks(filteredByPriority);
        }
        else if (option == "3")
        {
            Console.Write("\nEnter Due Date to Filter (mm dd yyyy): ");
            string[] dateParts = Console.ReadLine().Split();
            var dueDate = new DateTimeRetrieval(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]));
            var filteredByDueDate = taskManager.FilterByDueDate(dueDate);
            taskManager.DisplayTasks(filteredByDueDate);
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }

    // Highlight tasks nearing deadlines
    static void HighlightNearingDeadlines(TaskManager taskManager)
    {
        Console.Write("\nEnter Today's Date (mm dd yyyy) for near-deadline tasks: ");
        string[] dateParts = Console.ReadLine().Split();
        var today = new DateTimeRetrieval(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]));

        var highlighted = taskManager.HighlightNearingDeadlines(today);
        taskManager.DisplayTasks(highlighted);
    }

    // Display overdue tasks
    static void ShowOverdueTasks(TaskManager taskManager)
    {
        Console.Write("\nEnter Today's Date (mm dd yyyy) for overdue tasks: ");
        string[] dateParts = Console.ReadLine().Split();
        var today = new DateTimeRetrieval(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]));

        var overdue = taskManager.OverDueDeadlines(today);
        taskManager.DisplayTasks(overdue);
    }


}


