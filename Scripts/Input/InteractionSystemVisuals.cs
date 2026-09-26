using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(InteractionSystem))]
public class InteractionSystemVisuals : MonoBehaviour
{
    [SerializeField] private Image interactionSpriteImage;
    
    private InteractionSystem interactionSystem;

    private void Awake()
    {
        interactionSystem = GetComponent<InteractionSystem>();

        SetSprite(null);
    }

    private void OnEnable()
    {
        interactionSystem.OnInteractionAreaUpdatedEvent += UpdateInteractionVisual;
        interactionSystem.OnEnterInteractionAreaEvent += UpdateInteractionVisual;
        interactionSystem.OnExitInteractionAreaEvent += UpdateInteractionVisual;
    }

    private void OnDisable()
    {
        interactionSystem.OnInteractionAreaUpdatedEvent -= UpdateInteractionVisual;
        interactionSystem.OnEnterInteractionAreaEvent -= UpdateInteractionVisual;
        interactionSystem.OnExitInteractionAreaEvent -= UpdateInteractionVisual;
    }

    private void UpdateInteractionVisual(InteractionAreaBase interactionAreaBase)
    {
        Sprite sprite = GetInteractionAreaSprite(interactionAreaBase);
        SetSprite(sprite);
    }
    
    private void SetSprite(Sprite sprite)
    {
        if(sprite == null)
            interactionSpriteImage.gameObject.SetActive(false);
        else
            interactionSpriteImage.gameObject.SetActive(true);
        
        interactionSpriteImage.sprite = sprite;
    }

    private Sprite GetInteractionAreaSprite(InteractionAreaBase interactionAreaBase)
    {
        if (interactionAreaBase == null)
            return null;
        
        var bindingSpriteDictionary = BindingSpriteMapHolder.Instance.BindingSpriteMap.BindingSpriteDictionary;
        ControlSchemes controlScheme = interactionSystem.controllerBase.CurrentControlScheme;
        ActionNames actionName = interactionAreaBase.ActionName;
        
        Sprite sprite = null;
        if (bindingSpriteDictionary.ContainsKey(controlScheme) && bindingSpriteDictionary[controlScheme].ContainsKey(actionName))
            sprite = bindingSpriteDictionary[controlScheme][actionName];

        return sprite;
    }
}
