namespace dotnet_rpg.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceRespone<AttackResultDto>> WeaponAttack(WeaponAttachDto request);
        Task<ServiceRespone<AttackResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceRespone<FightResultDto>> Fight(FightRequestDto request);
        Task<ServiceRespone<List<HighScoreDto>>> GetHighScore();
    }
}