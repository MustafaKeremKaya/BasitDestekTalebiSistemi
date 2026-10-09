using System;
using System.Collections.Generic;
using System.Linq;
using BasitDestekTalebiSistemi.Business.Abstract;
using BasitDestekTalebiSistemi.Core.Constants;
using BasitDestekTalebiSistemi.Core.Utilities.Results;
using BasitDestekTalebiSistemi.DataAccess.Abstract;
using BasitDestekTalebiSistemi.Entities.Concrete;
using BasitDestekTalebiSistemi.Entities.DTOs;
using BasitDestekTalebiSistemi.Entities.Enums;

namespace BasitDestekTalebiSistemi.Business.Concrete
{

    public class ReplyManager : IReplyService
    {
        private readonly IReplyDal _replyDal;
        private readonly ITicketDal _ticketDal;

        public ReplyManager(IReplyDal replyDal, ITicketDal ticketDal)
        {
            _replyDal = replyDal;
            _ticketDal = ticketDal;
        }

        public IDataResult<ReplyResponse> AddReply(int ticketId, CreateReplyRequest request)
        {

            if (request == null)
            {
                return new ErrorDataResult<ReplyResponse>(Messages.ReplyRequestEmpty);
            }

            if (string.IsNullOrWhiteSpace(request.RepliedBy))
            {
                return new ErrorDataResult<ReplyResponse>(Messages.RepliedByEmpty);
            }

            if (request.RepliedBy.Trim().Length > 50)
            {
                return new ErrorDataResult<ReplyResponse>(Messages.RepliedByTooLong);
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return new ErrorDataResult<ReplyResponse>(Messages.ContentEmpty);
            }

            if (request.Content.Trim().Length > 500)
            {
                return new ErrorDataResult<ReplyResponse>(Messages.ContentTooLong);
            }

            var ticket = _ticketDal.Get(t => t.Id == ticketId);
            if (ticket == null)
            {
                return new ErrorDataResult<ReplyResponse>(Messages.TicketForReplyNotFound);
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                return new ErrorDataResult<ReplyResponse>(Messages.ReplyToClosedTicket);
            }

            var reply = new Reply
            {
                TicketId = ticketId,
                RepliedBy = request.RepliedBy.Trim(),
                Content = request.Content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _replyDal.Add(reply);

            var response = new ReplyResponse
            {
                Id = reply.Id,
                TicketId = reply.TicketId,
                RepliedBy = reply.RepliedBy,
                Content = reply.Content,
                CreatedAt = reply.CreatedAt
            };

            return new SuccessDataResult<ReplyResponse>(response, Messages.ReplyAdded);
        }

        public IDataResult<List<ReplyResponse>> GetRepliesByTicketId(int ticketId)
        {

            var ticket = _ticketDal.Get(t => t.Id == ticketId);
            if (ticket == null)
            {
                return new ErrorDataResult<List<ReplyResponse>>(Messages.TicketNotFound);
            }

            var replies = _replyDal.GetAll(r => r.TicketId == ticketId);

            var responseList = replies
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

            return new SuccessDataResult<List<ReplyResponse>>(responseList, Messages.RepliesListed);
        }
    }
}
