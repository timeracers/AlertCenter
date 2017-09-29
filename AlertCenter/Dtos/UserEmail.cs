using System;

namespace AlertCenter.Dtos
{
    public class UserEmail
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }

        private UserEmail() { }

        public UserEmail(Guid userId, string email, string username)
        {
            Email = email;
            UserId = userId;
            Username = username;
        }
    }
}
