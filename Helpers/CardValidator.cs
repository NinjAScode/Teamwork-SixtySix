﻿namespace Santase.AI.NinjaPlayer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Logic.Players;
    using Logic.Cards;
    using Logic.PlayerActionValidate;
    using Logic;

    public class CardValidator
    {
        private readonly IAnnounceValidator announceValidator;

        public CardValidator()
            : this(new AnnounceValidator())
        {
        }

        public CardValidator(IAnnounceValidator announceValidator)
        {
            this.announceValidator = announceValidator;
        }

        public bool HasNonTrumpCardType(PlayerTurnContext context, ICollection<Card> cards, CardType type)
        {
            return cards.Any(x => x.Suit != context.TrumpCard.Suit && x.Type == type);
        }

        public bool HasTrumpCard(PlayerTurnContext context, ICollection<Card> cards)
        {
            return cards.Any(x => x.Suit == context.TrumpCard.Suit);
        }

        public bool HasTrumpCardType(PlayerTurnContext context, ICollection<Card> cards, CardType type)
        {
            return cards.Any(x => x.Suit == context.TrumpCard.Suit && x.Type == type);
        }

        public bool HasHigherCard(Card firstPlayedCard, ICollection<Card> cards)
        {
            return cards.Any(c => c.Suit == firstPlayedCard.Suit
                                           && c.GetValue() > firstPlayedCard.GetValue());
        }

        public bool HasAnyCardInSuit(ICollection<Card> cards, CardSuit suit)
        {
            return cards.Any(c => c.Suit == suit);
        }

        public bool IsTenOrAce(Card card)
        {
            return card.GetValue() >= 10;
        }

        public bool IsTrump(Card card, CardSuit trumpCardSuit)
        {
            return card.Suit == trumpCardSuit;
        }

        public bool HasAnnounce(PlayerTurnContext context, Announce announce, ICollection<Card> cards)
        {
            foreach (var card in cards)
            {
                if (this.IsCardInAnnounce(context, card, cards, announce))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCardInAnnounce(PlayerTurnContext context, Card card, ICollection<Card> cards, Announce announce)
        {
            return card.Type == CardType.Queen
                    && this.announceValidator.GetPossibleAnnounce(cards, card, context.TrumpCard) == announce;
        }
    }
}
