using System.Collections.Generic;
using BasitDestekTalebiSistemi.Core.DataAccess;
using BasitDestekTalebiSistemi.Entities.Concrete;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.DataAccess.Abstract
{

    public interface ITicketDal : IEntityRepository<Ticket>
    {

        List<TicketSummaryResponse> GetTicketSummaries();

        TicketDetailResponse? GetTicketDetail(int ticketId);
    }
}
