using UnityEngine;
using System.Collections;

public class ItemDespawn : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TemporaryDespawn());
        }
    }

    IEnumerator TemporaryDespawn()
    {
        spriteRenderer.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(2f);

        spriteRenderer.enabled = true;
        col.enabled = true;
    }
}
