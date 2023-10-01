using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _musicPlayer;
    [SerializeField] AudioSource _sfxPlayer;

    [SerializeField] public AudioClip LaserClip;
    [SerializeField] public AudioClip HitClip;
    [SerializeField] public AudioClip ExplosionClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _musicPlayer.mute = !_musicPlayer.mute;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _sfxPlayer.mute = !_sfxPlayer.mute;
        }
    }

    public void PlaySfx(AudioClip audioClip, Vector3 position, bool randomPitch = false)
    {
        _sfxPlayer.transform.position = position;
        
        float originalPitch = _sfxPlayer.pitch;
        if (randomPitch)
        {
            _sfxPlayer.pitch = originalPitch + Random.Range(-.2f, .2f);
        }

        _sfxPlayer.PlayOneShot(audioClip);
        _sfxPlayer.pitch = originalPitch;
    }
}