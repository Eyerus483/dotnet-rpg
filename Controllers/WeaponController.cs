
using Microsoft.AspNetCore.Authorization;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IweaponService _weaponService;
        public WeaponController(IweaponService weaponService)
        {
            _weaponService = weaponService;

        }
        [HttpPost]
        public async Task<ActionResult<ServiceRespone<GetCharaterDto>>> AddWeapon(AddWeaponDto newWeapon) {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}