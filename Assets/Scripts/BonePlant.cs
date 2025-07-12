using UnityEngine;
using System.Collections;

public class BonePlant : MonoBehaviour
{
    [SerializeField] private float speedAttack, speedHide;
    
    [SerializeField] private float movementAmount;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(bonePlantMove());
    }

    IEnumerator bonePlantMove()
    {
        float startPosition = transform.position.y;
        float endPosition = startPosition + movementAmount;
        while (true)
        {
            while (transform.position.y < endPosition)
            {
                transform.Translate(transform.up * speedAttack * Time.deltaTime);  
                yield return null;  
            }                
            animator.SetBool("bonePlantAttack", true);  

            yield return new WaitForSeconds(0.5f);

              
            while (transform.position.y > startPosition)
            {
                transform.Translate(-transform.up * speedHide * Time.deltaTime);  
                yield return null;  
            }
            animator.SetBool("bonePlantAttack", false);  

            yield return new WaitForSeconds(2f);
        }
    }
}
