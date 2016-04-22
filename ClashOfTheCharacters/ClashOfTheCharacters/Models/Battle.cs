using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClashOfTheCharacters.Models
{
    public class Battle
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }

        public virtual ICollection<Attack> Attacks { get; set; }
    }
}