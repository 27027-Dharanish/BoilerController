using BoilerSystem.Core.Constants;

namespace BoilerSystem.View
{
    /// <summary>
    /// Handles user interaction activities by managing standard input and output via the console.
    /// </summary>
    public static class ConsoleActivity
    {
        private static readonly object _lock = new object();
        private static int _notificationRow = 1;
        private static int _currentRow = 0;

        /// <summary>
        /// Print the given content in the console.
        /// Always prints on the left side.
        /// </summary>
        /// <param name="content">Content that need to be printed.</param>
        public static void PrintInConsole(string content)
        {
            lock (_lock)
            {
                int leftWidth = Console.WindowWidth / 2;
                Console.SetCursorPosition(0, _currentRow);
                Console.Write(content.PadRight(leftWidth));
                _currentRow++;
            }
        }

        /// <summary>
        /// Prompts the user and reads their text input from the console.
        /// </summary>
        /// <param name="label">Label that requested for input.</param>
        /// <returns>Text entered by the user.</returns>
        public static string? GetStringInput(string label)
        {
            PrintEmptyLine();
            lock (_lock)
            {
                Console.SetCursorPosition(0, _currentRow);
                Console.Write($"Enter the {label} : ");
                _currentRow++;
            }

            return Console.ReadLine();
        }

        /// <summary>
        /// Wait in the console until user presses any key.
        /// </summary>
        public static void WaitInConsole()
        {
            PrintEmptyLine();
            PrintInConsole("Press any key to continue!!");
            Console.ReadKey();
        }

        /// <summary>
        /// Print empty line in console.
        /// </summary>
        public static void PrintEmptyLine()
        {
            PrintInConsole(string.Empty);
        }

        /// <summary>
        /// Show the menu options available.
        /// </summary>
        /// <param name="header">Menu header.</param>
        /// <param name="menuItem">List of menu items.</param>
        public static void ShowMenu(string header, string[] menuItem)
        {
            ShowHeader(header);
            PrintItems(menuItem);
            PrintInConsole(new string('-', 40));
        }

        /// <summary>
        /// Show the header for the operation.
        /// </summary>
        /// <param name="header">Name of the header.</param>
        public static void ShowHeader(string header)
        {
            ClearLeftHalf();
            PrintInConsole(new string('=', 40));
            PrintInConsole($"          {header}");
            PrintInConsole(new string('=', 40));
        }

        /// <summary>
        /// Clear the complete console.
        /// </summary>
        public static void ClearConsole()
        {
            lock (_lock)
            {
                Console.Clear();
                _currentRow = 0;
                _notificationRow = 1;
                Console.Write("\x1b[3J");
            }
        }

        /// <summary>
        /// Print the list of items in console.
        /// </summary>
        /// <param name="items">Items to be printed.</param>
        public static void PrintItems(string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                PrintInConsole($"[{i + 1}] {items[i]}");
            }
        }

        /// <summary>
        /// Get integer value input from user.
        /// </summary>
        /// <param name="label">Label of the input field.</param>
        /// <returns>The prompted integer value.</returns>
        public static int GetIntegerInput(string label)
        {
            string? userInput = GetStringInput(label);
            int.TryParse(userInput, out int value);
            return value;
        }

        /// <summary>
        /// Print and wait in console.
        /// </summary>
        /// <param name="content">Content to be printed in console.</param>
        public static void PrintAndWait(string content)
        {
            PrintInConsole(content);
            WaitInConsole();
        }


        /// <summary>
        /// Clear the right half of the console.
        /// </summary>
        public static void ClearRightHalf()
        {
            lock (_lock)
            {
                int startColumn = Console.WindowWidth / 2;
                for (int row = 0; row < Console.WindowHeight; row++)
                {
                    Console.SetCursorPosition(startColumn, row);
                    Console.Write(new string(' ', Console.WindowWidth - startColumn));
                }

                _notificationRow = 1;
                Console.SetCursorPosition(startColumn, 0);
            }
        }

        /// <summary>
        /// Clear the left half of the console.
        /// </summary>
        public static void ClearLeftHalf()
        {
            lock (_lock)
            {
                int endColumn = Console.WindowWidth / 2;
                for (int row = 0; row < Console.WindowHeight; row++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write(new string(' ', endColumn));
                }

                _currentRow = 0;
                Console.SetCursorPosition(0, 0);
            }
        }

        /// <summary>
        /// Display notification on the right side.
        /// </summary>
        public static void DisplayNotification(string message, Colors color)
        {
            lock (_lock)
            {
                int startColumn = Console.WindowWidth / 2;
                int notificationColumn = startColumn + 2;
                Console.SetCursorPosition(notificationColumn, _notificationRow);
                string notification = $"[Notification] :{message}";
                int availableWidth = Console.WindowWidth - notificationColumn;
                switch(color)
                {
                    case Colors.green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(notification);
                        break;
                    case Colors.red:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(notification);
                        break;
                    case Colors.yellow:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(notification, ConsoleColor.Yellow);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(notification, ConsoleColor.White);
                        break;
                }

                Console.ResetColor();
                _notificationRow++;
                if (_notificationRow >= Console.WindowHeight)
                {
                    ClearRightHalf();
                }

                Console.SetCursorPosition(0, _currentRow);
            }
        }

        public static void PrintLog(string[] lines)
        {
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
            ConsoleActivity.WaitInConsole();
            ConsoleActivity.ClearConsole();
        }
    }
}
