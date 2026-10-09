using System.Collections.Generic;
using BasitDestekTalebiSistemi.Core.Utilities.Results;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Business.Abstract
{

    public interface IReplyService
    {

        IDataResult<ReplyResponse> AddReply(int ticketId, CreateReplyRequest request);

        IDataResult<List<ReplyResponse>> GetRepliesByTicketId(int ticketId);
    }
}
