

namespace dotnet_rpg.Model
{
    public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = "Abebe";
    public int HitPoints { get; set; } = 100;
    public int Defence { get; set; }= 10;
    public int Strenght { get; set; } = 10;
    public int Intelligence { get; set; }
    public RpgClass Class { get; set; } = RpgClass.Cleric;
    public User? User { get; set; }
    public Weapon? Weapon { get; set; }
    public List<Skill>? Skills { get; set; }
    public int Fight { get; set; }
    public int Victories { get; set; }
    public int Defeats { get; set; }
}
    
}