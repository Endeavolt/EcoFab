using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardControl : MonoBehaviour
{
    public enum CardState
    {
        Neutral,
        Suggestion,
        Choice
    }
    public CardState cardState;
    public float choice;
    public GameObject card;

    public CardManager cardManager;
    public CardIdentity cardIdentity;

    public float minSuggestion;
    public float maxSuggestion;

    public float timeReturn;
    private float timeReturnCountdown = 0.0f;


    public Image background;
    public Text text1;
    public Text text2;

    private RectTransform[] rectArray = new RectTransform[3];


    private Vector3 cardPosEndInput;
    private float tReturn;
    private SpriteRenderer spriteRenderer;
    private bool ChangeCard;


    private void Start()
    {
        rectArray[0] = background.rectTransform;
        rectArray[1] = text1.rectTransform;
        rectArray[2] = text2.rectTransform;

        InitComponent();
        SetupNewCard(0);

    }

    private void InitComponent()
    {
        cardManager = GetComponent<CardManager>();
        spriteRenderer = card.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            if (ChangeCard)
            {
                if (cardIdentity.cardType == CardType.Choice) ChoiceControl();
                else SetupNewCard(cardIdentity.choiceID[0]);
            }
            else
            {
                ResetCardState();
            }

        }
        else
        {
            ChangeCard = true;
            ResetCardState();
        }


    }


    private void ChoiceControl()
    {
        Vector3 mousePos = GetMousePositionCentered();
        Vector3 percentMousePos = GetMousePercentPosition(mousePos);
        UpdateCardState(percentMousePos);

        if (cardState == CardState.Suggestion) UpdateCardSuggestionState(percentMousePos);
        if (cardState == CardState.Neutral) UpdateCardNeutralState();
        if (cardState == CardState.Choice) UpdateCardChoiceState((int)choice);

        card.transform.position = new Vector3(0.7f * percentMousePos.x / 100.0f, 0f, 0f);
        cardPosEndInput = card.transform.position;
        timeReturnCountdown = 0.0f;
    }

    private Vector3 GetMousePositionCentered()
    {
        Vector3 pos = Input.mousePosition;
        pos.x = pos.x - Screen.width / 2;
        pos.y = pos.y - Screen.height / 2;

        return pos;
    }


    // Transform mouse position into percent per the center
    private Vector3 GetMousePercentPosition(Vector3 position)
    {
        Vector3 pos = position;
        pos.x = pos.x / (Screen.width / 2.0f) * 100.0f;
        pos.y = Mathf.Abs(pos.y / (Screen.height / 2.0f) * 100.0f);
        return pos;
    }

    private void UpdateCardState(Vector3 percentMousePos)
    {
        if (Mathf.Abs(percentMousePos.x) > minSuggestion && Mathf.Abs(percentMousePos.x) < maxSuggestion)
        {
            choice = Mathf.Sign(percentMousePos.x);
            cardState = CardState.Suggestion;
        }
        if (Mathf.Abs(percentMousePos.x) > maxSuggestion)
        {
            choice = Mathf.Sign(percentMousePos.x);
            cardState = CardState.Choice;

        }
        if (Mathf.Abs(percentMousePos.x) < minSuggestion)
        {
            choice = 0;
            cardState = CardState.Neutral;
        }

    }

    private void UpdateCardSuggestionState(Vector3 percentMousePos)
    {

        background.gameObject.SetActive(true);

        if (choice == 1.0f) text1.gameObject.SetActive(true);
        if (choice == -1.0f) text2.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            float sizeMouvement = (Mathf.Abs((Screen.width / 2.0f) - rectArray[i].rect.width / 2.0f));
            rectArray[i].localPosition = new Vector3(sizeMouvement * (percentMousePos.x / 100.0f), rectArray[i].localPosition.y, 0f);
        }
    }

    private void UpdateCardNeutralState()
    {
        ResetElementPosition();
        HideUI();
    }

    private void UpdateCardChoiceState(int index)
    {

        if (index == 1)
        {
            ResetElementPosition();
            SetupNewCard(cardIdentity.choiceID[0]);

        }
        if (index == -1)
        {
            ResetElementPosition();
            SetupNewCard(cardIdentity.choiceID[1]);
        }


    }

    private void SetupNewCard(int index)
    {
        cardIdentity = cardManager.cardIdentities[index];
        spriteRenderer.sprite = cardIdentity.cardSprite;
        ChangeCard = false;
    }

    private void ResetCardState()
    {
        ResetElementPosition();
        HideUI();

        if (timeReturnCountdown < timeReturn)
        {
            timeReturnCountdown += Time.deltaTime;
            tReturn = timeReturnCountdown / timeReturn;
            card.transform.position = Vector3.Lerp(cardPosEndInput, Vector3.zero, tReturn);
        }
    }

    // Reset Position 
    private void ResetElementPosition()
    {
        for (int i = 0; i < 3; i++)
        {
            rectArray[i].localPosition = new Vector3(0f, rectArray[i].localPosition.y, 0f);
        }
    }
    // Reset UI
    private void HideUI()
    {
        background.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);

    }


    #region Debug
    private void OnDrawGizmos()
    {

        Vector3 posSuggest1 = Vector3.zero;
        posSuggest1.x = (5.00f * 0.5625f) * minSuggestion / 100.0f;
        posSuggest1.y = -5.00f;
        posSuggest1.y = -5.00f;
        Vector3 posSuggest2 = posSuggest1;
        posSuggest2.y = 5.00f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(posSuggest1, posSuggest2);

        Gizmos.color = Color.red;
        posSuggest1.x = -(5.00f * 0.5625f) * minSuggestion / 100.0f;
        posSuggest2.x = -(5.00f * 0.5625f) * minSuggestion / 100.0f;
        Gizmos.DrawLine(posSuggest1, posSuggest2);

        Gizmos.color = Color.blue;
        posSuggest1.x = -(5.00f * 0.5625f) * maxSuggestion / 100.0f;
        posSuggest2.x = -(5.00f * 0.5625f) * maxSuggestion / 100.0f;
        Gizmos.DrawLine(posSuggest1, posSuggest2);

        Gizmos.color = Color.blue;
        posSuggest1.x = (5.00f * 0.5625f) * maxSuggestion / 100.0f;
        posSuggest2.x = (5.00f * 0.5625f) * maxSuggestion / 100.0f;
        Gizmos.DrawLine(posSuggest1, posSuggest2);

    }

    #endregion
}
