namespace dotnet_rpg.Dtos.Character
{
    public class AddCharacterDto
    {
    public string Name { get; set; } = "Abebe";
    public int HitPoints { get; set; } = 100;
    public int Defence { get; set; }= 10;
    public int Strenght { get; set; } = 10;
    public int Intelligence { get; set; }
    public RpgClass Class { get; set; } = RpgClass.Cleric;
    }
}