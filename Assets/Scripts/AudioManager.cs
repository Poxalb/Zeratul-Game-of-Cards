using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource; 
    [SerializeField] AudioSource SFXSource;  
    public AudioClip backgroundMusic;
    public AudioClip deathSound;
    public AudioClip CheckpointSound;
    public AudioClip portalSound;
    public AudioClip chestOpenSound;
    public AudioClip itemPickupSound;
    public AudioClip dashSound;
    public AudioClip leverSound;
    public AudioClip openGateSound;

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
    public void playSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);        
    }
}
