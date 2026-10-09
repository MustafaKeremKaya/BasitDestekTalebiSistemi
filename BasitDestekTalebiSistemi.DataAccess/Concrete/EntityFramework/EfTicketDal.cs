using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BasitDestekTalebiSistemi.Core.DataAccess.EntityFramework;
using BasitDestekTalebiSistemi.DataAccess.Abstract;
using BasitDestekTalebiSistemi.Entities.Concrete;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.DataAccess.Concrete.EntityFramework
{

    public class EfTicketDal : EfEntityRepositoryBase<Ticket, AppDbContext>, ITicketDal
    {

        public List<TicketSummaryResponse> GetTicketSummaries()
        {
            using (var context = new AppDbContext())
            {

                return context.Tickets
                    .OrderBy(t => t.CreatedAt)
                    .Select(t => new TicketSummaryResponse
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status,
                        CreatedBy = t.CreatedBy,
                        CreatedAt = t.CreatedAt,
                        ReplyCount = t.Replies.Count
                    })
                    .ToList();
            }
        }

        public TicketDetailResponse? GetTicketDetail(int ticketId)
        {
            using (var context = new AppDbContext())
            {

                var ticket = context.Tickets
                    .Include(t => t.Replies)
                    .SingleOrDefault(t => t.Id == ticketId);

                if (ticket == null)
                {
                    return null;
                }

                return new TicketDetailResponse
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Status = ticket.Status,
                    CreatedBy = ticket.CreatedBy,
                    CreatedAt = ticket.CreatedAt,
                    ReplyCount = ticket.Replies.Count,
                    Participants = ticket.Replies
                        .Select(r => r.RepliedBy)
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Distinct()
                        .ToList()
                };
            }
        }
    }
}
