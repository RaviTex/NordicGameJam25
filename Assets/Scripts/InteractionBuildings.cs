using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class InteractionBuildings : MonoBehaviour
{
    public void StartDisable()
    {
        StartCoroutine(DisableCoroutine());
    }
    
    private IEnumerator DisableCoroutine()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        
        Vector3 orginalScale = transform.localScale;
        Vector3 targetScale = orginalScale * 1.5f;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(orginalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        elapsedTime = 0f;
        duration = 1f;
        Vector3 smallScale = orginalScale * 0.1f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, smallScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
