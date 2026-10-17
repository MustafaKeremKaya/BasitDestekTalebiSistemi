using System;
using System.Linq;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Client.Services;
using BasitDestekTalebiSistemi.Core.Constants;
using BasitDestekTalebiSistemi.Entities.DTOs;
using BasitDestekTalebiSistemi.Entities.Enums;

namespace BasitDestekTalebiSistemi.Client.UI
{

    public class TicketDetailView
    {
        private readonly ApiClient _apiClient;

        public TicketDetailView(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task ShowAsync(int ticketId, string currentUser)
        {
            while (true)
            {

                var detailResult = await _apiClient.GetTicketDetailAsync(ticketId);

                if (!detailResult.Success || detailResult.Data == null)
                {
                    ConsoleHelper.WriteError("Talep detayı alınamadı: " + detailResult.Message);
                    return;
                }

                var ticket = detailResult.Data;

                PrintTicketDetail(ticket);
                await PrintRepliesAsync(ticketId);

                Console.WriteLine("\n [1] Yanıt Yaz");
                Console.WriteLine(" [2] Çözüldü Olarak İşaretle");
                Console.WriteLine(" [0] Ana Menüye Dön");

                var choice = ConsoleHelper.Prompt("\nSeçiminiz: ");

                switch (choice)
                {
                    case "1":
                        if (ticket.Status == TicketStatus.Closed)
                        {
                            ConsoleHelper.WriteWarning(Messages.ReplyToClosedTicket);
                            break;
                        }
                        await WriteReplyAsync(ticketId, currentUser);
                        break;
                    case "2":
                        if (ticket.Status == TicketStatus.Closed)
                        {
                            ConsoleHelper.WriteWarning(Messages.TicketAlreadyClosed);
                            break;
                        }
                        await CloseTicketAsync(ticketId);
                        break;
                    case "0":
                        return;
                    default:
                        ConsoleHelper.WriteWarning("Geçersiz seçim! Lütfen 1, 2 veya 0 yazınız.");
                        break;
                }
            }
        }

        private void PrintTicketDetail(TicketDetailResponse ticket)
        {
            ConsoleHelper.WriteHeader($"TALEP #{ticket.Id}: {ticket.Title}");

            Console.Write("Durum       : ");
            ConsoleHelper.WriteStatus(ticket.Status);
            Console.WriteLine();
            Console.WriteLine($"Açan Kişi   : {ticket.CreatedBy}");
            Console.WriteLine($"Açılış      : {ticket.CreatedAt.ToLocalTime():g}");
            Console.WriteLine($"Açıklama    : {ticket.Description}");
            Console.WriteLine($"Yanıt Sayısı: {ticket.ReplyCount}");

            var participants = ticket.Participants.Count > 0
                ? string.Join(", ", ticket.Participants)
                : "-";
            Console.WriteLine($"Katılımcılar: {participants}");
        }

        private async Task PrintRepliesAsync(int ticketId)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- YANITLAR ---");
            Console.ResetColor();

            var repliesResult = await _apiClient.GetRepliesAsync(ticketId);

            if (!repliesResult.Success || repliesResult.Data == null)
            {
                ConsoleHelper.WriteError("Yanıtlar yüklenemedi: " + repliesResult.Message);
                return;
            }

            if (repliesResult.Data.Count == 0)
            {
                Console.WriteLine("Bu talebe henüz yanıt yazılmamış.");
                return;
            }

            foreach (var reply in repliesResult.Data.OrderBy(r => r.CreatedAt))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{reply.CreatedAt.ToLocalTime():g}] ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{reply.RepliedBy}: ");
                Console.ResetColor();
                Console.WriteLine(reply.Content);
            }
        }

        private async Task WriteReplyAsync(int ticketId, string currentUser)
        {
            var content = ConsoleHelper.Prompt("Yanıtınızı yazın (İptal etmek için boş bırakın): ");

            if (string.IsNullOrWhiteSpace(content))
            {
                ConsoleHelper.WriteWarning(Messages.ContentEmpty + " Yanıt gönderilmedi.");
                return;
            }

            if (content.Length > 500)
            {
                ConsoleHelper.WriteWarning(Messages.ContentTooLong);
                return;
            }

            var request = new CreateReplyRequest
            {
                RepliedBy = currentUser,
                Content = content
            };

            var result = await _apiClient.AddReplyAsync(ticketId, request);

            if (result.Success)
            {
                ConsoleHelper.WriteSuccess(result.Message ?? Messages.ReplyAdded);
            }
            else
            {
                ConsoleHelper.WriteError(result.Message);
            }
        }

        private async Task CloseTicketAsync(int ticketId)
        {
            var confirm = ConsoleHelper.Prompt("Talebi çözüldü olarak işaretlemek istediğinize emin misiniz? (E/H): ");

            if (!string.Equals(confirm, "E", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleHelper.WriteInfo("İşlem iptal edildi.");
                return;
            }

            var result = await _apiClient.CloseTicketAsync(ticketId);

            if (result.Success)
            {
                ConsoleHelper.WriteSuccess(result.Message ?? Messages.TicketClosed);
            }
            else
            {
                ConsoleHelper.WriteError(result.Message);
            }
        }
    }
}
