using UnityEngine;

public class MyShittyAudioManagerKasperMineeeeeeeee : MonoBehaviour
{
   


    public AudioClip sound;

    void Start()
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }


}
