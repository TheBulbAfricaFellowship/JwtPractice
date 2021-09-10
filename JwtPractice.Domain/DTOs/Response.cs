namespace JwtPractice.Domain.DTOs
{
    public class Response
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public dynamic Payload { get; set; }
    }
}
