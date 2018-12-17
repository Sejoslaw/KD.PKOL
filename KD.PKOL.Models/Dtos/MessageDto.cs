using System;

namespace KD.PKOL.Models.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public string ErrorTag { get; set; }
        public string ErrorMessage { get; set; }
    }
}
