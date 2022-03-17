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
    public Texture2D cardSprite;
    [TextArea]
    public string choiceText1;
    [TextArea]
    public string choiceText2;
}

public class CardManager : MonoBehaviour
{
    public CardIdentity[] cardIdentities;
}
