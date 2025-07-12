using UnityEngine;

public class UIPositionObject : MonoBehaviour
{

    public RectTransform objectToPosition;

    public int widthDivider = 2;
    public int heightDivider = 2;

    public float widthMultiplier = 1f;
    public float heightMultiplier = 1f;

    public bool updatePosition = false;
    void Start()
    {
        SetUIObjectPosition();
    }

    void Update()
    {
        if (updatePosition)
        {
            SetUIObjectPosition();
        }

    }
    
    public void SetUIObjectPosition()
    {
        if (objectToPosition != null && widthDivider != 0 && heightDivider != 0)
        {
            float xAnchor = widthMultiplier/ widthDivider;
            float yAnchor = heightMultiplier / heightDivider;
            objectToPosition.anchorMin = new Vector2(xAnchor, yAnchor);
            objectToPosition.anchorMax = new Vector2(xAnchor, yAnchor);
            objectToPosition.pivot = new Vector2(0.5f, 0.5f);

            objectToPosition.anchoredPosition = Vector2.zero;
        }
    }
}
