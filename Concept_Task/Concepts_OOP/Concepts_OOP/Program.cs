using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Concepts_OOP;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Concepts_OOP;
using Task = Concepts_OOP.Task;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();

        // Manually add tasks
        taskManager.AddTask(new Task(1, "Complete first class", new DateTimeRetrieval(2024, 12, 1), 3, "Pending"));
        taskManager.AddTask(new Task(2, "Complete second class", new DateTimeRetrieval(2023, 6, 17), 1, "Pending"));
        taskManager.AddTask(new Task(3, "Complete assignment", new DateTimeRetrieval(2024, 12, 18), 6, "Pending"));
        taskManager.AddTask(new Task(4, "Complete assignment", new DateTimeRetrieval(2024, 2, 19), 3, "Overdue"));
        taskManager.AddTask(new Task(5, "Complete assignment", new DateTimeRetrieval(2024, 3, 2), 1, "Completed"));

        Console.WriteLine("Load All Tasks:");
        Task[] loadedTasks = taskManager.LoadedTasks();
        taskManager.DisplayTasks(loadedTasks);

        //taskManager.Update("Completed", 1);

        Console.WriteLine("\nDeleting Task with ID 3...");
        taskManager.RemoveTaskById(3);
        loadedTasks = taskManager.LoadedTasks();
        taskManager.DisplayTasks(loadedTasks);

        // Sorting tasks
        Console.WriteLine("\nSorted by Priority:");
        var sortedByPriority = taskManager.SortByPriority();
        taskManager.DisplayTasks(sortedByPriority);

        //Sorted dueDate
        Console.WriteLine("\nSorted by Due Date:");
        var sortedByDueDate = taskManager.SortByDueDate();
        taskManager.DisplayTasks(sortedByDueDate);

        // Deleting a task
        Console.WriteLine("\nDeleting Task with ID 2...");
        taskManager.RemoveTaskById(2);
        loadedTasks = taskManager.LoadedTasks();
        taskManager.DisplayTasks(loadedTasks);

        // Filtering tasks by status
        Console.WriteLine("\nFiltered by Status (Completed):");
        var filteredByStatus = taskManager.FilterByStatus("Completed");
        taskManager.DisplayTasks(filteredByStatus);


        // Filtering tasks by prority
        Console.WriteLine("\nFiltered by Priority 1:");
        var filteredByPriority = taskManager.FilterByPriority(1);
        taskManager.DisplayTasks(filteredByPriority);

        // Filtering tasks by dueDate
        Console.WriteLine("\nFiltered by DueDate 2024/12/1:");
        var filteredByDueDate = taskManager.FilterByDueDate(new DateTimeRetrieval(2024, 12, 1));
        taskManager.DisplayTasks(filteredByDueDate);

        //Notification Tasks Nearing Deadlines
        Console.WriteLine("\nNotification Tasks Nearing Deadlines:");
        var highlighted = taskManager.HighlightNearingDeadlines(new DateTimeRetrieval(2024, 12, 1));
        taskManager.DisplayTasks(highlighted);

        //Notification Overdue Tasks
        Console.WriteLine("\nNotification Overdue Tasks:");
        var overdue = taskManager.OverDueDeadlines(new DateTimeRetrieval(2024, 12, 1));
        taskManager.DisplayTasks(overdue);

        Console.ReadLine();
    }
}









