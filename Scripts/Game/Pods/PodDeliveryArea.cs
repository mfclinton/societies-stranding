using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PodDeliveryArea : MonoBehaviour
{
    [SerializeField] private float minDeliveryTime = 3f;

    public Habitable Habitable { get; private set; }
    private Dictionary<Pod, Coroutine> activeCoroutines = new Dictionary<Pod, Coroutine>();

    private void Awake()
    {
        Habitable = GetComponent<Habitable>();
        if(Habitable == null)
            Habitable = GetComponentInParent<Habitable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pod = other.GetComponent<Pod>();
        if (pod != null && !activeCoroutines.ContainsKey(pod) && pod.CanDeliver(this))
        {
            var coroutine = StartCoroutine(HandleDeliveryCoroutine(pod));
            activeCoroutines.Add(pod, coroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pod = other.GetComponent<Pod>();
        if (pod != null && activeCoroutines.ContainsKey(pod))
        {
            StopCoroutine(activeCoroutines[pod]);
            activeCoroutines.Remove(pod);
        }
    }

    private IEnumerator HandleDeliveryCoroutine(Pod pod)
    {
        yield return new WaitForSeconds(minDeliveryTime);
        pod.HandleDelivery(this);
        activeCoroutines.Remove(pod);
    }
}