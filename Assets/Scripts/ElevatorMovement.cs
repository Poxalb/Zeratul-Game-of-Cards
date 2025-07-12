using UnityEngine;
using System.Collections;

public class ElevatorMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] private float movementAmount;
    
    private void Start()
    {
        StartCoroutine(elevatorMove());
    }

    IEnumerator elevatorMove()
    {
        float startPosition = transform.position.y;
        float endPosition = startPosition + movementAmount;
        while (true)
        {
            while (transform.position.y < endPosition)
            {
                transform.Translate(transform.up * speed * Time.deltaTime);  
                yield return null;  
            }                
              
            while (transform.position.y > startPosition)
            {
                transform.Translate(-transform.up * speed * Time.deltaTime);  
                yield return null;  
            }

        }
    }
}
