using UnityEngine;

public class CapacityPod : Pod
{
    [SerializeField] private int capacity = 1;

    public override bool CanDeliver(PodDeliveryArea podDeliveryArea)
    {
        return true;
    }

    protected override void ExpirationEffect()
    {
        
    }

    protected override void DeliveryEffect(PodDeliveryArea podDeliveryArea)
    {
        Habitable habitable = podDeliveryArea.Habitable;
        habitable.AddCapacity(capacity);
    }
}
