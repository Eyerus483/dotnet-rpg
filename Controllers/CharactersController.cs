

using System.Collections.Generic;
using System.Security.Claims;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_rpg.Controllers;
[Authorize]
[ApiController]
[Route("Api/[Controller]")]


    public class CharactersController : ControllerBase
{
   
    private readonly ICharacterService _characterService;

    public CharactersController(ICharacterService characterService)
{
        _characterService = characterService;
    }
    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceRespone<List<GetCharaterDto>>>> Get()
    {
        
        return Ok(await _characterService.GetAllCharacter());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceRespone<GetCharaterDto>>> GetSingle(int id)
    {
        return Ok(await _characterService.GetCharacterById(id));
    }
    [HttpPost]
    public async Task<ActionResult<ServiceRespone<List<GetCharaterDto>>>> AddCharater(AddCharacterDto charater){
        return Ok(await _characterService.AddCharacter(charater));
    }

    [HttpPut]
    public async Task<ActionResult<ServiceRespone<GetCharaterDto>>> updatedCharacter(UpdateCharacterDto updatedCharacter){
        var response = await _characterService.UpdateCharacter(updatedCharacter);
        if(response.Data is null)
        {
         return  NotFound(response);
        }
        return Ok();
    }

   [HttpDelete]
    public async Task<ActionResult<ServiceRespone<List<GetCharaterDto>>>> DeleteCharacter(int id){
        var response = await _characterService.DeleteCharacter(id);
        if(response.Data == null)
        {
         return  NotFound(response);
        }
        return Ok();
    }
    [HttpPost("skill")]

    public async Task<ActionResult<ServiceRespone<GetCharaterDto>>> AddCharacterSkill(AddCharacterSkillDto newcharacterSkill)
    {
        return Ok(await _characterService.AddCharacterSkill(newcharacterSkill));
    }
}
