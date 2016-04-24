using ClashOfTheCharacters.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClashOfTheCharacters.Models
{
    public class Attack
    {
        public int Id { get; set; }
        public int Damage { get; set; }
        public int HpRemaining { get; set; }
        public Effect Effect { get; set; }

        //public string ApplicationUserId { get; set; }
        //public virtual ApplicationUser User { get; set; }

        public int BattleId { get; set; }
        public virtual Battle Battle { get; set; }

        public int? AttackerId { get; set; }
        public virtual TeamMember Attacker { get; set; }

        public int? DefenderId { get; set; }
        public virtual TeamMember Defender { get; set; }
    }
}