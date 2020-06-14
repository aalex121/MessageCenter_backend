using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MessageCenter3.Authentication
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        const string KEY = "topsecret_FBI_X_files_Area_51";
        public const int LIFETIME = 15;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
