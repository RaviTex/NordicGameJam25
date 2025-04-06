using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
