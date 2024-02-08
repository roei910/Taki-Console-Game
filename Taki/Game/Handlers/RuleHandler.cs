using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Communicators;
using Taki.Game.Deck;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    //TODO: naming conventions and logics
    //TODO: check what happends when finishing hand with plus card
    internal class RuleHandler
    {
        protected readonly PlayersHandler _playersHandler;
        protected readonly CardsHandler _cardsHandler;
        protected int _numberOfPlayerCards;
        protected bool isDirectionNormal = true;

        public RuleHandler(
            GameHandlers takiGameHandlers,
            int numberOfPlayerCards)
        {
            _playersHandler = takiGameHandlers.GetPlayersHandler();
            _cardsHandler = takiGameHandlers.GetCardsHandler();
            _numberOfPlayerCards = numberOfPlayerCards;
        }

        public PlayersHandler GetPlayersHandler() 
        { 
            return _playersHandler;
        }

        public CardsHandler GetCardsHandler()
        {
            return _cardsHandler;
        }

        

        public void PlayTurn()
        {
            //Card topDiscard = GetTopDiscard(moveHandler.ChangeColor);
            //Player first = playerHandler.CurrentPlayer;
            //bool isPlayerPickCard = moveHandler.IsPlus2() ?
            //    first.AskPlayerToPickCardPlus2(topDiscard, out Card playerCard) :
            //    first.AskPlayerToPickCard(topDiscard, out playerCard);

            //if (!isPlayerPickCard)
            //    HandlePlayerFinishTurn(first, topDiscard);
            //else if (!TryHandleCard(topDiscard, playerCard))
            //{
            //    playerHandler.ReturnUnhandledCard(playerCard);
            //    PlayTurn();
            //}


            //Card topDiscard = _cardsHandler.GetTopDiscard();
            //if (topDiscard.CanStack())
            //    _playersHandler.CurrentPlayerPlay(topDiscard);
        }

        private void HandlePlayerFinishTurn(Player first, Card topDiscard)
        {
            //if (first.IsHandEmpty())
            //    return;
            //if (_moveHandler.IsTaki())
            //    _moveHandler.CloseTaki(topDiscard, first, HandleUniqueCard);
            //if (_moveHandler.IsPlus2())
            //    _moveHandler.FinishPlus2(cardDeck, _playersHandler.DrawCards);
            //_moveHandler.CheckForTie();
        }

        private bool TryHandleCard(Card topDiscard, Card card)
        {
            //if (!moveHandler.IsValidMove(topDiscard, card))
            //    return false;
            //cardDeck.AddCardToDiscardPile(card);
            //if (!moveHandler.IsTaki() && UniqueCard.IsUniqueCard(card))
            //    HandleUniqueCard(card);
            //Communicator.PrintMessage(
            //    $"Player[{playerHandler.CurrentPlayer.Id}] played {card}",
            //    Communicator.MessageType.Alert);
            //moveHandler.ResetPlayCounter();
            return true;
        }

        private void HandleUniqueCard(Card card)
        {
            ////TODO: no ifs
            //if (UniqueCard.IsPlus(card))
            //    PlayTurn();
            //else if (UniqueCard.IsPlus2(card))
            //    moveHandler.IncrementPlus2();
            //else if (UniqueCard.IsTaki(card))
            //{
            //    Communicator.PrintMessage("TAKI open");
            //    moveHandler.UpdateTakiCard(card);
            //}
            //else if (UniqueCard.IsSuperTaki(card))
            //{
            //    HandleUniqueCard(new UniqueCard(UniqueCardEnum.ChangeColor));
            //    HandleUniqueCard(new UniqueCard(UniqueCardEnum.Taki));
            //}
            //else if (UniqueCard.IsStop(card))
            //    playerHandler.NextPlayer(isDirectionNormal);
            //else if (UniqueCard.IsChangeColor(card))
            //    moveHandler.UpdateChangeColor(playerHandler.GetColorFromPlayer());
            //else if (UniqueCard.IsChangeDirection(card))
            //    isDirectionNormal = !isDirectionNormal;
            //else if (UniqueCard.IsSwitchCardsWithDirection(card))
            //    playerHandler.SwitchCardsWithDirectionCard(isDirectionNormal);
            //else
            //    throw new NotImplementedException("card functionality not implemented yet");
        }

        private Card? GetTopDiscard(Color changeColor)
        {
            ////TODO: split deck and discard pile (they can be two instances of the same class
            //Card topDiscard = cardDeck.GetTopDiscardPile();
            //while (MoveHandler.IsValidDiscard(topDiscard, changeColor))
            //    topDiscard = cardDeck.GetNextDiscard(topDiscard);
            //if (changeColor != Color.Empty)
            //    topDiscard = new NumberCard("", changeColor);
            //return topDiscard;
            return null;
        }

        protected virtual bool PlayerFinishedHand()
        {
            return _playersHandler.PlayerFinishedHand();
        }

        
    }
}