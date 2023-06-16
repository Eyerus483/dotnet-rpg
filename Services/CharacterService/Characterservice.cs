

using System.Security.Claims;

namespace dotnet_rpg.Services.CharacterService
{
    public class Characterservice : ICharacterService
    {
        // private static List<Character> Charaters = new List<Character>{
        // new Character(),
        // new Character{Id = 1,Name = "Kebede"},
        // };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Characterservice(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceRespone<List<GetCharaterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceRespone = new ServiceRespone<List<GetCharaterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
             character.User = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceRespone.Data = await _context.Characters
            .Where(c => c.User!.Id == GetUserId())
            .Select(c => _mapper.Map<GetCharaterDto>(c)).ToListAsync();
            return serviceRespone;


        }

        public async Task<ServiceRespone<List<GetCharaterDto>>> DeleteCharacter(int id)
        {
            var serviceRespone = new ServiceRespone<List<GetCharaterDto>>();
            try
            {

                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (dbCharacter == null)
                {
                    throw new Exception($"Character with id '{id}' is not found");
                }

                _context.Characters.Remove(dbCharacter);
                await _context.SaveChangesAsync();

                serviceRespone.Data = await _context.Characters
                .Where (c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharaterDto>(c)).ToListAsync();

            }
            catch (Exception ex)
            {
                serviceRespone.Success = false;
                serviceRespone.Message = ex.Message;
            }
            return serviceRespone;
        }
        public async Task<ServiceRespone<List<GetCharaterDto>>> GetAllCharacter()
        {
            var serviceRespone = new ServiceRespone<List<GetCharaterDto>>();
            var dbCharacter = await _context.Characters
            .Include(c=>c.Wepone)
            .Include(c=> c.Skills)
            .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceRespone.Data = dbCharacter.Select(c => _mapper.Map<GetCharaterDto>(c)).ToList();
            return serviceRespone;
        }

        public async Task<ServiceRespone<GetCharaterDto>> GetCharacterById(int id)
        {
            var serviceRespone = new ServiceRespone<GetCharaterDto>();
            var dbcharacter = await _context.Characters
            .Include(c=>c.Wepone)
            .Include(c=> c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            if (dbcharacter == null)
            {
                serviceRespone.Success = false;
                serviceRespone.Message = $"Character with id '{id}' is not found";
            }
            serviceRespone.Data = _mapper.Map<GetCharaterDto>(dbcharacter);

            return serviceRespone;
        }

        public async Task<ServiceRespone<GetCharaterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceRespone = new ServiceRespone<GetCharaterDto>();
            try
            {

                var dbCharacter = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if (dbCharacter == null || dbCharacter.User!.Id != GetUserId())
                {
                    throw new Exception($"Character with id '{updatedCharacter.Id}' is not found");
                }

                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Strenght = updatedCharacter.Strenght;
                dbCharacter.Defence = updatedCharacter.Defence;
                dbCharacter.Intelligence = updatedCharacter.Intelligence;
                dbCharacter.Class = updatedCharacter.Class;

                // alternetive options using auto mapper
                // character = _mapper.Map<Character>(updatedCharacter);
                // _mapper.Map(updatedCharacter, character);

                serviceRespone.Data = _mapper.Map<GetCharaterDto>(dbCharacter);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceRespone.Success = false;
                serviceRespone.Message = ex.Message;
            }
            return serviceRespone;
        }

        public async Task<ServiceRespone<GetCharaterDto>> AddCharacterSkill(AddCharacterSkillDto newcharacterskill)
        {
            var respone = new ServiceRespone<GetCharaterDto>();
            try
            {
                var character = await _context.Characters
                .Include(c=>c.Wepone)
                .Include(c=> c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newcharacterskill.CharacterId && c.User!.Id == GetUserId());
                if(character is null)
                {
                    respone.Success = false;
                    respone.Message = "Character not found";
                    return respone;
                }
                var skill = await _context.Skills
                .FirstOrDefaultAsync(c => c.Id == newcharacterskill.SkillId);
                   if(skill is null)
                {
                    respone.Success = false;
                    respone.Message = "Skill not found";
                    return respone;
                }
                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                respone.Data = _mapper.Map<GetCharaterDto>(character);
            }
            catch(Exception ex){
                respone.Success = false;
                respone.Message = ex.Message;
            }
             return respone;   
        }
    }
}

