using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.General;

namespace Taki.Game.Cards
{
    enum UniqueCardEnum
    {
        Taki,
        ChangeColor,
        Stop,
        ChangeDirection,
        Plus,
        Plus2,
        SuperTaki,
        SwitchCardsWithDirection
    }
    internal class UniqueCard
        (UniqueCardEnum uniqueCardEnum, Color color) : 
        Card(UniqueTakiNames[uniqueCardEnum], color)
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

        public UniqueCard(UniqueCardEnum uniqueCardEnum) 
            : this(uniqueCardEnum, Color.Empty) { }

        public static bool IsTaki(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == UniqueTakiNames[UniqueCardEnum.Taki];
        }

        public static bool IsSuperTaki(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == UniqueTakiNames[UniqueCardEnum.SuperTaki];
        }

        public static bool IsChangeColor(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == UniqueTakiNames[UniqueCardEnum.ChangeColor];
        }

        public static bool IsStop(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == UniqueTakiNames[UniqueCardEnum.Stop];
        }

        public static bool IsChangeDirection(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == UniqueTakiNames[UniqueCardEnum.ChangeDirection];
        }

        public static bool IsPlus(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == UniqueTakiNames[UniqueCardEnum.Plus];
        }

        public static bool IsPlus2(Card card)
        {
            return IsUniqueCard(card) &&
                card.Name == UniqueTakiNames[UniqueCardEnum.Plus2];
        }

        public static bool IsSwitchCardsWithDirection(Card card)
        {
            return IsUniqueCard(card) &&
                card.Name == UniqueTakiNames[UniqueCardEnum.SwitchCardsWithDirection];
        }

        public static bool IsUniqueCard(Card card)
        {
            return IsCard(card) &&
                UniqueTakiNames.ContainsValue(card.Name);
        }
    }
}
