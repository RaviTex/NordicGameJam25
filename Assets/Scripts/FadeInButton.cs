using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIFadeInButton : MonoBehaviour
{
    public float delay = 5f;
    public float fadeTime = 2f;

    private List<Graphic> graphics = new List<Graphic>();
    private float timer;
    private bool fading;

    void Start()
    {
        GetComponentsInChildren(true, graphics); // Grab all UI graphics, including TMP
        foreach (var g in graphics)
        {
            Color c = g.color;
            g.color = new Color(c.r, c.g, c.b, 0f); // Cloak all parts
        }
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

    IEnumerator FadeIn()
    {
        float t = 0f;
        List<Color> startColors = new List<Color>();
        List<Color> endColors = new List<Color>();

        foreach (var g in graphics)
        {
            Color start = g.color;
            Color end = new Color(start.r, start.g, start.b, 1f);
            startColors.Add(start);
            endColors.Add(end);
        }

        while (t < fadeTime)
        {
            for (int i = 0; i < graphics.Count; i++)
            {
                graphics[i].color = Color.Lerp(startColors[i], endColors[i], t / fadeTime);
            }
            t += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < graphics.Count; i++)
        {
            graphics[i].color = endColors[i]; // Full visibility
        }
    }
}
