using UnityEngine;

public class CardPickup : MonoBehaviour
{
    public CardData cardData; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CardSystem cardSystem = other.GetComponent<CardSystem>();
            if (cardSystem != null)
            {
                cardSystem.AddCard(cardData);
                gameObject.SetActive(false);
            }
        }
    }
}
