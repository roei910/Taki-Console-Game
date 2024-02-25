﻿using Taki.Data;
using Taki.Dto;
using Taki.Interfaces;
using Taki.Models.Algorithm;
using Taki.Models.Deck;
using Taki.Models.Players;

namespace Taki.Models.GameLogic
{
    internal class GameRestore
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;
        private readonly IDal<PlayerDto> _playersDatabase;
        private readonly CardDeckDatabase _cardDeckDatabase;

        public GameRestore(IUserCommunicator userCommunicator, List<IPlayerAlgorithm> playerAlgorithms,
            ManualPlayerAlgorithm manualPlayerAlgorithm, IDal<PlayerDto> playersDatabase, CardDeckDatabase cardDeckDatabase)
        {
            _userCommunicator = userCommunicator;
            _playerAlgorithms = playerAlgorithms;
            _playersDatabase = playersDatabase;
            _cardDeckDatabase = cardDeckDatabase;
            _playerAlgorithms.Add(manualPlayerAlgorithm);
        }

        public bool TryRestoreTakiGame(ICardDecksHolder cardDecksHolder, out IPlayersHolder? _playersHolder)
        {
            if(_playersDatabase.IsEmpty())
            {
                _playersHolder = null;
                return false;
            }

            _playersHolder = GeneratePlayersHolder(cardDecksHolder);
            UpdateCardDeckFromDatabase(cardDecksHolder);

            _userCommunicator.SendMessageToUser("users restored are:");
            var playersInormation = _playersHolder.Players.Select((p, i) => $"{i + 1}. {p.GetInformation()}")
                .ToList();
            _userCommunicator.SendMessageToUser(string.Join("\n", playersInormation));
            _userCommunicator.SendMessageToUser();

            return true;
        }

        public void DeleteAll()
        {
            _playersDatabase.DeleteAll();
            _cardDeckDatabase.DeleteAll();
        }

        private List<Player> GeneratePlayersFromDTO(List<PlayerDto> playerDTOs)
        {
            var players = playerDTOs.Select(player =>
            {
                IPlayerAlgorithm playerAlgorithm = _playerAlgorithms.Where(algo => algo.ToString() == player.ChoosingAlgorithm).First();

                return new Player(player.Name, playerAlgorithm, _userCommunicator)
                {
                    Score = player.Score
                };
            }).ToList();

            return players;
        }

        private IPlayersHolder GeneratePlayersHolder(ICardDecksHolder cardDecksHolder)
        {
            var playersDto = _playersDatabase.FindAll();
            var players = GeneratePlayersFromDTO(playersDto);
            var playersHolder = new PlayersHolder(players, 0, _userCommunicator, _playersDatabase);

            playersDto.ForEach(player =>
            {
                var cards = player.PlayerCards.Select(card =>
                    cardDecksHolder.RemoveCardByDTO(card)).ToList();

                playersHolder!.Players.Where(p => p.Name == player.Name).First()
                    .PlayerCards = cards;
            });

            //TODO: work on reading pyramid players
            //if(players is List<PyramidPlayerDto>)
            //...

            return playersHolder;
        }

        private void UpdateCardDeckFromDatabase(ICardDecksHolder cardDecksHolder)
        {
            List<CardDto> drawPileDto = _cardDeckDatabase.GetDrawCards();
            List<CardDto> discardPileDto = _cardDeckDatabase.GetDiscardCards();

            CardDeck drawCardDeck = cardDecksHolder.GetDrawCardDeck();
            CardDeck discardCardDeck = cardDecksHolder.GetDiscardCardDeck();

            var newDrawPile = drawPileDto.Select(drawCardDeck.RemoveFirstDTO).ToList();
            drawCardDeck.AddMany(newDrawPile);
            var newDiscardPile = discardPileDto.Select(drawCardDeck.RemoveFirstDTO).ToList();
            discardCardDeck.AddMany(newDiscardPile);

            cardDecksHolder.GetDiscardCardDeck().GetAllCards().Select((card, index) =>
            {
                card.UpdateFromDto(discardPileDto[index], cardDecksHolder);
                return card;
            }).ToList();
        }
    }
}
