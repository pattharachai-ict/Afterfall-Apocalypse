using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip zombie;
    public AudioClip zombiegrunt;
    public AudioClip zombiedead;
    public AudioClip humnagrunt;
    public AudioClip humandead;
    public AudioClip melee;
    public AudioClip arrmorcollect;
    public AudioClip gunequip;
    public AudioClip healthpackcollect;
    public AudioClip jump;
    public AudioClip meleeequip;
    public AudioClip shoot;
<<<<<<< HEAD
    public AudioClip reload;
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    private void Start()
    {
       musicSource.clip = background;
       musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


}