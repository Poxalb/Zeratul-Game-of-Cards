using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTranform;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    private GridManager gridManager;

    public bool hasBeenPlayed = false;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private bool needUpdateCardPlayPosition = false;
    [SerializeField] private int playPositionYDivider = 2;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 4;
    [SerializeField] private float playPositionXMultiplier = 1f;
    [SerializeField] private bool needUpdatePlayPosition = false;

    private LayerMask gridLayerMask;
    private LayerMask characterLayerMask;

    private Card cardData => cardDisplay?.cardData;

    private CardDisplay cardDisplay;

    HandManager handManager;

    DiscardManager discardManager;

    private bool canDrag = true;
    private static readonly Color defaultGlowColor = new Color32(0x00, 0x8F, 0xD6, 0xFF); // #008FD6

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTranform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        updateCardPlayPostion();
        updatePlayPostion();
        gridManager = FindFirstObjectByType<GridManager>();
        handManager = FindFirstObjectByType<HandManager>();
        discardManager = FindFirstObjectByType<DiscardManager>();


        gridLayerMask = LayerMask.GetMask("Grid");
        cardDisplay = GetComponent<CardDisplay>();
        //debug.Log($"[CardMovement:Awake] cardData = {cardDisplay?.cardData?.cardName}");
        
    }

    void Update()
    {
        if (hasBeenPlayed || currentState == 4)
        {
            return;
        }

        if (needUpdateCardPlayPosition)
        {
            updateCardPlayPostion();
        }

        if (needUpdatePlayPosition)
        {
            updatePlayPostion();
        }

        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0)) //Check if mouse button is released
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                break;
            case 4:
                // Final state, do nothing
                HandleFinalState();
                break;
        }
        
        //debug.Log($"{gameObject.GetInstanceID()} | State: {currentState} | Played: {hasBeenPlayed} | Active: {gameObject.activeSelf} | Playing Card: {GameManager.Instance.playingCard}");
    }


    public void InitializeCardState(int state, bool played)
    {
        currentState = state;
        hasBeenPlayed = played;
        GameManager.Instance.playingCard = false; // Reset the playing card flag

    }

    private void HandleFinalState()
    {
        // In the final state, we do not need to do anything.
        // This is just a placeholder for clarity.

        currentState = 4;
        this.enabled = false; // Deactivate this script
    }

    private void TransitionToState0()
    {
       // if (hasBeenPlayed || currentState == 4) return;

        currentState = 0;
        GameManager.Instance.playingCard = false; // Reset the playing card flag
        rectTransform.localScale = originalScale; //Reset Scale
        rectTransform.localRotation = originalRotation; //Reset Rotation
        rectTransform.localPosition = originalPosition; //Reset Position
        glowEffect.SetActive(false); //Disable glow effect
        playArrow.SetActive(false); //Disable playArrow
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasBeenPlayed || currentState == 4) return;

        // Check mana
        if (GameManager.Instance.PlayerMana < cardDisplay.cardData.cost)
        {
            canDrag = false;
            if (glowEffect != null)
            {
                glowEffect.SetActive(true);
                var image = glowEffect.GetComponent<Image>();
                if (image != null) image.color = Color.red;
            }
        }
        else
        {
            canDrag = true;
            if (glowEffect != null)
            {
                glowEffect.SetActive(true);
                var image = glowEffect.GetComponent<Image>();
                if (image != null) image.color = defaultGlowColor;
            }
        }

        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hasBeenPlayed || currentState == 4) return;

        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag) return;

        if (hasBeenPlayed || currentState == 4) return;

        if (currentState == 1)
        {
            if (Input.mousePosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);
            }
            else
            {
                currentState = 2;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        if (hasBeenPlayed || currentState == 4)
        {
            //debug.Log("Card has already been played or is in the final state.");
            return;
        }
        if (currentState == 2)
        {
            if (Input.mousePosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);
                rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
        rectTransform.localPosition = originalPosition;
    }

    private void HandleDragState()
    {
        //Set the card's rotation to zero
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);
    }
    private void HandlePlayState()
    {
        //debug.Log("Handle Play --- Card "+cardData.cardName+" Subtype: " + cardData.GetType().Name);
        EnsurePlayingCardFlag();
        MoveCardToPlayPosition();

        if (!Input.GetMouseButton(0)) // Check if mouse button is released
        {
            TryPlayCard();
        }

        CheckForDragBack();
    }

    private void EnsurePlayingCardFlag()
    {
        if (!GameManager.Instance.playingCard)
        {
            GameManager.Instance.playingCard = true;
        }
    }

