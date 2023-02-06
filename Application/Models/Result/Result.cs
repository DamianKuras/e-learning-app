namespace Application.Models.Result
{
    public class Result<T>
    {
        public List<String> Errors { get; set; } = new List<String>();

        public bool IsForbidden => Status == Status.Forbidden;
        public bool IsInvalid => Status == Status.Invalid;

        public bool IsNotFound => Status == Status.NotFound;
        public bool IsSucces => Status == Status.Succes;
        public bool IsUnauthorized => Status == Status.Unauthorized;
        public T Payload { get; set; }
        public Status Status { get; set; }

        public void SetAsForbidden()
        {
            Status = Status.Forbidden;
        }

        public void SetAsInvalid()
        {
            Status = Status.Invalid;
        }

        public void SetAsNotFound()
        {
            Status = Status.NotFound;
        }

        public void SetAsUnauthorized()
        {
            Status = Status.Unauthorized;
        }
    }
}