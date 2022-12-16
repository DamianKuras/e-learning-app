namespace Application.Dto
{
    public class LogedInUserDto
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}