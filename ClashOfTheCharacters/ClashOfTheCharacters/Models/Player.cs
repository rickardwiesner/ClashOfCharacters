using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClashOfTheCharacters.Models
{
    public class Player
    {
        public int Id { get; set; }

        public int Stamina { get; set; }
        public int Gold { get; set; }
        public int Rank { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}