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
            ConsoleHelper.WriteInfo("Talepler sunucudan yükleniyor...");

            var response = await _apiClient.GetTicketsAsync();

            if (!response.Success || response.Data == null)
            {
                ConsoleHelper.WriteError("Talepler yüklenemedi: " + response.Message);
                return;
            }

            if (response.Data.Count == 0)
            {
                ConsoleHelper.WriteInfo("Henüz açılmış hiçbir talep yok. 2'ye basarak ilk talebi siz açabilirsiniz!");
                return;
            }

            PrintTicketTable(response.Data);

            while (true)
            {
                var input = ConsoleHelper.Prompt("\nDetayını görmek istediğiniz Talep ID'sini girin (Geri dönmek için 0): ");

                if (input == "0")
                {
                    return;
                }

                if (!int.TryParse(input, out int selectedId))
                {
                    ConsoleHelper.WriteWarning("Geçersiz ID! Lütfen talep numarasını yazınız.");
                    continue;
                }

                if (!response.Data.Any(t => t.Id == selectedId))
                {
                    ConsoleHelper.WriteWarning($"#{selectedId} numaralı bir talep listede bulunamadı.");
                    continue;
                }

                await _ticketDetailView.ShowAsync(selectedId, currentUser);
                return;
            }
        }

        private void PrintTicketTable(List<TicketSummaryResponse> tickets)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- DESTEK TALEPLERİ ---");
            Console.WriteLine($"{"ID",-5}{"Başlık",-32}{"Durum",-12}{"Açan Kişi",-17}{"Tarih",-18}{"Yanıt",5}");
            Console.WriteLine(new string('-', 89));
            Console.ResetColor();

            foreach (var ticket in tickets.OrderBy(t => t.Id))
            {
                Console.Write($"{ticket.Id,-5}{Shorten(ticket.Title, 30),-32}");
                ConsoleHelper.WriteStatus(ticket.Status);
                Console.Write(new string(' ', 12 - ConsoleHelper.GetStatusText(ticket.Status).Length));
                Console.WriteLine($"{Shorten(ticket.CreatedBy, 15),-17}{ticket.CreatedAt.ToLocalTime(),-18:g}{ticket.ReplyCount,5}");
            }
        }

        private static string Shorten(string text, int maxLength)
        {
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}
