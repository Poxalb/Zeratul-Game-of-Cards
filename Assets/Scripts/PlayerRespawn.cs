using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerRespawn : MonoBehaviour
{
    public Transform currentCheckpoint;
    private PlayerMovement playerMovement;
    AudioManager audioManager;
    public int deathCounter;
    private bool isRespawning = false;
    public TMP_Text deathCounterText;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("DeathCounter"))
        {
            deathCounter = PlayerPrefs.GetInt("DeathCounter");
        }
        else
        {
            PlayerPrefs.SetInt("DeathCounter", 0);
            deathCounter = 0;
        }
        deathCounterText.text = deathCounter.ToString();
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        isRespawning = true;
        TrapReset();
        playerMovement.canMove = false;
        playerMovement.stopMovement();
        transform.position = currentCheckpoint.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        deathCounter++;
        PlayerPrefs.SetInt("DeathCounter", deathCounter);
        deathCounterText.text = deathCounter.ToString();
        yield return new WaitForSeconds(1f);
        playerMovement.canMove = true;
        isRespawning = false;
    }

    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CheckPoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Appear");
            audioManager.playSFX(audioManager.CheckpointSound);
        }
            
    }

    private void TrapReset(){
        GameObject[] traps = GameObject.FindGameObjectsWithTag("FallTrap");
        foreach (GameObject trap in traps)
        {
            if(trap.GetComponent<FallTrap>()!=null)
            {
                trap.GetComponent<FallTrap>().ResetTrap();
            }
            else if(trap.GetComponent<MovingPlatform>()!=null)
            {
                trap.GetComponent<MovingPlatform>().ResetTrap();
            }
        }
    }
}
