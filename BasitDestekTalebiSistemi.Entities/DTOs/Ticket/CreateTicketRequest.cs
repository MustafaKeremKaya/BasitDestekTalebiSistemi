using BasitDestekTalebiSistemi.Core.Entities;

namespace BasitDestekTalebiSistemi.Entities.DTOs
{

    public class CreateTicketRequest : IDto
    {

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
    }
}
