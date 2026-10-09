# Staj Projesi: Basit Destek Talebi Sistemi

Bu proje, staj kapsamında geliştirilen katmanlı mimariye sahip bir **ASP.NET Core Web API** ve bu servise terminalden bağlanan **Console Client** uygulamasıdır.

Kullanıcılar bir sorunu bildirmek için destek talebi açabilir, açılan taleplere yanıt yazabilir ve çözülen talepleri kapatabilir. Veriler PostgreSQL veritabanında kalıcı olarak saklanır. Kullanıcı girişi yoktur; kullanıcı uygulamayı açtığında bir isim girer ve talepler/yanıtlar bu isimle oluşturulur.

---

## Genel Mimari ve Teknolojiler

- **Backend:** C#, .NET 10, ASP.NET Core Web API, RESTful Mimari, Swagger / OpenAPI
- **ORM ve Veritabanı:** Entity Framework Core (Code-First, Migration), PostgreSQL
- **IoC ve Bağımlılık Yönetimi:** Autofac Dependency Injection
- **Tasarım Desenleri:** Generic Repository Deseni, Result Deseni (IResult / IDataResult), DTO Deseni
- **İstemci:** .NET Console / Terminal Client (Asenkron HttpClient)

### Katmanlar

| Proje | Sorumluluk |
|---|---|
| `BasitDestekTalebiSistemi.Core` | Ortak altyapı: `IEntity`, `IDto`, generic repository (`IEntityRepository`, `EfEntityRepositoryBase`), Result sınıfları, `Messages` sabitleri |
| `BasitDestekTalebiSistemi.Entities` | `Ticket`, `Reply` entity'leri, `TicketStatus` enum'u, Request/Response DTO'ları |
| `BasitDestekTalebiSistemi.DataAccess` | `AppDbContext`, `EfTicketDal`, `EfReplyDal`, migration dosyaları |
| `BasitDestekTalebiSistemi.Business` | `TicketManager`, `ReplyManager` (validasyonlar ve iş kuralları), Autofac modülü |
| `BasitDestekTalebiSistemi.Api` | `TicketsController`, hata yakalama middleware'i, Swagger |
| `BasitDestekTalebiSistemi.Client` | `ApiClient` ve konsol ekranları |

### Endpoint'ler

| Metod | Route | Açıklama | Durum Kodları |
|---|---|---|---|
| POST | `/api/tickets` | Yeni talep oluşturur (açık durumda başlar) | 201 / 400 |
| GET | `/api/tickets` | Tüm talepleri listeler | 200 |
| GET | `/api/tickets/{id}` | Talep detayı (durum, yanıt sayısı, katılımcılar) | 200 / 404 |
| GET | `/api/tickets/{id}/replies` | Talebe ait yanıtları listeler | 200 / 404 |
| POST | `/api/tickets/{id}/replies` | Talebe yanıt ekler | 201 / 400 / 404 |
| PUT | `/api/tickets/{id}/close` | Talebi kapatır | 200 / 400 / 404 |

### İş Kuralları

- Kapalı bir talebe yeni yanıt eklenemez → `400 Bad Request` ("Kapalı bir talebe yeni yanıt eklenemez.")
- Zaten kapalı olan bir talep tekrar kapatılamaz → `400 Bad Request` ("Bu talep zaten kapalı, tekrar kapatılamaz.")
- Var olmayan bir talep üzerinde işlem yapılamaz → `404 Not Found`
- Başlık, açıklama, isim ve yanıt içeriği boş olamaz; uzunluk sınırları: başlık 100, açıklama 1000, isim 50, yanıt 500 karakter.

Kurallar `Business` katmanındaki Manager sınıflarında uygulanır. Console client aynı kontrolleri kullanıcıyı önceden uyarmak için ayrıca yapar.

---

## 1. Backend (Web API) Çalıştırma Talimatı

### Gereksinimler:
- .NET SDK 10
- PostgreSQL Veritabanı Sunucusu (Varsayılan port: 5432)
- EF Core komut satırı aracı (migration için): `dotnet tool install --global dotnet-ef`

### Veritabanı Yapılandırması:
Bağlantı bilgisi: `Host=localhost;Port=5432;Database=TicketSystemDb;Username=postgres;Password=postgres`

