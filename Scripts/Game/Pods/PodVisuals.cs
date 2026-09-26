using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Pod))]
public class PodVisuals : MonoBehaviour
{
    [SerializeField] private Image expirationHorizontalFilledImage;
    
    private Pod pod;

    #region Unity Callbacks

    private void Awake()
    {
        pod = GetComponent<Pod>();
    }

    private void OnEnable()
    {
        pod.onPodCheckTimeLeft += UpdatePodTimeLeftVisual;
    }

    private void OnDisable()
    {
        pod.onPodCheckTimeLeft -= UpdatePodTimeLeftVisual;
    }

    #endregion

    #region Setters

    private void UpdatePodTimeLeftVisual(float timeLeft, float expirationDuration)
    {
        expirationHorizontalFilledImage.fillAmount = (timeLeft / expirationDuration);
    }

    #endregion
}