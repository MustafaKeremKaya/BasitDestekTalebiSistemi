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
                ConsoleHelper.WriteHeader($"ANA MENU (Kullanici: {currentUser})");

                Console.WriteLine(" [1] Talepleri Listele");
                Console.WriteLine(" [2] Yeni Talep Ac");
                Console.WriteLine(" [3] Cikis");

                var choice = ConsoleHelper.Prompt("\nSeciminiz (1-3): ");

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
                        ConsoleHelper.WriteInfo("Uygulamadan cikiliyor. Iyi gunler dileriz!");
                        break;
                    default:
                        ConsoleHelper.WriteWarning("Gecersiz secim! Lutfen 1, 2 veya 3 yazip Enter'a basiniz.");
                        break;
                }
            }
        }
    }
}
