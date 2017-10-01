using MailKit.Net.Smtp;
using System;

namespace AlertCenter
{
    public static class Config
    {
        public const string ConnectionString = "User ID=postgres;Password=postgresPassword;Host=localhost;Port=5432;Database=mydb;";
        public const string Secret = "This Secret Is SOOO secret it is writtin in plaintext";
        public const string EmailAddress = "example@gmail.com";
        public const string EmailName = "Alert Center";
        public static readonly Uri EmailService = new Uri("smtp://smtp.gmail.com:587");
        public static readonly Action<SmtpClient> EmailAuthenticate = (client) =>
        {
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate("example@gmail.com", "Drowssap Elbisseugnu");
        } ;
    }
}
