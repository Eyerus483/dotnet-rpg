namespace dotnet_rpg.Controllers
{

     [ApiController]
     [Route("[controller]")]
    public class FightController : ControllerBase
    {

       
        private readonly IFightService _fightService;
        public FightController(IFightService fightService)
        {
            _fightService = fightService;

        }
        [HttpPost("WeaponAttack")]
        public async Task<ActionResult<ServiceRespone<AttackResultDto>>> WeaponAttack(WeaponAttachDto request)
        {
            return Ok(await _fightService.WeaponAttack(request));
        }

        [HttpPost("SkillAttack")]
        public async Task<ActionResult<ServiceRespone<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            return Ok(await _fightService.SkillAttack(request));
        }

       [HttpPost]
        public async Task<ActionResult<ServiceRespone<FightResultDto>>> Fight(FightRequestDto request)
        {
            return Ok(await _fightService.Fight(request));
        }

        [HttpGet]
        public async Task<ActionResult<ServiceRespone<List<HighScoreDto>>>> GetHighScore()
        {
            return Ok(await _fightService.GetHighScore());
        }
    }
    
}