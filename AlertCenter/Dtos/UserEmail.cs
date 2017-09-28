namespace AlertCenter.Dtos
{
    public class UserEmail
    {
        public string Email { get; set; }
        public string UserId { get; set; }

        private UserEmail() { }

        public UserEmail(string userId, string email)
        {
            Email = email;
            UserId = userId;
        }
    }
}
