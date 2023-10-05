using map.backend.shared.DTO;
using map.backend.shared.Interfaces.Auth;
using map.backend.shared.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Net;

namespace map.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }
        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<object>> LoginApp([FromBody] login_request req)
        {
            try
            {
                login_response res = new login_response();
                res = await _authRepository.loginAction(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                message_response res = new message_response();
                res.resCode = "999";
                res.resDesc = ex.Message;
                return BadRequest(res);
            }
        }
    }
}
