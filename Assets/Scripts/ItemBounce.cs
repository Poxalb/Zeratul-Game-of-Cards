using UnityEngine;

public class ItemBounce : MonoBehaviour
{
    [SerializeField] private float bounceHeight = 1f;
    [SerializeField] private float bounceDuration = 0.3f;
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatFrequency = 1f;

    private Vector3 initialPosition;
    private float floatTimer;
    private bool hasBounced = false;

    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(BounceUpward());
    }

    void Update()
    {
        if (hasBounced)
        {
            floatTimer += Time.deltaTime;
            float floatOffset = Mathf.Sin(floatTimer * floatFrequency * 2 * Mathf.PI) * floatAmplitude;
            transform.position = initialPosition + Vector3.up * floatOffset;
        }
    }

    System.Collections.IEnumerator BounceUpward()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * bounceHeight;
        float elapsed = 0;

        while (elapsed < bounceDuration)
        {
            float t = elapsed / bounceDuration;
            float curvedT = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(start, end, curvedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        initialPosition = transform.position;
        hasBounced = true;
    }
}
