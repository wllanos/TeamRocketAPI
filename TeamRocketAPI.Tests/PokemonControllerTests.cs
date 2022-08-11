using AutoMapper;
using Castle.Core.Configuration;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamRocketAPI.Controllers;
using TeamRocketAPI.DTOs;
using TeamRocketAPI.Entities;
using TeamRocketAPI.Utilities;

namespace TeamRocketAPI.Tests
{
    public class PokemonControllerTests
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        public PokemonControllerTests()
        {
            context = A.Fake<ApplicationDBContext>();
            mapper = A.Fake<IMapper>();
            configuration = A.Fake<Microsoft.Extensions.Configuration.IConfiguration>();
        }

        [Fact]
        public void PokemonController_Get_Pokemon_Return_List()
        {
            //Arrange
            var pokemon = A.Fake<ICollection<PokemonDTO>>();
            var list = A.Fake<List<PokemonDTO>>();
            A.CallTo(() => mapper.Map<List<PokemonDTO>>(pokemon)).Returns(list);
            var controller = new PokemonController(context, mapper, configuration);
            //Act
            var paginationDTO = A.Fake<PaginationDTO>();
            var result = controller.Get(paginationDTO);
            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void PokemonController_Create_Pokemon_Return_Route()
        {
            //Arrange
            var pokemon = A.Fake<Pokemon>();
            var pokemonCreate = A.Fake<PokemonCreateDTO>();        
            A.CallTo(() => mapper.Map<Pokemon>(pokemonCreate)).Returns(pokemon);
            var controller = new PokemonController(context, mapper, configuration);
        //    //Act
            var result = controller.Post(pokemonCreate);
        //    //Assert
            result.Should().NotBeNull();
        }

        

    }
}
