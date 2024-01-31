using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki.Game.General
{
    internal class Enums
    {
        public static Dictionary<UniqueCardEnum, string> UniqueTakiNames =
            new Dictionary<UniqueCardEnum, string>()
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
            new Dictionary<CardColorsEnum, Color>()
        {
            { CardColorsEnum.Green, Color.Green },
            { CardColorsEnum.Red, Color.Red },
            { CardColorsEnum.Yellow, Color.Yellow },
            { CardColorsEnum.Blue, Color.Blue },
        };
    }

    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    enum UniqueCardEnum
    {
        Taki,//work
        ChangeColor,//work
        Stop,//work
        ChangeDirection,//work
        Plus,//work, error with multiple plus
        Plus2,//work 
        SuperTaki,//work
        SwitchCardsWithDirection//work
    }

    enum CardColorsEnum
    {
        Green,
        Red,
        Yellow,
        Blue
    }
}
