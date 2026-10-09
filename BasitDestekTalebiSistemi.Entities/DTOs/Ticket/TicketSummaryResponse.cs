using System;
using BasitDestekTalebiSistemi.Core.Entities;
using BasitDestekTalebiSistemi.Entities.Enums;

namespace BasitDestekTalebiSistemi.Entities.DTOs
{

    public class TicketSummaryResponse : IDto
    {

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public TicketStatus Status { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int ReplyCount { get; set; }
    }
}
