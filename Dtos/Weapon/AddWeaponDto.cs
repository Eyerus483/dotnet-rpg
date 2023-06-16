namespace dotnet_rpg.Dtos.Weapon
{
    public class AddWeaponDto
    {
        public string Name { get; set; } = string.Empty;
        public int Damange { get; set; }
        public int CharacterId { get; set; }
    }
}