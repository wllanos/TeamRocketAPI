namespace TeamRocketAPI.DTOs
{
    public class PokemonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TipeOne { get; set; }
        public string TipeTwo { get; set; }
        public int Total { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAtk { get; set; }
        public int SpDef { get; set; }
        public int Speed { get; set; }
        public int Generation { get; set; }
        public bool Legendary { get; set; }
    }
}
