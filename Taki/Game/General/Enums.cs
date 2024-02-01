using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;

namespace Taki.Game.General
{
    internal class Enums
    {
        public static Dictionary<UniqueCardEnum, string> UniqueTakiNames =
            new()
        {
            { UniqueCardEnum.Taki, "TAKI" },
            { UniqueCardEnum.ChangeColor, "Change-Color" },
            { UniqueCardEnum.Stop, "Stop" },
            { UniqueCardEnum.ChangeDirection, "Change-Direction" },
            { UniqueCardEnum.Plus, "Plus" },
            { UniqueCardEnum.Plus2, "Plus2" },
            { UniqueCardEnum.SuperTaki, "SUPER-TAKI" },
            { UniqueCardEnum.SwitchCardsWithDirection, "Switch-Cards-With-Direction"}
        };

        public static Dictionary<CardColorsEnum, Color> CardColors =
            new()
        {
            { CardColorsEnum.Green, Color.Green },
            { CardColorsEnum.Red, Color.Red },
            { CardColorsEnum.Yellow, Color.Yellow },
            { CardColorsEnum.Blue, Color.Blue },
        };
    }
}
