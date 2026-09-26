using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class Pod : MonoBehaviour
{
    public delegate void OnPodCheckTimeLeft(float timeLeft, float expirationDuration);
    public event OnPodCheckTimeLeft onPodCheckTimeLeft;
    public delegate void OnPodExpired();
    public event OnPodExpired onPodExpired;
    public delegate void OnPodDelivered(PodDeliveryArea podDeliveryArea);
    public event OnPodDelivered onPodDelivered;

    [SerializeField] private float expirationCheckInterval = 0.2f;
    [SerializeField] private float expirationDuration = 30f;
    private float creationTime;

    Coroutine expirationCoroutine;

    #region Unity Callbacks

    private void Awake()
    {
        creationTime = Time.time;
    }

    private void OnEnable()
    {
        expirationCoroutine = StartCoroutine(CheckPodExpired());
    }
    
    private void OnDisable()
    {
        if(expirationCoroutine != null)
            StopCoroutine(expirationCoroutine);
    }

    #endregion
    
    private IEnumerator CheckPodExpired()
    {
        while (true)
        {
            float timeLeft = expirationDuration - (Time.time - creationTime);
            onPodCheckTimeLeft?.Invoke(timeLeft, expirationDuration);
            if (timeLeft <= 0f)
                HandleExpiration();
            
            yield return new WaitForSeconds(expirationCheckInterval);
        }
    }

    public abstract bool CanDeliver(PodDeliveryArea podDeliveryArea);
    protected abstract void ExpirationEffect();

    protected abstract void DeliveryEffect(PodDeliveryArea podDeliveryArea);

    protected void HandleExpiration()
    {
        ExpirationEffect();
        onPodExpired?.Invoke();
        Destroy(gameObject);
    }

    // Protected method to allow derived classes to invoke onPodDelivered event
    public void HandleDelivery(PodDeliveryArea podDeliveryArea)
    {
        if (!CanDeliver(podDeliveryArea))
            return;
        
        DeliveryEffect(podDeliveryArea);
        onPodDelivered?.Invoke(podDeliveryArea);
        Destroy(gameObject);
    }
}
