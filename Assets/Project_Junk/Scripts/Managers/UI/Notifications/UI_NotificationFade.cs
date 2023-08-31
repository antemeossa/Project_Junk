using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NotificationFade : MonoBehaviour
{
    public float fadeDuration = 1.0f;

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        
        canvasGroup = GetComponent<CanvasGroup>();
        
        StartCoroutine(FadeOutPanel());
    }
    void Start()
    {

        
    }

    private IEnumerator FadeOutPanel()
    {
        
        
        canvasGroup.alpha = 1.0f;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 1.5f;
            yield return null;
        }

        // Disable the panel and its children when the fade-out is complete
        
        gameObject.SetActive(false);
       
    }


}
