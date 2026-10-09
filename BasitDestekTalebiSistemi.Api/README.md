# BasitDestekTalebiSistemi.Api - Backend Servisi Çalıştırma Talimatı

Bu proje, staj projesi kapsamında geliştirilen destek talebi sisteminin veritabanı bağlantılarını yöneten ve istemcilere RESTful uç noktalar sunan ASP.NET Core Web API katmanıdır.

## Gereksinimler
- .NET SDK 10
- PostgreSQL Veritabanı Sunucusu (Varsayılan port: 5432)
- EF Core komut satırı aracı: `dotnet tool install --global dotnet-ef`

## Çalıştırma Adımları
1. PostgreSQL sunucusunun çalıştığından emin olun.
2. Veritabanı henüz oluşturulmadıysa proje ana dizininde şu komutu çalıştırın:
   ```bash
   dotnet ef database update --project BasitDestekTalebiSistemi.DataAccess --startup-project BasitDestekTalebiSistemi.Api
   ```
3. Terminali bu klasörde açarak şu komutu çalıştırın:
   ```bash
   dotnet run
   ```
4. Servis varsayılan olarak `http://localhost:5000` adresinde dinlemeye başlayacaktır (yerel ağ için `http://<bilgisayar-ip>:5000`).
5. Swagger test arayüzüne tarayıcınızdan erişmek için: `http://localhost:5000/swagger`
