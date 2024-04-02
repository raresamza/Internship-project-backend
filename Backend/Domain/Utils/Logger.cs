using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Utils;

public class Logger
{

    private static readonly string LogDirectory = "Logs";
    private static readonly string logFileName = $"Logs_{DateTime.Now:dd_MM_yyyy}.txt";
    //private static readonly string logFilePath = $"../Logs/{logFileName}";
    private static readonly string logFilePath = Path.Combine("..","..", "..", "..", "Domain", "Logs", logFileName);

    public async static Task LogMethodCall(string methodName, bool success)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {methodName} - {(success ? "success" : "failure")}{Environment.NewLine}";

        try
        {
            Directory.CreateDirectory(LogDirectory);
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter writer = File.CreateText(logFilePath))
                {
                    writer.WriteLine("Log File Created:");
                }
            }
            await WriteLogAsync(logFilePath, logEntry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    private async static Task WriteLogAsync(string filePath, string logEntry)
    {
        using (StreamWriter writer = File.AppendText(filePath))
        {
            await writer.WriteAsync(logEntry);
        }
    }

}