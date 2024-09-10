using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSystem : MonoBehaviour
{
    public List<Card> cardSlots = new List<Card>(5);  
    private List<Card> originalCards = new List<Card>(5);
    public List<Image> cardSlotUI = new List<Image>(5); 
    public List<Image> inventorySlotUI = new List<Image>(5); 
    public List<Sprite> cardIcons = new List<Sprite>(5);

    private int selectedSlotIndex = -1; 
    private bool isInventoryActive;
    public GameObject inventoryPanel;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Card newCard = new Card("Card " + (i + 1), cardIcons[i]);
            cardSlots.Add(newCard);
            originalCards.Add(newCard); 
        }

        UpdateCardUI();
        UpdateInventoryUI(); 
    }

    public void UseFrontCard()
    {
        if (cardSlots.Count > 0)
        {
            cardSlots.RemoveAt(0);
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

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlotUI.Count; i++)
        {
            if (i < originalCards.Count)
            {
                inventorySlotUI[i].sprite = originalCards[i].cardIcon;
                inventorySlotUI[i].enabled = true;
            }
            else
            {
                inventorySlotUI[i].enabled = false;
            }
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        if (selectedSlotIndex == -1)
        {
            selectedSlotIndex = slotIndex;
            HighlightSlot(slotIndex, true);  
        }
        else
        {
            if (selectedSlotIndex != slotIndex)
            {
                SwapCards(selectedSlotIndex, slotIndex);
            }

            HighlightSlot(selectedSlotIndex, false);
            selectedSlotIndex = -1;
        }
    }

    private void SwapCards(int index1, int index2)
    {
        Card temp = originalCards[index1];
        originalCards[index1] = originalCards[index2];
        originalCards[index2] = temp;
        UpdateInventoryUI();
    }

    private void HighlightSlot(int slotIndex, bool highlight)
    {
        Color highlightColor = Color.yellow;  
        inventorySlotUI[slotIndex].color = highlight ? highlightColor : Color.white;
    }

    public void CloseInventory()
    {
        cardSlots.Clear();
        cardSlots.AddRange(originalCards);
        UpdateCardUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseFrontCard();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isInventoryActive = !isInventoryActive;
            inventoryPanel.SetActive(isInventoryActive);
        }

        if (isInventoryActive)
        {
            CloseInventory();
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
