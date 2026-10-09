# BasitDestekTalebiSistemi.Client - Terminal İstemcisi Çalıştırma Talimatı

Bu proje, staj projesi kapsamında kullanıcıların terminal üzerinden Web API ile haberleşerek destek talebi açmasını, talepleri listelemesini, taleplere yanıt yazmasını ve talepleri kapatmasını sağlayan konsol istemcisidir.

## Çalıştırma Adımları
1. Öncelikle `BasitDestekTalebiSistemi.Api` projesinin çalışır durumda olduğundan emin olun.
2. Terminali bu klasörde açarak şu komutu çalıştırın:
   ```bash
   dotnet run
   ```
3. Uygulama açıldığında bir kullanıcı adı girin.
4. Bağlanılacak sunucu adresi olarak varsayılan `http://localhost:5000` adresini (Enter) veya yerel ağdaysanız sunucu bilgisayarın yerel IP adresini girin.
5. Ana menü üzerinden talepleri listeleyebilir, yeni talep açabilir veya bir talebin ID'sini girerek detayına geçebilirsiniz. Detay ekranında yanıt yazabilir (`1`), talebi kapatabilir (`2`) veya ana menüye dönebilirsiniz (`0`).
