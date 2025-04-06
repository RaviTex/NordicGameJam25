using UnityEngine;
using TMPro;

public class TMPFadeIn : MonoBehaviour
{
    public float delay = 5f;
    public float fadeTime = 2f;

    private TextMeshProUGUI tmp;
    private float timer;
    private bool fading;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        if (tmp == null)
        {
            Debug.LogWarning("No TextMeshProUGUI on this gonk. Script’s useless here.");
            enabled = false;
            return;
        }

        Color c = tmp.color;
        tmp.color = new Color(c.r, c.g, c.b, 0f); // Cloaked
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delay && !fading)
        {
            fading = true;
            StartCoroutine(FadeIn());
        }
    }

    System.Collections.IEnumerator FadeIn()
    {
        float t = 0f;
        Color start = tmp.color;
        Color end = new Color(start.r, start.g, start.b, 1f);

        while (t < fadeTime)
        {
            tmp.color = Color.Lerp(start, end, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }

        tmp.color = end; // Locked in. Visible like a neon sign in Kabuki.
    }
}
