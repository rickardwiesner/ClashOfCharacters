using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClashOfTheCharacters.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

        public int Level { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int Attack
        {
            get
            {
                return Convert.ToInt32(Level * Character.AttackMultiplier + Character.BaseAttack);
            }
        }

        public int Defense
        {
            get
            {
                return Convert.ToInt32(Level * Character.DefenseMultiplier + Character.BaseDefense);
            }
        }

        public int Hp
        {
            get
            {
                return Convert.ToInt32(Level * Character.HpMultiplier + Character.BaseHp);
            }
        }
    }
}