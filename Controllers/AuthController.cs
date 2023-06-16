
using dotnet_rpg.Dtos.User;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_rpg.Controllers
{
   
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        public readonly IAuthRepository _AuthRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _AuthRepository = authRepository;

        }
[HttpPost("Register")]
        public async Task<ActionResult<ServiceRespone<int>>> Register(UserRegisterDto request){
            var respone = await _AuthRepository.Register(new User { UserName = request.UserName }, request.Password);
            if(!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);

        }
        [HttpPost("Login")]
         public async Task<ActionResult<ServiceRespone<string>>> Login(UserLoginDto request){
        var respone = await _AuthRepository.Login(request.UserName, request.Password);
        if(!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);

      }
    }

}