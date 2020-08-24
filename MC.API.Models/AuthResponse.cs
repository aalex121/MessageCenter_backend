using Microsoft.Exchange.WebServices.Auth.Validation;
using System;

namespace MC.API.Models
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
