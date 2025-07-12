using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float castDistance;
    public Vector2 boxSize;
    private Animator animator;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        animator.SetBool("Bounce", Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, playerLayer));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}