Bu bilgi iki yerde bulunur:
- `BasitDestekTalebiSistemi.DataAccess/Concrete/EntityFramework/AppDbContext.cs` → `OnConfiguring` metodu (uygulamanın ve migration'ların kullandığı bağlantı)
- `BasitDestekTalebiSistemi.Api/appsettings.json` → `ConnectionStrings:DefaultConnection`

PostgreSQL kullanıcı adınız veya şifreniz farklıysa iki yeri de güncelleyin.

### Migration Adımları:
1. PostgreSQL servisinin çalıştığından emin olun. Kapalıysa (Windows, PostgreSQL 16):
   ```powershell
   & "C:\Program Files\PostgreSQL\16\bin\pg_ctl.exe" start -D "C:\Program Files\PostgreSQL\16\data" -l "$env:TEMP\pg.log"
   ```
2. Proje ana dizinindeyken veritabanını ve tabloları oluşturun:
   ```bash
   dotnet ef database update --project BasitDestekTalebiSistemi.DataAccess --startup-project BasitDestekTalebiSistemi.Api
   ```
   Bu komut `TicketSystemDb` veritabanını, `Tickets` ve `Replies` tablolarını oluşturur.

### Çalıştırma Adımları:
1. Proje ana dizinindeyken şu komutu çalıştırın:
   ```bash
   dotnet run --project BasitDestekTalebiSistemi.Api
   ```
   *(Veya Visual Studio içerisinde `BasitDestekTalebiSistemi.Api` projesini başlangıç projesi olarak seçip F5 tuşuna basın.)*
2. Web API `http://0.0.0.0:5000` adresinde dinler (aynı bilgisayardan `http://localhost:5000`).
3. Swagger test arayüzü: `http://localhost:5000/swagger`

---

## 2. Console Client (Terminal İstemcisi) Çalıştırma Talimatı

### Çalıştırma Adımları:
1. Web API çalışır durumdayken yeni bir terminal penceresi açın ve şu komutu çalıştırın:
   ```bash
   dotnet run --project BasitDestekTalebiSistemi.Client
   ```
   *(Veya Visual Studio içerisinde `BasitDestekTalebiSistemi.Client` projesini çalıştırın.)*
2. Kullanıcı adınızı girin (boş olamaz, en fazla 50 karakter).
3. API bağlantı adresini girin:
   - Aynı bilgisayarda test ediyorsanız: Enter tuşuna basarak varsayılan adresi (`http://localhost:5000`) kabul edin.
   - Yerel ağdaki başka bir bilgisayardan bağlanıyorsanız: API'nin çalıştığı bilgisayarın yerel IP adresini girin (örneğin `192.168.1.107:5000`). Windows Güvenlik Duvarı'nda 5000 portuna izin verilmesi gerekebilir.
4. Ana menü:
   - `[1]` Talepleri Listele → listeden ID girerek talep detayına geçilir
   - `[2]` Yeni Talep Aç
   - `[3]` Çıkış
5. Talep detay ekranı:
   - `[1]` Yanıt Yaz (talep kapalıysa uyarı verir)
   - `[2]` Talebi Kapat (talep zaten kapalıysa uyarı verir)
   - `[0]` Ana Menüye Dön

API'ye ulaşılamazsa istemci çökmez; "Sunucuya bağlanılamadı..." mesajı gösterilir.

---

## 3. Örnek Kullanım Senaryosu

İki ayrı terminalde `Ahmet` ve `Mehmet` isimleriyle iki client açılır:

1. **Ahmet** `[2] Yeni Talep Aç` ile "Mail gelmiyor" başlıklı talebi açar ve yanıt yazar.
2. **Mehmet** `[1] Talepleri Listele` ekranında talebi görür, ID'sini girerek detaya geçer, yanıt yazar ve `[2] Talebi Kapat` ile talebi kapatır.
3. **Ahmet**'in ekranında talep hâlâ açık görünürken yanıt yazmaya çalışırsa API isteği reddeder ve ekran güncellenir.

Ekran çıktıları:

**Talep listesi (`[1] Talepleri Listele`):** Talepler ID, başlık, durum (yeşil "Acik" / kırmızı "Kapali"), açan kişi, tarih ve yanıt sayısıyla tablo halinde gösterilir.
```
--- DESTEK TALEPLERI ---
ID   Baslik                          Durum   Acan Kisi        Yanit
-------------------------------------------------------------------
3    Mail gelmiyor                   Acik    Ahmet                1
```

**Talep detayı:** Talebin bilgileri, yanıt sayısı, yanıt veren katılımcılar ve geçmiş yanıtlar tarih sırasıyla gösterilir.
```
==================================================
TALEP #3: Mail gelmiyor
==================================================
Durum       : Kapali
Acan Kisi   : Ahmet
Aciklama    : Sabahtan beri Outlook'a hiç mail düşmüyor.
Yanit Sayisi: 2
Katilimcilar: Ahmet, Mehmet

--- YANITLAR ---
Ahmet: Acil bakılmasını rica ediyorum.
Mehmet: Sunucuyu kontrol ettim, düzeldi.
```

**Kapalı talebe yanıt (diğer terminalde, API'nin döndürdüğü hata):**
```
[HATA] Kapalı bir talebe yeni yanıt eklenemez.
```

**Zaten kapalı talebi kapatma denemesi:**
```
[UYARI] Bu talep zaten kapalı, tekrar kapatılamaz.
```
