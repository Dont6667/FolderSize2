using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Пожалуйста, укажите путь к целевой папке.");
            return;
        }

        string directoryPath = args[0];

        try
        {
            // Проверяем существование указанной папки
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Папка '{directoryPath}' не существует.");
                return;
            }

            // Получаем и выводим размер папки
            long directorySize = GetDirectorySize(directoryPath);
            Console.WriteLine($"Размер папки '{directoryPath}': {FormatBytes(directorySize)}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Ошибка доступа: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static long GetDirectorySize(string directoryPath)
    {
        long directorySize = 0;

        // Перебираем все файлы в текущей директории
        foreach (string file in Directory.EnumerateFiles(directoryPath))
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file);
                directorySize += fileInfo.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось получить информацию о файле '{file}': {ex.Message}");
            }
        }

        // Рекурсивно вызываем этот метод для подпапок
        foreach (string subDirectory in Directory.EnumerateDirectories(directoryPath))
        {
            try
            {
                directorySize += GetDirectorySize(subDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось получить размер папки '{subDirectory}': {ex.Message}");
            }
        }

        return directorySize;
    }

    static string FormatBytes(long bytes)
    {
        const int scale = 1024;
        string[] orders = { "TB", "GB", "MB", "KB", "Bytes" };

        double max = Math.Pow(scale, orders.Length - 1);

        foreach (string order in orders)
        {
            if (bytes > max)
                return string.Format("{0:##.##} {1}", bytes / max, order);

            max /= scale;
        }

        return "0 Bytes";
    }
}
