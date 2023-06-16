



namespace dotnet_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceRespone<List<GetCharaterDto>>> GetAllCharacter();
        Task<ServiceRespone<GetCharaterDto>> GetCharacterById(int id);
        Task<ServiceRespone<List<GetCharaterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceRespone<GetCharaterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
        Task<ServiceRespone<List<GetCharaterDto>>> DeleteCharacter(int id);
        Task<ServiceRespone<GetCharaterDto>> AddCharacterSkill(AddCharacterSkillDto newcharacterskill);

    }
}