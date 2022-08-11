using System.ComponentModel.DataAnnotations;

namespace TeamRocketAPI.DTOs
{
    public class PokemonCreateDTO
    {
        public string Name { get; set; }
        [Required]
        public string TipeOne { get; set; }
        public string TipeTwo { get; set; }
        [Required]
        [Range(1, 999)]
        public int Total { get; set; }
        [Required]
        [Range(1, 999)]
        public int HP { get; set; }
        [Required]
        [Range(1, 999)]
        public int Attack { get; set; }
        [Required]
        [Range(1, 999)]
        public int Defense { get; set; }
        [Required]
        [Range(1, 999)]
        public int SpAtk { get; set; }
        [Required]
        [Range(1, 999)]
        public int SpDef { get; set; }
        [Required]
        [Range(1, 999)]
        public int Speed { get; set; }
        [Required]
        [Range(1, 9)]
        public int Generation { get; set; }
        [Required]
        public bool Legendary { get; set; }
    }
}
