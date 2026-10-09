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
                    ConsoleHelper.WriteError("Talep detayi alinamadi: " + detailResult.Message);
                    return;
                }

                var ticket = detailResult.Data;

                PrintTicketDetail(ticket);
                await PrintRepliesAsync(ticketId);

                Console.WriteLine("\n [1] Yanit Yaz");
                Console.WriteLine(" [2] Talebi Kapat");
                Console.WriteLine(" [0] Ana Menuye Don");

                var choice = ConsoleHelper.Prompt("\nSeciminiz: ");

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
                        ConsoleHelper.WriteWarning("Gecersiz secim! Lutfen 1, 2 veya 0 yaziniz.");
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
            Console.WriteLine($"Acan Kisi   : {ticket.CreatedBy}");
            Console.WriteLine($"Acilis      : {ticket.CreatedAt.ToLocalTime():g}");
            Console.WriteLine($"Aciklama    : {ticket.Description}");
            Console.WriteLine($"Yanit Sayisi: {ticket.ReplyCount}");

            var participants = ticket.Participants.Count > 0
                ? string.Join(", ", ticket.Participants)
                : "-";
            Console.WriteLine($"Katilimcilar: {participants}");
        }

        private async Task PrintRepliesAsync(int ticketId)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- YANITLAR ---");
            Console.ResetColor();

            var repliesResult = await _apiClient.GetRepliesAsync(ticketId);

            if (!repliesResult.Success || repliesResult.Data == null)
            {
                ConsoleHelper.WriteError("Yanitlar yuklenemedi: " + repliesResult.Message);
                return;
            }

            if (repliesResult.Data.Count == 0)
            {
                Console.WriteLine("Bu talebe henuz yanit yazilmamis.");
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
            var content = ConsoleHelper.Prompt("Yanitinizi yazin (Iptal etmek icin bos birakin): ");

            if (string.IsNullOrWhiteSpace(content))
            {
                ConsoleHelper.WriteWarning(Messages.ContentEmpty + " Yanit gonderilmedi.");
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
            var confirm = ConsoleHelper.Prompt("Talebi kapatmak istediginize emin misiniz? (E/H): ");

            if (!string.Equals(confirm, "E", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleHelper.WriteInfo("Kapatma islemi iptal edildi.");
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
