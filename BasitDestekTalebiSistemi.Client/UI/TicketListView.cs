using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Client.Services;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Client.UI
{

    public class TicketListView
    {
        private readonly ApiClient _apiClient;
        private readonly TicketDetailView _ticketDetailView;

        public TicketListView(ApiClient apiClient, TicketDetailView ticketDetailView)
        {
            _apiClient = apiClient;
            _ticketDetailView = ticketDetailView;
        }

        public async Task ShowAsync(string currentUser)
        {
            ConsoleHelper.WriteInfo("Talepler sunucudan yukleniyor...");

            var response = await _apiClient.GetTicketsAsync();

            if (!response.Success || response.Data == null)
            {
                ConsoleHelper.WriteError("Talepler yuklenemedi: " + response.Message);
                return;
            }

            if (response.Data.Count == 0)
            {
                ConsoleHelper.WriteInfo("Henuz acilmis hicbir talep yok. 2'ye basarak ilk talebi siz acabilirsiniz!");
                return;
            }

            PrintTicketTable(response.Data);

            while (true)
            {
                var input = ConsoleHelper.Prompt("\nDetayini gormek istediginiz Talep ID'sini girin (Geri donmek icin 0): ");

                if (input == "0")
                {
                    return;
                }

                if (!int.TryParse(input, out int selectedId))
                {
                    ConsoleHelper.WriteWarning("Gecersiz ID! Lutfen talep numarasini yaziniz.");
                    continue;
                }

                if (!response.Data.Any(t => t.Id == selectedId))
                {
                    ConsoleHelper.WriteWarning($"#{selectedId} numarali bir talep listede bulunamadi.");
                    continue;
                }

                await _ticketDetailView.ShowAsync(selectedId, currentUser);
                return;
            }
        }

        private void PrintTicketTable(List<TicketSummaryResponse> tickets)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- DESTEK TALEPLERI ---");
            Console.WriteLine($"{"ID",-5}{"Baslik",-32}{"Durum",-8}{"Acan Kisi",-17}{"Tarih",-18}{"Yanit",5}");
            Console.WriteLine(new string('-', 85));
            Console.ResetColor();

            foreach (var ticket in tickets.OrderBy(t => t.Id))
            {
                Console.Write($"{ticket.Id,-5}{Shorten(ticket.Title, 30),-32}");
                ConsoleHelper.WriteStatus(ticket.Status);
                Console.Write(new string(' ', 8 - ConsoleHelper.GetStatusText(ticket.Status).Length));
                Console.WriteLine($"{Shorten(ticket.CreatedBy, 15),-17}{ticket.CreatedAt.ToLocalTime(),-18:g}{ticket.ReplyCount,5}");
            }
        }

        private static string Shorten(string text, int maxLength)
        {
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}
