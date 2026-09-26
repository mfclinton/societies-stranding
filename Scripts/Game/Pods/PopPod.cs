using UnityEngine;

public class PopPod : Pod
{
    [SerializeField] private int score = 1;
    [SerializeField] private float failReputation = -10f;
    [SerializeField] private float successReputation = 10f;

    public override bool CanDeliver(PodDeliveryArea podDeliveryArea)
    {
        return podDeliveryArea.Habitable.CurrentStage != Habitable.Stage.FinalStage;
    }

    protected override void ExpirationEffect()
    {
        GameManager.Instance.AddRep(failReputation);
    }

    protected override void DeliveryEffect(PodDeliveryArea podDeliveryArea)
    {
        Habitable habitable = podDeliveryArea.Habitable;
        habitable.CreateNewPopulation();
        
        GameManager.Instance.AddScore(score);
        GameManager.Instance.AddRep(successReputation);
    }
}
