using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Concepts_Functional
{
    public record Task(
        int Id,
        string Description,
        DateTime DueDate,
        int Priority,
        string Status
    );

    public class Program
    {
        public static void Main(string[] args)
        {


            string filePath = @"C:\Users\HP\source\repos\Concepts_Functional\Concepts_Functional\tasksFunctional.json";

            Task task1 = new Task(1, "Task 1", new DateTime(2024, 12, 10), 1, "Not Started");
            Task task2 = new Task(2, "Task 2", new DateTime(2024, 12, 15), 2, "In Progress");
            Task task3 = new Task(3, "Task 3", new DateTime(2024, 12, 5), 3, "Completed");


            // Create initial tasks array
            Task[] tasks = new Task[] { task1, task2, task3 };

            //save task 
            Console.WriteLine("created tasks");
            SaveToJson(filePath, tasks);
            // Display the original list of tasks
            Console.WriteLine("Original Tasks:");
            DisplayTasks(tasks);

            //file handling 1
            Console.WriteLine("\nTasks Before functions:");
            var tasksBeforeFunction = LoadFromJson(filePath);
            DisplayTasks(tasksBeforeFunction);

            // Add a new task and save
            Task newTask = new Task(4, "New Task 4", new DateTime(2024, 12, 25), 1, "Not Started");



            tasks = AddTask(tasks, newTask);
            Console.WriteLine("\nAdding a New Task and save it :");
            SaveToJson(filePath, tasks);
            Console.WriteLine("\nAfter Adding a New Task:");
            DisplayTasks(tasks);

            // Sort tasks by priority
            tasks = SortByPriority(tasks);
            Console.WriteLine("\nAfter Sorting by Priority:");
            DisplayTasks(tasks);

            // Sort tasks by due date
            tasks = SortByDueDate(tasks);
            Console.WriteLine("\nAfter Sorting by Due Date:");
            DisplayTasks(tasks);

            // Filter tasks by status
            var filteredTasksByStatus = FilterByStatus(tasks, "Not Started");
            Console.WriteLine("\nTasks with Status 'Not Started':");
            DisplayTasks(filteredTasksByStatus);

            // Filter tasks by priority
            var filteredTasksByPriority = FilterByPriority(tasks, 2);
            Console.WriteLine("\nTasks with Priority 2:");
            DisplayTasks(filteredTasksByPriority);

            // Filter tasks by due date
            var filteredTasksByDueDate = FilterByDueDate(tasks, new DateTime(2024, 12, 15));
            Console.WriteLine("\nTasks Due on 12/15/2024:");
            DisplayTasks(filteredTasksByDueDate);

            // Highlight tasks nearing deadlines (tasks with due date greater or equal to today's date)
            var nearingDeadlines = HighlightNearingDeadlines(tasks, DateTime.Now);
            Console.WriteLine("\nTasks with Nearing Deadlines of Today date:");
            DisplayTasks(nearingDeadlines);

            // Show overdue tasks (tasks with due date before today's date)
            var overdueTasks = OverDueDeadlines(tasks, DateTime.Now);
            Console.WriteLine("\nOverdue Tasks:");
            DisplayTasks(overdueTasks);

            // Delete a task by ID
            tasks = DeleteTaskById(tasks, 2);
            Console.WriteLine("\nAfter Deleting Task with ID 2:");
            DisplayTasks(tasks);

        }


        // Recursive method to get the length of the array
        public static int GetArrayLength(Task[] tasks, int index = 0)
        {
            try
            {
                // Try accessing the current index
                var _ = tasks[index];
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
            return 1 + GetArrayLength(tasks, index + 1);
        }
        public static Task[] AddTask(Task[] tasks, Task newTask)
        {
            return AddTaskHelper(tasks, newTask, new Task[GetArrayLength(tasks) + 1], 0);
        }
        private static Task[] AddTaskHelper(Task[] tasks, Task newTask, Task[] newTasksArray, int index)
        {
            if (index == GetArrayLength(tasks))
            {
                // Add the new task to the end of the array
                newTasksArray[index] = newTask;
                return newTasksArray;
            }

            // Copy the current task and move to the next
            newTasksArray[index] = tasks[index];
            return AddTaskHelper(tasks, newTask, newTasksArray, index + 1);
        }

        // Method to display tasks recursively
        public static void DisplayTasks(Task[] tasks, int index = 0)
        {
            if (index == GetArrayLength(tasks))
            {
                return;
            }


            Console.WriteLine($"Task ID: {tasks[index].Id}, Description: {tasks[index].Description}, Due Date: {tasks[index].DueDate.ToString("MM/dd/yyyy")}, Priority: {tasks[index].Priority}, Status: {tasks[index].Status}");

            DisplayTasks(tasks, index + 1);
        }


        public static Task[] DeleteTaskById(Task[] tasks, int id, int index = 0, Task[] newArray = null)
        {
            // Initialize newArray on the first call if it's null
            if (newArray == null)
            {
                newArray = Array.Empty<Task>(); // Start with an empty array
            }

            if (index == GetArrayLength(tasks))
            {
                return newArray;
            }
            if (tasks[index].Id == id)
            {
                return DeleteTaskById(tasks, id, index + 1, newArray);
            }
            newArray = AddTask(newArray, tasks[index]);
            return DeleteTaskById(tasks, id, index + 1, newArray);
        }
        public static Task[] SortByPriority(Task[] tasks)
        {
            return SortHelper(tasks, tasks.Length, 0, tasks, (task1, task2) => task1.Priority > task2.Priority);
        }

        public static Task[] SortByDueDate(Task[] tasks)
        {
            return SortHelper(tasks, tasks.Length, 0, tasks, (task1, task2) => task1.DueDate.CompareTo(task2.DueDate) > 0);
        }

        private static Task[] SortHelper(Task[] tasks, int length, int currentIndex, Task[] sortedArray, Func<Task, Task, bool> compare)
        {
            if (currentIndex == length - 1 || length == 0)
            {
                return sortedArray; // Terminate when sorting is complete
            }
            return SortInnerHelper(sortedArray, length, currentIndex, currentIndex + 1, sortedArray, compare);
        }

        private static Task[] SortInnerHelper(Task[] tasks, int length, int i, int j, Task[] currentSorted, Func<Task, Task, bool> compare)
        {
            if (j == length)
            {
                return SortHelper(tasks, length, i + 1, currentSorted, compare); // Move to the next outer loop index
            }
            if (compare(currentSorted[i], currentSorted[j]))
            {
                currentSorted = SwapTasks(currentSorted, i, j); // Swap tasks if out of order
            }

            return SortInnerHelper(tasks, length, i, j + 1, currentSorted, compare); // Continue inner loop
        }

        private static Task[] SwapTasks(Task[] tasks, int i, int j)
        {
            Task temp = tasks[i];
            tasks[i] = tasks[j];
            tasks[j] = temp;
            return tasks;
        }


        private static bool IsDateGreater(DateTime date1, DateTime date2)
        {
            if (date1.Year > date2.Year) return true;
            if (date1.Year == date2.Year && date1.Month > date2.Month) return true;
            if (date1.Year == date2.Year && date1.Month == date2.Month && date1.Day > date2.Day) return true;

            return false;
        }

        public static Task[] FilterTasksHelper(Task[] tasks, Func<Task, bool> predicate, int length, int index, Task[] filteredTasks)
        {
            if (index == length) return filteredTasks;
            if (predicate(tasks[index]))
            {
                filteredTasks = AddTask(filteredTasks, tasks[index]);
            }
            return FilterTasksHelper(tasks, predicate, length, index + 1, filteredTasks);
        }

        public static Task[] FilterByStatus(Task[] tasks, string status)
        {
            return FilterTasksHelper(tasks, task => task.Status == status, GetArrayLength(tasks), 0, Array.Empty<Task>());
        }

        public static Task[] FilterByPriority(Task[] tasks, int priority)
        {
            return FilterTasksHelper(tasks, task => task.Priority == priority, GetArrayLength(tasks), 0, Array.Empty<Task>());
        }

        public static Task[] FilterByDueDate(Task[] tasks, DateTime dueDate)
        {
            return FilterTasksHelper(tasks, task =>
                task.DueDate.Year == dueDate.Year &&
                task.DueDate.Month == dueDate.Month &&
                task.DueDate.Day == dueDate.Day, GetArrayLength(tasks), 0, Array.Empty<Task>());
        }

        public static Task[] HighlightNearingDeadlines(Task[] tasks, DateTime currentDate)
        {
            // Using FilterTasksHelper directly with the predicate for nearing deadlines
            return FilterTasksHelper(tasks, task => task.DueDate.CompareTo(currentDate) >= 0, GetArrayLength(tasks), 0, Array.Empty<Task>());
        }

        public static Task[] OverDueDeadlines(Task[] tasks, DateTime currentDate)
        {
            // Using FilterTasksHelper directly with the predicate for overdue deadlines
            return FilterTasksHelper(tasks, task => task.DueDate.CompareTo(currentDate) < 0, GetArrayLength(tasks), 0, Array.Empty<Task>());
        }
       

        ///////////file handling
        public static Task[] LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at {filePath}. Returning an empty task list.");
                return Array.Empty<Task>();
            }

            try
            {
                var jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Task[]>(jsonData) ?? Array.Empty<Task>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks from file: {ex.Message}");
                return Array.Empty<Task>();
            }
        }

        public static void SaveToJson(string filePath, Task[] tasks)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine($"Tasks successfully saved in file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to file : {ex.Message}");
            }
        }

       

    }
}
