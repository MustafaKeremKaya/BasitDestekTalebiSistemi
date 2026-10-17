using System;
using BasitDestekTalebiSistemi.Entities.Enums;

namespace BasitDestekTalebiSistemi.Client.UI
{

    public static class ConsoleHelper
    {

        public static void WriteHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n==================================================");
            Console.WriteLine(title);
            Console.WriteLine("==================================================");
            Console.ResetColor();
        }

        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[BİLGİ] " + message);
            Console.ResetColor();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[BAŞARILI] " + message);
            Console.ResetColor();
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[UYARI] " + message);
            Console.ResetColor();
        }

        public static void WriteError(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[HATA] " + message);
            Console.ResetColor();
        }

        public static string Prompt(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
            Console.ResetColor();

            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static string GetStatusText(TicketStatus status)
        {
            return status == TicketStatus.Open ? "Çözülmedi" : "Çözüldü";
        }

        public static void WriteStatus(TicketStatus status)
        {
            Console.ForegroundColor = status == TicketStatus.Open ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(GetStatusText(status));
            Console.ResetColor();
        }
    }
}
