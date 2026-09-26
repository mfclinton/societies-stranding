

using UnityEngine;

[RequireComponent(typeof(Tetherable))]
public class TetherableInteractionArea : InteractionAreaBase
{
    private Tetherable tetherable;
    
    public override ActionNames ActionName => ActionNames.PrimaryInteract;
    public override bool IsInteractable => !tetherable.isTethered;
    
    protected override void Awake()
    {
        base.Awake();
        tetherable = GetComponent<Tetherable>();
    }
    
    public override void ExecuteInteraction(InteractionSystem interactor)
    {
        TetherSystem tetherSystem = interactor.GetComponent<TetherSystem>();
        if (tetherSystem == null)
            return;
        
        bool success = tetherSystem.TryAttachTetherable(tetherable);
        interactor.OnInteractionAreaUpdated(this);
    }
}