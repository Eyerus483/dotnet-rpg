namespace dotnet_rpg.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly Mapper _mapper;
        public FightService(DataContext context, Mapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<ServiceRespone<FightResultDto>> Fight(FightRequestDto request)
        {
            var respone = new ServiceRespone<FightResultDto>
            {
                Data = new FightResultDto()
            };
            try
            {
                var characters = await _context.Characters
                .Include(c => c.Wepone)
                .Include(c => c.Skills)
                .Where(c => request.CharacterId.Contains(c.Id))
                .ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        int damage = 0;
                        string attackUsed = string.Empty;
                        bool useweapon = new Random().Next(2) == 0;
                        if (useweapon && attacker.Wepone is not null)
                        {
                            attackUsed = attacker.Wepone.Name;
                            damage = DoWeaponeAttack(attacker, opponent);
                        }
                        else if (!useweapon && attacker.Skills is not null)
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count())];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            respone.Data.Log
                            .Add($"{attacker.Name} wasn't able to attack!");
                            continue;
                        }
                        respone.Data.Log
                        .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage > 0 ? damage : 0)} damage");
                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            respone.Data.Log.Add($"{opponent.Name} has been defeated");
                            respone.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP lest");
                            break;
                        }
                    }
                }
                characters.ForEach(
                    c =>
                    {
                        c.Fight++;
                        c.HitPoints = 100;
                    }
                );
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }

        public async Task<ServiceRespone<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var respone = new ServiceRespone<AttackResultDto>();
            try
            {
                var attacher = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (attacher is null || opponent is null || attacher.Skills is null)
                {
                    throw new Exception("Something fishy is going here...");
                }
                var skill = attacher.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill is null)
                {
                    respone.Success = false;
                    respone.Message = $"{attacher.Name} doesn't know that skill";
                    return respone;
                }
                int damage = DoSkillAttack(attacher, opponent, skill);
                if (opponent.HitPoints <= 0)
                {
                    respone.Message = $"{opponent.Name} has been defeated";
                }
                await _context.SaveChangesAsync();
                respone.Data = new AttackResultDto
                {
                    Attacker = attacher.Name,
                    Opponent = opponent.Name,
                    AttackerHp = attacher.HitPoints,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage,
                };
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }

        private static int DoSkillAttack(Character attacher, Character opponent, Skill skill)
        {

            int damage = skill.Damage + (new Random().Next(attacher.Intelligence));
            damage -= new Random().Next(opponent.Defeats);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }
        public async Task<ServiceRespone<AttackResultDto>> WeaponAttack(WeaponAttachDto request)
        {
            var respone = new ServiceRespone<AttackResultDto>();
            try
            {
                var attacher = await _context.Characters
                .Include(c => c.Wepone)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (attacher is null || opponent is null || attacher.Wepone is null)
                {
                    throw new Exception("Something fishy is going here...");
                }
                int damage = DoWeaponeAttack(attacher, opponent);
                if (opponent.HitPoints <= 0)
                {
                    respone.Message = $"{opponent.Name} has been defeated";
                }
                await _context.SaveChangesAsync();
                respone.Data = new AttackResultDto
                {
                    Attacker = attacher.Name,
                    Opponent = opponent.Name,
                    AttackerHp = attacher.HitPoints,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage,
                };
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }

        private static int DoWeaponeAttack(Character attacher, Character opponent)
        {
            if (attacher.Wepone is null)
            {
                throw new Exception("attacker has no weapon");
            }
            int damage = attacher.Wepone.Damage + (new Random().Next(attacher.Strenght));
            damage -= new Random().Next(opponent.Defeats);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceRespone<List<HighScoreDto>>> GetHighScore()
        {
            var characters = await _context.Characters
            .Where(c => c.Fight > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats)
            .ToListAsync();
            var response = new ServiceRespone<List<HighScoreDto>>()
            {
                Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
            };
            return response;
        }
    }
}