using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillDeck : MonoBehaviour
{
    [field: SerializeField] public int[] deck;
    
    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            deck[i] = i;
        }

        for(int i = deck.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    //Once we've pulled cards from the deck, they are placed in an in-use array
    //We don't touch cards in this array whatsoever
    //If a player has played a card that's in the in-use array, that card goes to a discard array
    //When the deck is empty, take the discard array and append it to the deck
}