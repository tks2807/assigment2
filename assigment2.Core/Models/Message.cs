using System;
namespace assigment2.Core.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string JsonContent { get; set; }
        public bool Handled { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime? HandleTime { get; set; }
    }
}
