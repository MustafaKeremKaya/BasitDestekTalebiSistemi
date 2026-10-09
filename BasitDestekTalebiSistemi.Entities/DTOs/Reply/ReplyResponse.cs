using System;
using BasitDestekTalebiSistemi.Core.Entities;

namespace BasitDestekTalebiSistemi.Entities.DTOs
{

    public class ReplyResponse : IDto
    {

        public int Id { get; set; }

        public int TicketId { get; set; }

        public string RepliedBy { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
