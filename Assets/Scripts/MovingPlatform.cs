    using UnityEngine;
    using System.Collections;

    public class MovingPlatform : MonoBehaviour
    {
        public Vector2 boxSize;
        public float castDistance;
        public LayerMask playerLayer;
        public float movementAmount;
        [SerializeField] private float moveSpeed;
        public Vector3 initialPosition;
        private Coroutine moveCoroutine;
        public Vector3 direction;
        [SerializeField] private float delay = 1f;
        [SerializeField] private float ResetDelay = 4f;




        private void Awake()
        {
            initialPosition = transform.position;
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, transform.eulerAngles.z, -transform.up, castDistance, playerLayer);

            if (hit.collider != null && moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(platformMove());
            }
        }


        private IEnumerator platformMove()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * movementAmount;

        float elapsed = 0f;
        float duration = movementAmount / moveSpeed;

        
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

        yield return new WaitForSeconds(ResetDelay);

        moveCoroutine = null;
    }


        public void ResetTrap()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            transform.position = initialPosition; 
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 origin = transform.position;
            Vector3 destination = origin + -transform.up * castDistance;

            Gizmos.DrawWireCube(destination, boxSize);
        }





    }
