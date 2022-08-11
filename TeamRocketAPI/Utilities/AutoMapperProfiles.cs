using AutoMapper;
using TeamRocketAPI.DTOs;
using TeamRocketAPI.Entities;

namespace TeamRocketAPI.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //mapping configured from DTO to Entity
            CreateMap<PokemonCreateDTO, Pokemon>();
            //mapping configured from Entity to DTO
            CreateMap<Pokemon, PokemonDTO>();
        }    
    }
}