private void MoveCardToPlayPosition()
{
    rectTransform.localPosition = playPosition;
    rectTransform.localRotation = Quaternion.identity;
}

    private void TryPlayCard()
    {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
    //debug.Log("TryPlayCard --- Raycast hit: " + hit.collider != null);
    if (hit.collider != null && hit.collider.GetComponent<GridCell>())
    {
            //debug.Log("TryPlayCard --- cardData type: " + cardData?.GetType().Name);

            if (cardData is Character character)
            {
                //debug.Log("Trying to play card...");
                AttemptCharacterPlacement(hit.collider.GetComponent<GridCell>());
            }
            else if (cardData is Spell spell)
            {
                AttemptSpellPlacement(hit.collider.GetComponent<GridCell>());
            }
        
    }
    else
    {
        GameManager.Instance.playingCard = false; // Reset the playing card flag
        TransitionToState0();
    }
}

    private void AttemptCharacterPlacement(GridCell cell)
    {
        Vector2 gridIndex = cell.gridIndex;
        //debug.Log($"Attempting to play character card to grid index: {gridIndex}");

        Card cardInstance = GetComponent<CardDisplay>().cardData.Clone(); // Clone the card data
        if (gridIndex.y == 0 && gridManager.AddOccupantToGrid(cardInstance, gridIndex))
        {
            hasBeenPlayed = true;
            
            // Deduct mana cost from player
            GameManager.Instance.PlayerMana -= cardInstance.cost;

            GameManager.Instance.UpdateUI(); // Update UI after playing card
            
            RemoveCardFromHand();
            //debug.Log($"Card {GetComponent<CardDisplay>().cardData.cardName} played successfully to grid index: " + gridIndex);
            currentState = 4;
            GameManager.Instance.playingCard = false; // Reset the playing card flag

            Destroy(gameObject); // Destroy the card GameObject
        }
        else
        {
            GameManager.Instance.playingCard = false; // Reset the playing card flag

            //debug.Log("Failed to play card to grid index: " + gridIndex);
            TransitionToState0();
        }
    }
    private void AttemptSpellPlacement(GridCell cell)
    {
        //debug.Log("Attempting to play spell card...");
        Vector2 gridIndex = cell.gridIndex;
        if (gridManager.IsCellOccupied(gridIndex))
        {
            hasBeenPlayed = true;
            
            // Deduct mana cost from player
            GameManager.Instance.PlayerMana -= GetComponent<CardDisplay>().cardData.cost;
            GameManager.Instance.UpdateUI(); // Update UI after playing card

            SpellEffectApplier.ApplySpellEffect(GetComponent<CardDisplay>().cardData as Spell, gridManager.GetCardAtGridIndex(gridIndex));
            cardDisplay.UpdateCardDisplay(); // Update the card display after applying the spell effect
            RemoveCardFromHand();
            //debug.Log($"Card {GetComponent<CardDisplay>().cardData.cardName} played successfully to grid index: " + gridIndex);
            currentState = 4;
            GameManager.Instance.playingCard = false; // Reset the playing card flag

            Destroy(gameObject); // Destroy the card GameObject
        }
        else
        {
            GameManager.Instance.playingCard = false; // Reset the playing card flag

            //debug.Log("Failed to play spell card to grid index: " + gridIndex);
            TransitionToState0();
        }
    }

private void RemoveCardFromHand()
    {
        if (discardManager != null)
        {
            discardManager.AddCardToDiscard(GetComponent<CardDisplay>().cardData);
        }
        if (handManager != null)
        {
            handManager.cardsInHand.Remove(gameObject);
            handManager.OnCardRemoved();
        }
    }

private void CheckForDragBack()
{
    if (Input.mousePosition.y < cardPlay.y)
    {
        currentState = 2;
        playArrow.SetActive(false);
    }
}

    private void updateCardPlayPostion()
    {
        if (cardPlayDivider != 0 && canvasRectTranform != null)
        {
            float segment = cardPlayMultiplier / cardPlayDivider;
            cardPlay.y = canvasRectTranform.rect.height * segment;
        }
    }

    private void updatePlayPostion()
    {
        if (canvasRectTranform != null && playPositionYDivider != 0 && playPositionXDivider != 0)
        {
            // Increase X multiplier for more rightward position
            float segmentX = (playPositionXMultiplier * 1.5f) / playPositionXDivider; // Try 3x or more
            float segmentY = playPositionYMultiplier / playPositionYDivider;

            playPosition.x = canvasRectTranform.rect.width * segmentX;
            playPosition.y = canvasRectTranform.rect.height * segmentY;
            playPosition.z = -10f; // Ensure it's in front of the hand
        }
    }
}