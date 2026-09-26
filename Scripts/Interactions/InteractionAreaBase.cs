using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionAreaBase : MonoBehaviour
{

    #region State Variables

    public LinkedList<InteractionSystem> InteractorsInArea { get; private set; }
    public virtual bool IsInteractable => true;
    public virtual ActionNames ActionName => ActionNames.PrimaryInteract;
    
    #endregion

    #region Unity Callbacks

    protected virtual void Awake()
    {
        InteractorsInArea = new LinkedList<InteractionSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractionSystem interactor = collision.gameObject.GetComponent<InteractionSystem>();
        if (interactor != null)
            OnInteractorEnterArea(interactor);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractionSystem interactor = collision.gameObject.GetComponent<InteractionSystem>();
        if (interactor != null)
            OnInteractorExitArea(interactor);
    }

    #endregion

    #region Interaction Methods

    public virtual bool TryInteraction(InteractionSystem interactor)
    {
        if (!IsInteractable)
            return false;
        
        ExecuteInteraction(interactor);

        return true;
    }

    public abstract void ExecuteInteraction(InteractionSystem interactor);

    #endregion

    #region Interactors Methods

    public virtual void UpdateInteractorsInArea()
    {
        foreach(InteractionSystem interactor in InteractorsInArea)
            interactor.OnInteractionAreaUpdated(this);
    }

    public virtual void OnInteractorEnterArea(InteractionSystem interactor)
    {
        InteractorsInArea.AddLast(interactor);
        interactor.EnterInteractionArea(this);
    }

    public virtual void OnInteractorExitArea(InteractionSystem interactor)
    {
        InteractorsInArea.Remove(interactor);
        interactor.ExitInteractionArea(this);
    }

    #endregion
    
}
