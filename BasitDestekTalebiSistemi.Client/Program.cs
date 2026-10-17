using System;
using System.Text;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Client.Services;
using BasitDestekTalebiSistemi.Client.UI;
using BasitDestekTalebiSistemi.Core.Constants;

namespace BasitDestekTalebiSistemi.Client
{

    internal class Program
    {
        private static async Task Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
========================================================================
BASİT DESTEK TALEBİ SİSTEMİ (TERMİNAL İSTEMCİSİ)
========================================================================");
            Console.ResetColor();

            string currentUser = string.Empty;
            while (string.IsNullOrWhiteSpace(currentUser))
            {
                var input = ConsoleHelper.Prompt("\nKullanıcı Adınızı Girin (Maks. 50 karakter): ");

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleHelper.WriteWarning(Messages.CreatedByEmpty);
                    continue;
                }

                if (input.Length > 50)
                {
                    ConsoleHelper.WriteWarning(Messages.CreatedByTooLong);
                    continue;
                }

                currentUser = input;
            }

            var serverInput = ConsoleHelper.Prompt("\nSunucu Adresi (Varsayılan için Enter'a basın [http://localhost:5000]): ");
            string serverUrl = "http://localhost:5000";

            if (!string.IsNullOrWhiteSpace(serverInput))
            {
                serverUrl = serverInput;
                if (!serverUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !serverUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    serverUrl = "http://" + serverUrl;
                }
            }

            ConsoleHelper.WriteInfo($"Sunucuya bağlanılıyor: {serverUrl}...");

            var apiClient = new ApiClient(serverUrl);

            var connectionTest = await apiClient.GetTicketsAsync();
            if (!connectionTest.Success)
            {
                ConsoleHelper.WriteError(connectionTest.Message);
                Console.WriteLine("İpucu: Eğer aynı bilgisayardaysanız Web API'nin açık olduğundan emin olun.");

                var retryChoice = ConsoleHelper.Prompt("\nYine de devam etmek istiyor musunuz? (E/H): ");
                if (!string.Equals(retryChoice, "E", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Uygulama kapatılıyor.");
                    return;
                }
            }
            else
            {
                ConsoleHelper.WriteSuccess("Sunucuya başarıyla bağlanıldı!");
            }

            var menu = new MainMenu(apiClient);
            await menu.ShowAsync(currentUser);
        }
    }
}
