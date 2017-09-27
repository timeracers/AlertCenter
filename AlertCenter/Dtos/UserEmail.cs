using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertCenter.Dtos
{
    public class UserEmail
    {
        public string Email { get; set; }
        public string UserId { get; set; }

        public UserEmail(string userId, string email)
        {
            Email = email;
            UserId = userId;
        }
    }
}
