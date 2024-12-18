using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

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
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string filePath = Path.Combine(projectRoot, "tasksFunctional.json");
            InteractiveMenu(Array.Empty<Task>(), "C:\\Users\\HP\\source\\repos\\Concepts_Functional\\Concepts_Functional\\tasksFunctional.json");
        }
        

        public static void InteractiveMenu(Task[] tasks, string filePath)
        {
            Console.WriteLine("Select an operation: \n1. Add Task \n2. Delete Task \n3. Display Tasks \n4. Sort by Priority \n5. Sort by Due Date \n6. Filter by Status \n7. Filter by Priority \n8. Filter by Due Date \n9. Highlight Near Deadlines \n10. Show Overdue Deadlines \n11. Save to File \n12. Load from File \n13. Update Task \n14. Exit");
            var input = Console.ReadLine();
            HandleUserInput(tasks, filePath, input);
        }

        public static void HandleUserInput(Task[] tasks, string filePath, string input)
        {
            var updatedTasks = input switch
            {
                "1" => AddNewTask(tasks),
                "2" => DeleteTaskById(tasks, PromptInt("Enter task ID to delete: "), filePath),
                "3" => DisplayTasksAndReturn(tasks),
                "4" => SortByPriority(tasks),
                "5" => SortByDueDate(tasks),
                "6" => FilterByStatus(tasks, PromptStatusChoice("Enter status to filter by: ")),
                "7" => FilterByPriority(tasks, PromptInt("Enter priority to filter by: ")),
                "8" => FilterByDueDate(tasks, PromptDate("Enter due date to filter by (MM/dd/yyyy): ")),
                "9" => HighlightNearingDeadlines(tasks),
                "10" => OverDueDeadlines(tasks),
                "11" => SaveToFileAndReturn(tasks, filePath),
                "12" => LoadFromJson(filePath),
                "13" => UpdateTask(tasks, PromptInt("Enter task ID to update: "), GatherTaskUpdate()),
                "14" => ExitProgram(tasks),
                _ => InvalidOption(tasks)
            };

            InteractiveMenu(updatedTasks, filePath);
        }

        public static Task[] DisplayTasksAndReturn(Task[] tasks)
        {
            DisplayTasks(tasks);
            return tasks;
        }
        public static string PromptStatusChoice(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. Completed");
            Console.WriteLine("3. Overdue");
            var choice = Console.ReadLine();
            string status = choice switch
            {
                "1" => "Pending",
                "2" => "Completed",
                "3" => "Overdue",
            };
            return status;
        }
        public static Task[] SaveToFileAndReturn(Task[] tasks, string filePath)
        {
            SaveToJson(filePath, tasks);
            return tasks;
        }
        public static TaskUpdate GatherTaskUpdate()
        {
            Console.WriteLine("Leave a field blank to keep the current value.");
            var description = PromptString("Enter new description (or press Enter to skip): ");
            var dueDate = PromptString("Enter new due date (MM/dd/yyyy) (or press Enter to skip): ");
            var priority = PromptString("Enter new priority (or press Enter to skip): ");
            Console.WriteLine("Choose status:");
            Console.WriteLine("1. OverDue");
            Console.WriteLine("2. Completed");
            var statusChoice = PromptString("Enter your choice (1 or 2): ");

            string status = statusChoice switch
            {
                "1" => "OverDue",
                "2" => "Completed",
            };

            return new TaskUpdate(
                Description: description,
                DueDate: DateTime.TryParse(dueDate, out var parsedDate) ? parsedDate : null,
                Priority: int.TryParse(priority, out var parsedPriority) ? parsedPriority : null,
                Status: status
            );
        }
        public static Task[] AddNewTask(Task[] tasks)
        {
            var newTask = new Task(
                PromptInt("Enter task ID: "),
                PromptString("Enter description: "),
                PromptDate("Enter due date (MM/dd/yyyy): "),
                PromptInt("Enter priority: "),
                "Pending"
            );
            Console.WriteLine("Task added successfully.");
            return AddTask(tasks, newTask);
        }

        public static Task[] ExitProgram(Task[] tasks)
        {
            Console.WriteLine("Exiting program. Goodbye!");
            Environment.Exit(0);
            return tasks; // Unreachable but required for functional completeness
        }
        public static bool IsValidDate(DateTime date)
        {
            // Check for valid month and day
            if (date.Month < 1 || date.Month > 12)
            {
                return false;
            }

            // Check for valid day based on the month
            if (date.Day < 1 || date.Day > DateTime.DaysInMonth(date.Year, date.Month))
            {
                return false;
            }

            return true; // The date is valid
        }

        public static Task[] InvalidOption(Task[] tasks)
        {
            Console.WriteLine("Invalid option. Please try again.");
            return tasks;
        }

        public static string PromptString(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? string.Empty;
        }

        public static int PromptInt(string message)
        {
            Console.Write(message);
            return int.TryParse(Console.ReadLine(), out var result) ? result : 0;
        }

        public static DateTime PromptDate(string message)
        {
            DateTime result;
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();
                // Attempt to parse the input
                if (DateTime.TryParse(input, out result))
                {
                    // Validate the date using the IsValidDate function
                    if (!IsValidDate(result))
                    {
                        Console.WriteLine($"Invalid day for the month {result.Month}. Please enter a valid day.");
                        continue;
                    }
                    return result; // Return the valid date
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter a date in MM/dd/yyyy format.");
                }
            }
        }
        // Recursive method to get the length of the array
        public static int GetArrayLength(Task[] tasks, int index = 0, int accumulator = 0)
        {
            try
            {
                var temp = tasks[index];
                return GetArrayLength(tasks, index + 1, accumulator + 1);
            }
            catch (IndexOutOfRangeException)
            {
                return accumulator;
            }
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


        public static Task[] DeleteTaskById(Task[] tasks, int id, string filePath, int index = 0, Task[] newArray = null)
        {
            // Initialize newArray on the first call if it's null
            if (newArray == null)
            {
                newArray = Array.Empty<Task>();
            }
            if (index == GetArrayLength(tasks))
            {
                SaveToJson(filePath, newArray);
                return newArray;
            }
            if (tasks[index].Id == id)
            {
                return DeleteTaskById(tasks, id, filePath, index + 1, newArray);
            }
            newArray = AddTask(newArray, tasks[index]);
            return DeleteTaskById(tasks, id, filePath, index + 1, newArray);

        }
        public static Task[] SortByPriority(Task[] tasks)
        {
            return SortHelper(tasks, tasks.Length, 0, tasks, (task1, task2) => task1.Priority > task2.Priority);
        }

        public static Task[] SortByDueDate(Task[] tasks)
        {
            return SortHelper(tasks, tasks.Length, 0, tasks, (task1, task2) => IsDateGreater(task1.DueDate, task2.DueDate));
        }

        // Bubble Sort Alogrithm
        // iterates over all elements, and compare each one with the rest of the array
        private static Task[] SortHelper(Task[] tasks, int length, int currentIndex, Task[] sortedArray, Func<Task, Task, bool> compare)
        {
            // checks if the array is empty " no need to sort " or sorting is done
            if (currentIndex == length - 1 || length == 0)
            {
                return sortedArray;
            }
            return SortInnerHelper(sortedArray, length, currentIndex, currentIndex + 1, sortedArray, compare);
        }

        // "i" the index of the outer loop, "j" the inner one that compares the current element with each element
        private static Task[] SortInnerHelper(Task[] tasks, int length, int i, int j, Task[] currentSorted, Func<Task, Task, bool> compare)
        {
            if (j == length)
            {
                return SortHelper(tasks, length, i + 1, currentSorted, compare); // Move to the next outer loop index
            }
            if (compare(currentSorted[i], currentSorted[j]))
            {
                currentSorted = SwapTasks(currentSorted, i, j);
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
            return FilterTasksHelper(tasks, task => task.DueDate.Date == dueDate.Date, GetArrayLength(tasks), 0, Array.Empty<Task>());
        }

        public static Task[] HighlightNearingDeadlines(Task[] tasks)
        {
            DateTime currentDate = DateTime.Now;
            return FilterTasksHelper(tasks, task => IsDateGreaterThan(task.DueDate, currentDate), GetArrayLength(tasks), 0, Array.Empty<Task>());
        }

        public static Task[] OverDueDeadlines(Task[] tasks)
        {
            DateTime currentDate = DateTime.Now;
            // Use IsDateGreaterThan to check for overdue deadlines
            var overdueTasks = FilterTasksHelper(tasks, task => IsDateLessThanOrEqual(task.DueDate, currentDate), GetArrayLength(tasks), 0, Array.Empty<Task>());
            return UpdateTaskStatusRecursively(overdueTasks, 0, "Overdue");
        }

        private static Task[] UpdateTaskStatusRecursively(Task[] tasks, int index, string newStatus)
        {
            int length = GetArrayLength(tasks);
            if (index >= length)
                return tasks;
            var taskUpdate = new TaskUpdate(Status: newStatus);
            tasks = UpdateTask(tasks, tasks[index].Id, taskUpdate);
            return UpdateTaskStatusRecursively(tasks, index + 1, newStatus);
        }

        private static bool IsDateGreater(DateTime date1, DateTime date2)
        {
            if (date1.Year > date2.Year) return true;
            if (date1.Year == date2.Year && date1.Month > date2.Month) return true;
            if (date1.Year == date2.Year && date1.Month == date2.Month && date1.Day > date2.Day) return true;

            return false;
        }
        private static bool IsDateLessThanOrEqual(DateTime date1, DateTime date2)
        {
            if (date1.Year < date2.Year) return true;
            if (date1.Year == date2.Year && date1.Month < date2.Month) return true;
            if (date1.Year == date2.Year && date1.Month == date2.Month && date1.Day <= date2.Day) return true;

            return false;
        }

        private static bool IsDateGreaterThan(DateTime date1, DateTime date2)
        {
            if (date1.Year > date2.Year) return true;
            if (date1.Year == date2.Year && date1.Month > date2.Month) return true;
            if (date1.Year == date2.Year && date1.Month == date2.Month && date1.Day > date2.Day) return true;

            return false;
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

        public record TaskUpdate(
        string? Description = null,
        DateTime? DueDate = null,
        int? Priority = null,
        string? Status = null
        );

        public static Task[] UpdateTask(Task[] tasks, int taskId, TaskUpdate taskUpdate)
        {
            int Length = GetArrayLength(tasks);
            // Create a new array to hold updated tasks
            Task[] updatedTasks = new Task[Length];

            // Recursive helper function to update tasks
            void UpdateTaskRecursive(int index)
            {
                if (index >= Length)
                    return;

                // If the current task matches the taskId, create a new task with updated fields
                if (tasks[index].Id == taskId)
                {
                    updatedTasks[index] = new Task(
                        Id: tasks[index].Id,
                        Description: taskUpdate.Description ?? tasks[index].Description,
                        DueDate: taskUpdate.DueDate ?? tasks[index].DueDate,
                        Priority: taskUpdate.Priority ?? tasks[index].Priority,
                        Status: taskUpdate.Status ?? tasks[index].Status
                    );
                }
                else
                {
                    // Otherwise, keep the original task
                    updatedTasks[index] = tasks[index];
                }

                // Recur for the next index
                UpdateTaskRecursive(index + 1);
            }

            // Start the recursive update process
            UpdateTaskRecursive(0);

            return updatedTasks;
        }

    }
}


