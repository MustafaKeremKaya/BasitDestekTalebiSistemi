using System;
using System.Collections.Generic;
using BasitDestekTalebiSistemi.Business.Abstract;
using BasitDestekTalebiSistemi.Core.Constants;
using BasitDestekTalebiSistemi.Core.Utilities.Results;
using BasitDestekTalebiSistemi.DataAccess.Abstract;
using BasitDestekTalebiSistemi.Entities.Concrete;
using BasitDestekTalebiSistemi.Entities.DTOs;
using BasitDestekTalebiSistemi.Entities.Enums;

namespace BasitDestekTalebiSistemi.Business.Concrete
{

    public class TicketManager : ITicketService
    {
        private readonly ITicketDal _ticketDal;

        public TicketManager(ITicketDal ticketDal)
        {
            _ticketDal = ticketDal;
        }

        public IDataResult<TicketDetailResponse> Create(CreateTicketRequest request)
        {

            if (request == null)
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.TicketRequestEmpty);
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.TicketTitleEmpty);
            }

            if (request.Title.Trim().Length > 100)
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.TicketTitleTooLong);
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.TicketDescriptionEmpty);
            }

            if (request.Description.Trim().Length > 1000)
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.TicketDescriptionTooLong);
            }

            if (string.IsNullOrWhiteSpace(request.CreatedBy))
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.CreatedByEmpty);
            }

            if (request.CreatedBy.Trim().Length > 50)
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.CreatedByTooLong);
            }

            var ticket = new Ticket
            {
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                CreatedBy = request.CreatedBy.Trim(),
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _ticketDal.Add(ticket);

            var response = new TicketDetailResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                CreatedBy = ticket.CreatedBy,
                CreatedAt = ticket.CreatedAt,
                ReplyCount = 0
            };

            return new SuccessDataResult<TicketDetailResponse>(response, Messages.TicketCreated);
        }

        public IDataResult<List<TicketSummaryResponse>> GetAll()
        {
            var responseList = _ticketDal.GetTicketSummaries();

            return new SuccessDataResult<List<TicketSummaryResponse>>(responseList, Messages.TicketsListed);
        }

        public IDataResult<TicketDetailResponse> GetById(int id)
        {
            var detail = _ticketDal.GetTicketDetail(id);

            if (detail == null)
            {
                return new ErrorDataResult<TicketDetailResponse>(Messages.TicketNotFound);
            }

            return new SuccessDataResult<TicketDetailResponse>(detail, Messages.TicketDetailListed);
        }

        public IResult CloseTicket(int id)
        {
            var ticket = _ticketDal.Get(t => t.Id == id);

            if (ticket == null)
            {
                return new ErrorResult(Messages.TicketNotFound);
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                return new ErrorResult(Messages.TicketAlreadyClosed);
            }

            ticket.Status = TicketStatus.Closed;

            _ticketDal.Update(ticket);

            return new SuccessResult(Messages.TicketClosed);
        }
    }
}
