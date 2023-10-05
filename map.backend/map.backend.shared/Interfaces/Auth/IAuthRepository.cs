using map.backend.shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map.backend.shared.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<login_response> loginAction(login_request req);
    }
}
