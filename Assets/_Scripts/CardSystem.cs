using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSystem : MonoBehaviour
{
    public List<Card> cardSlots = new List<Card>(5);
    private List<Card> originalCards = new List<Card>(5);
    public List<Image> cardSlotUI = new List<Image>(5);
    public List<Sprite> cardIcons = new List<Sprite>(5);

    private int cardsUsed = 0;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Card newCard = new Card("Card " + (i + 1), cardIcons[i]);
            cardSlots.Add(newCard);
            originalCards.Add(newCard); 
        }

        UpdateCardUI();
    }

    public void UseFrontCard()
    {
        if (cardSlots.Count > 0)
        {
            cardSlots.RemoveAt(0);
            cardsUsed++;
            UpdateCardUI();

            if (cardSlots.Count == 0)
            {
                ResetCards();
            }
        }
    }

    private void ResetCards()
    {
        cardSlots.Clear();

        foreach (Card originalCard in originalCards)
        {
            cardSlots.Add(originalCard);
        }

        cardsUsed = 0;

        UpdateCardUI();
    }

    private void UpdateCardUI()
    {
        for (int i = 0; i < cardSlotUI.Count; i++)
        {
            if (i < cardSlots.Count)
            {
                cardSlotUI[i].sprite = cardSlots[i].cardIcon;
                cardSlotUI[i].enabled = true; 
            }
            else
            {
                cardSlotUI[i].enabled = false;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFrontCard();
        }
    }
}

public class Card
{
    public string cardName;
    public Sprite cardIcon;

    public Card(string name, Sprite icon)
    {
        cardName = name;
        cardIcon = icon;
    }
}
