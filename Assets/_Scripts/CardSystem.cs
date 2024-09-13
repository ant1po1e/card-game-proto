using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> cardDataList = new List<CardData>(5);
    public List<Card> cardSlots = new List<Card>(5);
    private List<Card> originalCards = new List<Card>(5);
    public List<Image> cardSlotUI = new List<Image>(5);
    public List<Image> inventorySlotUI = new List<Image>(5);

    public CardData ultimateCardData; 
    private Card ultimateCard;
    public Image ultimateCardSlotUI;

    private int selectedSlotIndex = -1;
    private bool isInventoryActive;
    private Coroutine resetCoroutine;
    public GameObject inventoryPanel;

    void Start()
    {
        for (int i = 0; i < cardDataList.Count; i++)
        {
            Card newCard = new Card(cardDataList[i]);
            cardSlots.Add(newCard);
            originalCards.Add(newCard);
        }

        if (ultimateCardData != null)
        {
            ultimateCard = new Card(ultimateCardData);
            UpdateUltimateCardUI();
        }

        UpdateCardUI();
        UpdateInventoryUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseFrontCard();
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            UseUltimateCard();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isInventoryActive = !isInventoryActive;
            UpdateInventoryUI();
            inventoryPanel.SetActive(isInventoryActive);
        }

        if (isInventoryActive)
        {
            CloseInventory();
        }
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

            RestartIdleTimer();
        }
    }

    public void UseUltimateCard()
    {
        if (ultimateCard != null)
        {
            Debug.Log("Using ultimate card: " + ultimateCard.cardData.cardName);
        }
    }

    public void AddCard(CardData newCardData)
{
    Debug.Log("Attempting to add card: " + newCardData.cardName);

    if (cardSlots.Count < 5)
    {
        Card newCard = new Card(newCardData);
        cardSlots.Insert(0, newCard);
        cardDataList.Insert(0, newCardData); 
        originalCards.Insert(0, newCard);  
        UpdateCardUI();
        Debug.Log("Card added to slot. Total slots: " + cardSlots.Count);
    }
    else
    {
        if (ultimateCard == null)
        {
            ultimateCard = new Card(newCardData);
            ultimateCardData = newCardData;
            UpdateUltimateCardUI();
            Debug.Log("Ultimate card slot filled.");
        }
        else
        {
            Debug.Log("No available slots for new card.");
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
                cardSlotUI[i].sprite = cardSlots[i].cardData.cardIcon;
                cardSlotUI[i].enabled = true;
            }
            else
            {
                cardSlotUI[i].enabled = false;
            }
        }
    }

    private void UpdateUltimateCardUI()
    {
        if (ultimateCard != null)
        {
            ultimateCardSlotUI.sprite = ultimateCard.cardData.cardIcon;
            ultimateCardSlotUI.enabled = true;
        }
        else
        {
            ultimateCardSlotUI.enabled = false;
        }
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlotUI.Count; i++)
        {
            if (i < originalCards.Count)
            {
                inventorySlotUI[i].sprite = originalCards[i].cardData.cardIcon;
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

    private void RestartIdleTimer()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetAfterIdleTime());
    }

    private IEnumerator ResetAfterIdleTime()
    {
        yield return new WaitForSeconds(1f);
        ResetCards();
    }
}


public class Card
{
    public CardData cardData;

    public Card(CardData data)
    {
        cardData = data;
    }
}
