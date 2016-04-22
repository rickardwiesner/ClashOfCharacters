using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClashOfTheCharacters.Models
{
    public class Attack
    {
        public int Id { get; set; }

        public int Damage { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int BattleId { get; set; }
        public virtual Battle Battle { get; set; }
    }
}