using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Globalization;

class TaskItem
{
    public string Title { get; set; }
    public string DueDate { get; set; }
    public string Project { get; set; }
    public string Status { get; set; }

    public TaskItem(string title, string dueDate, string project, string status = "Todo")
    {
        Title = title;
        DueDate = dueDate;
        Project = project;
        Status = status;
    }
}

class Program
{
    static List<TaskItem> tasks = new List<TaskItem>();
    static string dataFile = "tasks.json";

    static void Main(string[] args)
    {
        LoadTasks();

        while (true)
        {
            ShowMenu();
            Console.Write(">> ");
            string choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1":
                    ShowTasks();
                    break;
                case "2":
                    AddTask();
                    break;
                case "3":
                    EditTask();
                    break;
                case "4":
                    SaveTasks();
                    Console.WriteLine(">> Tasks saved. Goodbye!");
                    return;
                default:
                    Console.WriteLine(">> Invalid choice. Try again.");
                    break;
            }

            Console.WriteLine("\n>> Press Enter to return to the menu...");
            Console.ReadLine();
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(">> Welcome to ToDoLy");
        Console.WriteLine($">> You have {CountTasks("Todo")} tasks todo and {CountTasks("Done")} tasks are done!");
        Console.WriteLine(">> Pick an option:");
        Console.WriteLine(">> (1) Show Task List (by date or project)");
        Console.WriteLine(">> (2) Add New Task");
        Console.WriteLine(">> (3) Edit Task (update, mark as done, remove)");
        Console.WriteLine(">> (4) Save and Quit");
        Console.ResetColor();
    }

    static int CountTasks(string status)
    {
        return tasks.FindAll(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).Count;
    }

    static void AddTask()
    {
        while (true)
        {
            string title;
            while (true)
            {
                Console.Write(">> Enter title (required): ");
                title = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrEmpty(title) && title.Length <= 50)
                    break;
                Console.WriteLine(">> Title cannot be empty and max 50 characters.");
            }

            string dueDate;
            while (true)
            {
                Console.Write(">> Enter due date (YYYY-MM-DD): ");
                dueDate = (Console.ReadLine() ?? "").Trim();
                // Striktare datumval med TryParseExact
                if (DateTime.TryParseExact(dueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    break;
                else
                    Console.WriteLine(">> Invalid date format. Please use YYYY-MM-DD.");
            }

            string project;
            while (true)
            {
                Console.Write(">> Enter project (optional, max 50 chars): ");
                project = (Console.ReadLine() ?? "").Trim();
                if (project.Length <= 50)
                    break;
                Console.WriteLine(">> Project name too long. Max 50 characters.");
            }

            tasks.Add(new TaskItem(title, dueDate, project));
            Console.WriteLine(">> Task added!");

            Console.Write(">> Do you want to add another task? (y/n): ");
            string answer = (Console.ReadLine() ?? "").Trim().ToLower();

            if (answer != "y" && answer != "yes")
            {
                Console.WriteLine(">> Returning to main menu...");
                break;
            }

            Console.WriteLine();
        }
    }

    static void ShowTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine(">> No tasks to show!");
            return;
        }

        Console.WriteLine("\n--- Task List ---");
        Console.WriteLine("{0,-4} {1,-25} {2,-15} {3,-20} {4,-10}", "No.", "Title", "Due Date", "Project", "Status");
        Console.WriteLine(new string('-', 80));

        for (int i = 0; i < tasks.Count; i++)
        {
            TaskItem t = tasks[i];
            Console.WriteLine("{0,-4} {1,-25} {2,-15} {3,-20} {4,-10}",
                i + 1,
                Truncate(t.Title, 25),
                t.DueDate,
                string.IsNullOrEmpty(t.Project) ? "-" : Truncate(t.Project, 20),
                t.Status);
        }

        Console.WriteLine(new string('-', 80));
        Console.WriteLine();
    }

    static void EditTask()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine(">> No tasks to edit!");
            return;
        }

        ShowTasks();

        int taskNumber;
        while (true)
        {
            Console.Write(">> Enter task number to edit: ");
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out taskNumber) &&
                taskNumber >= 1 && taskNumber <= tasks.Count)
                break;
            Console.WriteLine(">> Invalid number. Try again.");
        }

        TaskItem task = tasks[taskNumber - 1];

        Console.WriteLine(">> Choose action:");
        Console.WriteLine("1. Update title");
        Console.WriteLine("2. Update due date");
        Console.WriteLine("3. Update project");
        Console.WriteLine("4. Mark as Done");
        Console.WriteLine("5. Remove task");

        string choice = (Console.ReadLine() ?? "").Trim();
        if (choice == "1")
        {
            while (true)
            {
                Console.Write(">> Enter new title (leave empty to keep current): ");
                string newTitle = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrEmpty(newTitle))
                {
                    Console.WriteLine(">> Title unchanged.");
                    break;
                }
                else if (newTitle.Length <= 50)
                {
                    task.Title = newTitle;
                    Console.WriteLine(">> Title updated.");
                    break;
                }
                else
                {
                    Console.WriteLine(">> Title too long. Max 50 characters.");
                }
            }
        }
        else if (choice == "2")
        {
            while (true)
            {
                Console.Write(">> Enter new due date (YYYY-MM-DD) (leave empty to keep current): ");
                string newDate = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrEmpty(newDate))
                {
                    Console.WriteLine(">> Due date unchanged.");
                    break;
                }
                else if (DateTime.TryParseExact(newDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    task.DueDate = newDate;
                    Console.WriteLine(">> Due date updated.");
                    break;
                }
                else
                    Console.WriteLine(">> Invalid date format. Please use YYYY-MM-DD.");
            }
        }
        else if (choice == "3")
        {
            while (true)
            {
                Console.Write(">> Enter new project (leave empty to keep current): ");
                string newProject = (Console.ReadLine() ?? "").Trim();
                if (newProject.Length <= 50)
                {
                    if (!string.IsNullOrEmpty(newProject))
                    {
                        task.Project = newProject;
                        Console.WriteLine(">> Project updated.");
                    }
                    else
                    {
                        Console.WriteLine(">> Project unchanged.");
                    }
                    break;
                }
                else
                {
                    Console.WriteLine(">> Project name too long. Max 50 characters.");
                }
            }
        }
        else if (choice == "4")
        {
            task.Status = "Done";
            Console.WriteLine(">> Task marked as Done.");
        }
        else if (choice == "5")
        {
            tasks.RemoveAt(taskNumber - 1);
            Console.WriteLine(">> Task removed.");
        }
        else
        {
            Console.WriteLine(">> Invalid choice.");
        }
    }

    static void SaveTasks()
    {
        try
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(dataFile, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($">> Error saving tasks: {ex.Message}");
        }
    }

    static void LoadTasks()
    {
        if (File.Exists(dataFile))
        {
            try
            {
                string json = File.ReadAllText(dataFile);
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($">> Error loading tasks: {ex.Message}");
                tasks = new List<TaskItem>();
            }
        }
    }

    static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return "";
        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
    }
}
