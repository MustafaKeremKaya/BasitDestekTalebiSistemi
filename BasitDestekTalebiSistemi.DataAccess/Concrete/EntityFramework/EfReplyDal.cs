using System.Collections.Generic;
using System.Linq;
using BasitDestekTalebiSistemi.Core.DataAccess.EntityFramework;
using BasitDestekTalebiSistemi.DataAccess.Abstract;
using BasitDestekTalebiSistemi.Entities.Concrete;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.DataAccess.Concrete.EntityFramework
{

    public class EfReplyDal : EfEntityRepositoryBase<Reply, AppDbContext>, IReplyDal
    {

        public List<ReplyResponse> GetRepliesByTicketId(int ticketId)
        {
            using (var context = new AppDbContext())
            {

                return context.Replies
                    .Where(r => r.TicketId == ticketId)
                    .OrderBy(r => r.CreatedAt)
                    .Select(r => new ReplyResponse
                    {
                        Id = r.Id,
                        TicketId = r.TicketId,
                        RepliedBy = r.RepliedBy,
                        Content = r.Content,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList();
            }
        }
    }
}
