using Application.Enums;

namespace Application.Models
{
    public class Result<T>
    {
        public List<string> Errors { get; set; } = new List<string>();
        public ErrorType ErrorType { get; set; }
        public bool IsError { get; set; }
        public T Payload { get; set; }
    }
}