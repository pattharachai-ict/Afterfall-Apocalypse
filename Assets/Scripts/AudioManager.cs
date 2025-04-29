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