using map.backend.shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace map.backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MapController : ControllerBase
    {
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<object>> getMap()
        {
            //try
            //{
            //    login_response res = new login_response();
            //    res = await _authRepository.loginAction(req);
            //    return Ok(res);
            //}
            //catch (Exception ex)
            //{
            //    message_response res = new message_response();
            //    res.resCode = "999";
            //    res.resDesc = ex.Message;
            //    return BadRequest(res);
            //}
            return Ok("abc");
        }
    }
}
