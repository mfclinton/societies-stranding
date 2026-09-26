
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ControllerBase))]
public class InteractionSystem : MonoBehaviour
{
    // Define delegate (if using non-generic pattern).
    public delegate void InteractionAreaDelegate(InteractionAreaBase interactionAreaBase);

    // Declare the event using the delegate (if using non-generic pattern).
    public event InteractionAreaDelegate OnInteractionAreaUpdatedEvent;
    public event InteractionAreaDelegate OnEnterInteractionAreaEvent;
    public event InteractionAreaDelegate OnExitInteractionAreaEvent;

    private LinkedList<InteractionAreaBase> interactionAreasIn = new LinkedList<InteractionAreaBase>();
    public ControllerBase controllerBase { get; private set; }
    
    private void Awake()
    {
        controllerBase = GetComponent<ControllerBase>();
    }

    private void OnEnable()
    {
        controllerBase.OnInteractPerformedEvent += TryInteraction;
    }
    
    private void OnDisable()
    {
        controllerBase.OnInteractPerformedEvent -= TryInteraction;
    }

    #region Interaction Methods
    
    public void TryInteraction(ActionNames actionName)
    {
        InteractionAreaBase curInteractionArea = GetInteractionArea(actionName);
        if (curInteractionArea == null)
            return;
        
        curInteractionArea.TryInteraction(this);
    }

    public void OnInteractionAreaUpdated(InteractionAreaBase newInteractionAreaBase)
    {
        InteractionAreaBase curInteractionArea = GetInteractionArea();
        OnInteractionAreaUpdatedEvent?.Invoke(curInteractionArea);
    }

    public void EnterInteractionArea(InteractionAreaBase newInteractionAreaBase)
    {
        interactionAreasIn.AddLast(newInteractionAreaBase);
        InteractionAreaBase curInteractionArea = GetInteractionArea();
        OnEnterInteractionAreaEvent?.Invoke(curInteractionArea);
    }

    public void ExitInteractionArea(InteractionAreaBase newInteractionAreaBase)
    {
        interactionAreasIn.Remove(newInteractionAreaBase);
        InteractionAreaBase curInteractionArea = GetInteractionArea();
        OnExitInteractionAreaEvent?.Invoke(curInteractionArea);
    }
    
    private InteractionAreaBase GetInteractionArea()
    {
        return interactionAreasIn.FirstOrDefault(ia => ia.IsInteractable);
    }

    private InteractionAreaBase GetInteractionArea(ActionNames actionName)
    {
        return interactionAreasIn.FirstOrDefault(ia => ia.IsInteractable && ia.ActionName == actionName);
    }

    #endregion
}