using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClashOfTheCharacters.Helpers
{
    public class Helper
    {
        static public string GetElementUrl(Element element)
        {
            switch (element)
            {
                case Element.Fire:
                    return "/Images/Elements/fire.png";

                case Element.Air:
                    return "/Images/Elements/air.png";

                case Element.Earth:
                    return "/Images/Elements/earth.png";

                case Element.Water:
                    return "/Images/Elements/water.png";

                default:
                    return string.Empty;
            }
        }

        static public string GetPercentage(int xp, int maxXp)
        {
            return Convert.ToString(Convert.ToDecimal(xp) / Convert.ToDecimal(maxXp) * 100).Replace(",", ".");
        }
    }
}