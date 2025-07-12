using System.Collections;
using UnityEngine;

public class FragileObject : MonoBehaviour
{
    [SerializeField] private float shakeIntensity = 0.1f; 
    [SerializeField] private float shakeDuration = 0.5f; 
    [SerializeField] private LayerMask playerLayer; 
    [SerializeField] private float respawnTime = 2f; 

    private Vector3 originalPosition;
    private Collider2D objectCollider;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        originalPosition = transform.position;
        objectCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;

        StartCoroutine(ShakeAndDestroy());
    }

    private IEnumerator ShakeAndDestroy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity),
                0f
            );
            transform.position = originalPosition + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        objectCollider.enabled = false;
        spriteRenderer.enabled = false;


        yield return new WaitForSeconds(respawnTime);


        transform.position = originalPosition;
        objectCollider.enabled = true;
        spriteRenderer.enabled = true;

    }
}
