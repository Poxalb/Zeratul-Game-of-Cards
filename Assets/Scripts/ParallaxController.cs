using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private float startPos,startPosY, length, lengthY;
    public GameObject cam;
    public float parallaxEffect;
    public float parallaxEffectY;
    void Start()
    {
        startPos = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;  
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;  
    }

    void Update()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1-parallaxEffect);
        float distanceY = cam.transform.position.y * parallaxEffectY;
        float movementY = cam.transform.position.y * (1 - parallaxEffectY);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, startPosY + distanceY, transform.position.z);


        if(movement > startPos + length)
        {
            startPos += length;
        }
        else if(movement < startPos - length)
        {
            startPos -= length;
        }
        if (movementY > startPosY + lengthY)
        {
            startPosY += lengthY;
        }
        else if (movementY < startPosY - lengthY)
        {
            startPosY -= lengthY;
        }
        
    }
}
