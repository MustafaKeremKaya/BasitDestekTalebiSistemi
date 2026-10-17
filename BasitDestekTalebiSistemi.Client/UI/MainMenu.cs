using System;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Client.Services;

namespace BasitDestekTalebiSistemi.Client.UI
{

    public class MainMenu
    {
        private readonly TicketListView _ticketListView;
        private readonly CreateTicketView _createTicketView;

        public MainMenu(ApiClient apiClient)
        {
            var ticketDetailView = new TicketDetailView(apiClient);
            _ticketListView = new TicketListView(apiClient, ticketDetailView);
            _createTicketView = new CreateTicketView(apiClient, ticketDetailView);
        }

        public async Task ShowAsync(string currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {
                ConsoleHelper.WriteHeader($"ANA MENÜ (Kullanıcı: {currentUser})");

                Console.WriteLine(" [1] Talepleri Listele");
                Console.WriteLine(" [2] Yeni Talep Aç");
                Console.WriteLine(" [3] Çıkış");

                var choice = ConsoleHelper.Prompt("\nSeçiminiz (1-3): ");

                switch (choice)
                {
                    case "1":
                        await _ticketListView.ShowAsync(currentUser);
                        break;
                    case "2":
                        await _createTicketView.ShowAsync(currentUser);
                        break;
                    case "3":
                        isRunning = false;
                        ConsoleHelper.WriteInfo("Uygulamadan çıkılıyor. İyi günler dileriz!");
                        break;
                    default:
                        ConsoleHelper.WriteWarning("Geçersiz seçim! Lütfen 1, 2 veya 3 yazıp Enter'a basınız.");
                        break;
                }
            }
        }
    }
}
