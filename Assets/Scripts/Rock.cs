using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("collided with player");
            AudioManager.instance.PlayLogCrashSound();
            GameManager.Instance.GameOver();
        }
    }
}
