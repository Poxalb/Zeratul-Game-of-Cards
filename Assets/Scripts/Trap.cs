using UnityEngine;

public class Trap : MonoBehaviour
{

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioManager.playSFX(audioManager.deathSound);
            collision.GetComponent<PlayerRespawn>().Respawn();
        }
    }
}
