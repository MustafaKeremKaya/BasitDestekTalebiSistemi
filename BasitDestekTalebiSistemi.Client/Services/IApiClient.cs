using System.Collections.Generic;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Client.Services
{

    public interface IApiClient
    {

        Task<ApiResponse<List<TicketSummaryResponse>>> GetTicketsAsync();

        Task<ApiResponse<TicketDetailResponse>> GetTicketDetailAsync(int id);

        Task<ApiResponse<TicketDetailResponse>> CreateTicketAsync(CreateTicketRequest request);

        Task<ApiResponse<List<ReplyResponse>>> GetRepliesAsync(int ticketId);

        Task<ApiResponse<ReplyResponse>> AddReplyAsync(int ticketId, CreateReplyRequest request);

        Task<ApiResponse<object>> CloseTicketAsync(int ticketId);
    }
}
