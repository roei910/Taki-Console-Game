﻿using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards
{
    public class ChangeColor : ColorCard
    {
        public ChangeColor(IUserCommunicator userCommunicator) :
            base(DEFAULT_COLOR, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* CHANGE *",
                "*        *",
                "*        *",
                "*        *",
                "* COLOR  *",
                "**********"];
        }

        public override bool IsStackableWith(Card other)
        {
            if (_color.Name == DEFAULT_COLOR.Name)
                return true;
            if (other is ColorCard colorCard)
                return _color.Equals(colorCard.GetColor());
            return false;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            while (!Colors.Contains(_color))
                _color = playersHolder.CurrentPlayer.ChooseColor();

            cardDecksHolder.UpdateTopDiscardInDB();
            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void ResetCard()
        {
            _color = DEFAULT_COLOR;
        }

        public override string ToString()
        {
            return $"ChangeColor {base.ToString()}";
        }
    }
}
