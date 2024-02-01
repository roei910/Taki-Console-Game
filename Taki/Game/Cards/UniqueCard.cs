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
        Card(Enums.UniqueTakiNames[uniqueCardEnum], color)
    {
        public UniqueCard(UniqueCardEnum uniqueCardEnum) 
            : this(uniqueCardEnum, Color.Empty) { }

        public static bool IsTaki(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.Taki];
        }

        public static bool IsSuperTaki(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.SuperTaki];
        }

        public static bool IsChangeColor(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.ChangeColor];
        }

        public static bool IsStop(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.Stop];
        }

        public static bool IsChangeDirection(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.ChangeDirection];
        }

        public static bool IsPlus(Card card)
        {
            return IsUniqueCard(card) && 
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.Plus];
        }

        public static bool IsPlus2(Card card)
        {
            return IsUniqueCard(card) &&
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.Plus2];
        }

        public static bool IsSwitchCardsWithDirection(Card card)
        {
            return IsUniqueCard(card) &&
                card.Name == Enums.UniqueTakiNames[UniqueCardEnum.SwitchCardsWithDirection];
        }

        public static bool IsUniqueCard(Card card)
        {
            return IsCard(card) &&
                Enums.UniqueTakiNames.ContainsValue(card.Name);
        }
    }
}
