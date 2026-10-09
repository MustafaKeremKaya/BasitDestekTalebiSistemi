using BasitDestekTalebiSistemi.Core.Entities;

namespace BasitDestekTalebiSistemi.Entities.DTOs
{

    public class CreateReplyRequest : IDto
    {

        public string RepliedBy { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
