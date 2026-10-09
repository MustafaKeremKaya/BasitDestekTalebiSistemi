using System.Collections.Generic;
using BasitDestekTalebiSistemi.Core.Utilities.Results;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Business.Abstract
{

    public interface ITicketService
    {

        IDataResult<TicketDetailResponse> Create(CreateTicketRequest request);

        IDataResult<List<TicketSummaryResponse>> GetAll();

        IDataResult<TicketDetailResponse> GetById(int id);

        IResult CloseTicket(int id);
    }
}
