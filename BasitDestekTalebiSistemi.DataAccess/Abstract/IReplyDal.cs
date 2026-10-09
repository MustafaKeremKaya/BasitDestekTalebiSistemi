using System.Collections.Generic;
using BasitDestekTalebiSistemi.Core.DataAccess;
using BasitDestekTalebiSistemi.Entities.Concrete;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.DataAccess.Abstract
{

    public interface IReplyDal : IEntityRepository<Reply>
    {

        List<ReplyResponse> GetRepliesByTicketId(int ticketId);
    }
}
