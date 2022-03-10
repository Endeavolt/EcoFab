using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Story,
    Choice,
};

[System.Serializable]
public struct CardIdentity
{
    public CardType cardType;
    public int[] choiceID;
    public Sprite cardSprite;
}

public class CardManager : MonoBehaviour
{
    public CardIdentity[] cardIdentities;
}
