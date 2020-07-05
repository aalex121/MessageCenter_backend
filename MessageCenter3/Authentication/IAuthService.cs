using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Authentication
{
    public interface IAuthService
    {
        AuthToken GetAuthToken(AuthRequest request);
    }
}
