using System;
using System.Collections.Generic;
using BasitDestekTalebiSistemi.Core.Entities;
using BasitDestekTalebiSistemi.Entities.Enums;

namespace BasitDestekTalebiSistemi.Entities.Concrete
{

    public class Ticket : IEntity
    {

        public Ticket()
        {
            Replies = new List<Reply>();
            Status = TicketStatus.Open;
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public TicketStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }
    }
}
