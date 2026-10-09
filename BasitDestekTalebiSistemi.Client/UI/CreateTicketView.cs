using System;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Client.Services;
using BasitDestekTalebiSistemi.Core.Constants;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Client.UI
{

    public class CreateTicketView
    {
        private readonly ApiClient _apiClient;
        private readonly TicketDetailView _ticketDetailView;

        public CreateTicketView(ApiClient apiClient, TicketDetailView ticketDetailView)
        {
            _apiClient = apiClient;
            _ticketDetailView = ticketDetailView;
        }

        public async Task ShowAsync(string currentUser)
        {
            ConsoleHelper.WriteHeader("YENI TALEP AC");

            var title = ConsoleHelper.Prompt("Talep Basligini Girin (Iptal etmek icin bos birakin): ");

            if (string.IsNullOrWhiteSpace(title))
            {
                ConsoleHelper.WriteWarning(Messages.TicketTitleEmpty + " Talep acma islemi iptal edildi.");
                return;
            }

            if (title.Length > 100)
            {
                ConsoleHelper.WriteWarning(Messages.TicketTitleTooLong);
                return;
            }

            var description = ConsoleHelper.Prompt("Talep Aciklamasini Girin: ");

            if (string.IsNullOrWhiteSpace(description))
            {
                ConsoleHelper.WriteWarning(Messages.TicketDescriptionEmpty + " Talep acma islemi iptal edildi.");
                return;
            }

            if (description.Length > 1000)
            {
                ConsoleHelper.WriteWarning(Messages.TicketDescriptionTooLong);
                return;
            }

            ConsoleHelper.WriteInfo("Talep olusturuluyor...");

            var request = new CreateTicketRequest
            {
                Title = title,
                Description = description,
                CreatedBy = currentUser
            };

            var result = await _apiClient.CreateTicketAsync(request);

            if (result.Success && result.Data != null)
            {
                ConsoleHelper.WriteSuccess($"'{result.Data.Title}' talebi basariyla acildi! (ID: {result.Data.Id})");

                await _ticketDetailView.ShowAsync(result.Data.Id, currentUser);
            }
            else
            {
                ConsoleHelper.WriteError("Talep olusturulamadi: " + result.Message);
            }
        }
    }
}
