

namespace dotnet_rpg.Dtos.Character
{
    public class GetCharaterDto
    {
    public int Id { get; set; }
    public string Name { get; set; } = "Abebe";
    public int HitPoints { get; set; } = 100;
    public int Defence { get; set; }= 10;
    public int Strenght { get; set; } = 10;
    public int Intelligence { get; set; }
    public RpgClass Class { get; set; } = RpgClass.Cleric; 
    public GetWeaponDto? Weapon { get; set; }
    public List<GetSkillDto>? Skills { get; set; }
    public int Fight { get; set; }
    public int Victoris { get; set; }
    public int Defeats { get; set; }
    }
}