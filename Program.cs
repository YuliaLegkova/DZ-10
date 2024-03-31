using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        DirectoryInfo dir1 = Directory.CreateDirectory(@"c:\Otus\TestDir1");
        DirectoryInfo dir2 = Directory.CreateDirectory(@"c:\Otus\TestDir2");

        for (int i = 1; i <= 10; i++)
        {
            string fileName = $"File{i}.txt";
            string content = fileName;

            WriteToFileSynchronously(Path.Combine(dir1.FullName, fileName), content);
           // WriteToFileSynchronously(Path.Combine(dir2.FullName, fileName), content);
           await WriteToFileAsynchronously(Path.Combine(dir2.FullName, fileName), content);
        }

        ReadAndPrintFiles(dir1.FullName);
        ReadAndPrintFiles(dir2.FullName);
    }

    static void WriteToFileSynchronously(string filePath, string content)
    {
        try
        {
            File.WriteAllText(filePath, content, Encoding.UTF8);
            AppendDateTime(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл {filePath}: {ex.Message}");
        }
    }

    static async Task WriteToFileAsynchronously(string filePath, string content)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                await writer.WriteAsync(content);
                await writer.FlushAsync();
                await AppendDateTimeAsync(filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при асинхронной записи в файл {filePath}: {ex.Message}");
        }
    }

    static void AppendDateTime(string filePath)
    {
        try
        {
            string dateTimeString = $" [{DateTime.Now}]";
            File.AppendAllText(filePath, dateTimeString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении даты в файл {filePath}: {ex.Message}");
        }
    }

    static async Task AppendDateTimeAsync(string filePath)
    {
        try
        {
            string dateTimeString = $" [{DateTime.Now}]";
            using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                await writer.WriteAsync(dateTimeString);
                await writer.FlushAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при асинхронном добавлении даты в файл {filePath}: {ex.Message}");
        }
    }

    static void ReadAndPrintFiles(string directoryPath)
    {
        Console.WriteLine($"Содержимое директории: {directoryPath}");
        string[] files = Directory.GetFiles(directoryPath);
        foreach (string file in files)
        {
            try
            {
                string content = File.ReadAllText(file);
                Console.WriteLine($"{Path.GetFileName(file)}: {content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла {file}: {ex.Message}");
            }
        }
    }
}