﻿using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SuperTaki : TakiCard
    {
        public SuperTaki(IUserCommunicator userCommunicator) :
            base(Color.Empty, userCommunicator) { }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            _color = Color.Empty;

            while (!Colors.Contains(_color))
                _color = playersHolder.CurrentPlayer.ChooseColor();

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override bool IsStackableWith(Card other)
        {
            if (_color.Equals(DEFAULT_COLOR))
                return true;
            return base.IsStackableWith(other);
        }

        public override string ToString()
        {
            if(_color.Equals(DEFAULT_COLOR))
                return "SUPER-TAKI";
            return $"SUPER-TAKI, {_color}";
        }

        public override string[] GetStringArray()
        {
            return [
                "***********",
                "*  SUPER  *",
                "*         *",
                "*         *",
                "*         *",
                "*  TAKI   *",
                "***********"];
        }

        public override void ResetCard()
        {
            _color = DEFAULT_COLOR;
        }
    }
}
