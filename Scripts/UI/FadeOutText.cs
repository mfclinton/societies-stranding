using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeOutText : MonoBehaviour
{
    public float fadeDuration = 9.0f;
    private TMP_Text textComponent;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        StartCoroutine(FadeTextOut());
    }

    IEnumerator FadeTextOut()
    {
        Color originalColor = textComponent.color;
        float elapsedTime = 0f;
        //Set the original color with an alpha of 0.9
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        gameObject.SetActive(false);
    }
}
