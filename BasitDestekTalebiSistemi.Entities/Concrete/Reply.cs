using System;
using BasitDestekTalebiSistemi.Core.Entities;

namespace BasitDestekTalebiSistemi.Entities.Concrete
{

    public class Reply : IEntity
    {

        public Reply()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public int TicketId { get; set; }

        public string RepliedBy { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }
    }
}
