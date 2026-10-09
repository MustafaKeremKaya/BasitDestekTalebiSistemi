namespace BasitDestekTalebiSistemi.Core.Constants
{

    public static class Messages
    {

        public static readonly string TicketRequestEmpty = "Talep isteği boş olamaz.";
        public static readonly string TicketTitleEmpty = "Talep başlığı boş bırakılamaz.";
        public static readonly string TicketTitleTooLong = "Talep başlığı 100 karakterden uzun olamaz.";
        public static readonly string TicketDescriptionEmpty = "Talep açıklaması boş bırakılamaz.";
        public static readonly string TicketDescriptionTooLong = "Talep açıklaması 1000 karakterden uzun olamaz.";
        public static readonly string CreatedByEmpty = "Talebi açan kişi bilgisi boş bırakılamaz.";
        public static readonly string CreatedByTooLong = "Talebi açan kişinin ismi 50 karakterden uzun olamaz.";
        public static readonly string TicketCreated = "Talep başarıyla oluşturuldu.";
        public static readonly string TicketsListed = "Talepler başarıyla listelendi.";
        public static readonly string TicketDetailListed = "Talep detayı başarıyla getirildi.";
        public static readonly string TicketNotFound = "Belirtilen ID'ye sahip talep bulunamadı.";
        public static readonly string TicketForReplyNotFound = "Yanıt eklenecek talep bulunamadı.";
        public static readonly string TicketAlreadyClosed = "Bu talep zaten kapalı, tekrar kapatılamaz.";
        public static readonly string TicketClosed = "Talep başarıyla kapatıldı.";

        public static readonly string ReplyRequestEmpty = "Yanıt isteği boş olamaz.";
        public static readonly string RepliedByEmpty = "Yanıtı yazan kişi bilgisi boş bırakılamaz.";
        public static readonly string RepliedByTooLong = "Yanıtı yazan kişinin ismi 50 karakterden uzun olamaz.";
        public static readonly string ContentEmpty = "Yanıt içeriği boş bırakılamaz.";
        public static readonly string ContentTooLong = "Yanıt içeriği 500 karakterden uzun olamaz.";
        public static readonly string ReplyToClosedTicket = "Kapalı bir talebe yeni yanıt eklenemez.";
        public static readonly string ReplyAdded = "Yanıt başarıyla eklendi.";
        public static readonly string RepliesListed = "Yanıtlar başarıyla listelendi.";

        public static readonly string ServerConnectionError = "Sunucuya bağlanılamadı. Lütfen API'nin çalıştığından ve doğru adreste olduğundan emin olun.";
        public static readonly string ServerTimeout = "Sunucu yanıt vermedi. Lütfen ağ bağlantınızı kontrol edin.";
        public static readonly string EmptyResponse = "Sunucudan boş yanıt döndü.";

        public static readonly string NotFound404 = "İstenen kayıt veya talep bulunamadı.";
        public static readonly string BadRequest400 = "Gönderilen bilgiler geçersiz veya eksik.";
        public static readonly string InternalServerError500 = "Sunucuda içsel bir hata oluştu.";
        public static readonly string UnexpectedError = "Sunucuda beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
    }
}
