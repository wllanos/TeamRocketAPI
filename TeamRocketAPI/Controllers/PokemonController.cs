using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamRocketAPI.DTOs;
using TeamRocketAPI.Entities;
using TeamRocketAPI.Utilities;

namespace TeamRocketAPI.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PokemonController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public PokemonController(ApplicationDBContext context, IMapper mapper
                , IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get paginated Pokemon list 
        /// </summary>
        /// <returns></returns>
        [HttpGet]//api/pokemon
        public async Task<ActionResult<List<PokemonDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var query = context.Pokemon.AsQueryable();
            await HttpContext.InsertPaginationParametersInHeader(query);
            var pokemon = await query.OrderBy(pokemon => pokemon.Id).Pagination(paginationDTO).ToListAsync();

            if (pokemon == null)
            {
                return NotFound();
            }

            return mapper.Map<List<PokemonDTO>>(pokemon);
        }
        /// <summary>
        /// Get Pokemon by Id
        /// </summary>
        /// <param name="id">Pokemon Id</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "getPokemon")]//api/pokemon/id
        public async Task<ActionResult<PokemonDTO>> Get(int id)
        {
  
            var pokemon = await context.Pokemon.FirstOrDefaultAsync(x => x.Id == id);
            
            if (pokemon == null)
            {
                return NotFound();
            }

            return mapper.Map<PokemonDTO>(pokemon);
        }

        /// <summary>
        /// Create Pokemon
        /// </summary>
        /// <param name="pokemonCreateDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PokemonCreateDTO pokemonCreateDTO)
        {
            var exist = await context.Pokemon.AnyAsync(x => x.Name.ToUpper().TrimStart().TrimEnd() 
                == pokemonCreateDTO.Name.ToUpper().TrimStart().TrimEnd());

            if (exist)
                return BadRequest($"Name: {pokemonCreateDTO.Name} has been already created");
            //mapping from source(DTO) to destination(Entity)
            var pokemon = mapper.Map<Pokemon>(pokemonCreateDTO);

            context.Add(pokemon);
            await context.SaveChangesAsync();

            var pokemonDTO = mapper.Map<PokemonDTO>(pokemon);

            return CreatedAtRoute("getPokemon", new { id = pokemon.Id }, pokemonDTO);
        }

        /// <summary>
        /// Update pokemon
        /// </summary>
        /// <param name="pokemonCreateDTO"></param>
        /// <param name="id">Pokemon Id</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]//api/pokemon/1
        public async Task<ActionResult> Put([FromBody] PokemonCreateDTO pokemonCreateDTO, [FromRoute] int id)
        {
            var exist = await context.Pokemon.AnyAsync(x => x.Id == id);

            if (!exist)
                return NotFound();

            var pokemon = mapper.Map<Pokemon>(pokemonCreateDTO);
            pokemon.Id = id;

            context.Update(pokemon);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete Pokemon
        /// </summary>
        /// <param name="id">Pokemon Id</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]//api/pokemon/1
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var exist = await context.Pokemon.AnyAsync(x => x.Id == id);

            if (!exist)
                return NotFound();

            context.Remove(new Pokemon {Id = id});
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
