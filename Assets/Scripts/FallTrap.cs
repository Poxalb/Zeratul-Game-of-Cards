using System.Collections;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask playerLayer;
    public float movementAmount;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float delay = 1f;
    [SerializeField] private float resetDelay = 1f;

    private bool canAttack = true;
    private Vector3 initialPosition;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, transform.eulerAngles.z, -transform.up, castDistance, playerLayer);

        if (hit.collider != null && canAttack)
        {
            attackCoroutine = StartCoroutine(trapAttack());
        }
    }

    private IEnumerator trapAttack()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (-transform.up * movementAmount);

        canAttack = false;
        float elapsed = 0f;
        float duration = movementAmount / fallSpeed;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        yield return new WaitForSeconds(delay);

        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(endPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;

        yield return new WaitForSeconds(resetDelay);
        canAttack = true;
        attackCoroutine = null;
    }


    public void ResetTrap()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        transform.position = initialPosition;
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(
            transform.position - transform.up * castDistance,
            Quaternion.Euler(0, 0, transform.eulerAngles.z),
            Vector3.one
        );

        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
