using map.backend.shared.DTO;
using map.backend.shared.Entities.Auth;
using map.backend.shared.Interfaces.Auth;
using map.backend.shared.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace map.backend.shared.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _iconfiguration;
        public AuthRepository(IUnitOfWork unitOfWork, IConfiguration iconfiguration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _iconfiguration = iconfiguration ?? throw new ArgumentNullException(nameof(iconfiguration));
        }
        public async Task<login_response> loginAction(login_request req)
        {
            login_response res = new login_response();
            var _userRepository = _unitOfWork.GetRepository<tb_user>(true);
            var _user = await _userRepository.GetFirstOrDefaultAsync(predicate: o => o.userid == req.userid
                                                                                  && o.record_stat == "O");
            if (_user == null)
                throw new Exception("U - Thông tin đăng nhập không chính xác không chính xác!");
            else if (_user.status == "C")
                throw new Exception("Tài khoản đã bị khóa1");
            bool verified = BCrypt.Net.BCrypt.Verify(req.password, _user.password);
            if (!verified)
                throw new Exception("P - Thông tin đăng nhập không chính xác không chính xác!");
            _user.limit = 5;
            await _userRepository.UpdateAsync(_user);
            //Tạo token đăng nhập
            var tokenHandler = new JwtSecurityTokenHandler();
            var aaa = _iconfiguration["JWT:Key"];
            var key = Encoding.ASCII.GetBytes(_iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserId", _user.userid == null ? "" : _user.userid),
                        new Claim("UserName", _user.username == null ? "" : _user.username),
                        new Claim("Email", _user.email == null ? "" : _user.email),
                        new Claim("PhoneNumber", _user.phone == null ? "" : _user.phone),
                }),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            res.token = token;
            res.img = _user.img;
            return res;
        }
    }
}
