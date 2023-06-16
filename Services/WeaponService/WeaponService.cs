using System.Security.Claims;
using AutoMapper;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IweaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceRespone<GetCharaterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var respone = new ServiceRespone<GetCharaterDto>();
            try
            {
                var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User!.Id == GetUserId());
                if(character is null){
                    respone.Success = false;
                    respone.Message = "Character is not found";
                    return respone;
                }
                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damange,
                    Character = character,

                };
                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();

                respone.Data = _mapper.Map<GetCharaterDto>(character);
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }
    }
}