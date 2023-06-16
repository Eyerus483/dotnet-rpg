

namespace dotnet_rpg.Services.WeaponService
{
    public interface IweaponService
    {
        Task<ServiceRespone<GetCharaterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}