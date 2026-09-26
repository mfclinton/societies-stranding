using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BindingSpritePair
{
    public ControlSchemes controlScheme;
    [SerializeField] private ActionBinding actionBinding;
    public Sprite sprite;
    
    [System.Serializable]
    public enum ActionBinding
    {
        Movement,
        PrimaryInteract,
        SecondaryInteract
    }
    
    public ActionNames GetActionName()
    {
        switch (actionBinding)
        {
            case ActionBinding.Movement:
                return ActionNames.Movement;
            case ActionBinding.PrimaryInteract:
                return ActionNames.PrimaryInteract;
            case ActionBinding.SecondaryInteract:
                return ActionNames.SecondaryInteract;
            default:
                return ActionNames.Movement;
        }
    }
}

[CreateAssetMenu(fileName = "NewBindingSpriteMap", menuName = "BindingSpriteMap")]
public class BindingSpriteMap : ScriptableObject
{
    public List<BindingSpritePair> pairs = new List<BindingSpritePair>();

    // Not serialized; initialized on demand
    private Dictionary<ControlSchemes, Dictionary<ActionNames, Sprite>> bindingSpriteDictionary;

    public Dictionary<ControlSchemes, Dictionary<ActionNames, Sprite>> BindingSpriteDictionary
    {
        get
        {
            if (bindingSpriteDictionary == null)
            {
                bindingSpriteDictionary = new Dictionary<ControlSchemes, Dictionary<ActionNames, Sprite>>();

                foreach (var pair in pairs)
                {
                    // Assuming pair has ControlScheme, ActionName, and Sprite properties
                    ControlSchemes controlScheme = pair.controlScheme;
                    ActionNames actionName = pair.GetActionName();
                    Sprite sprite = pair.sprite;

                    if (!bindingSpriteDictionary.ContainsKey(controlScheme))
                    {
                        bindingSpriteDictionary[controlScheme] = new Dictionary<ActionNames, Sprite>();
                    }

                    bindingSpriteDictionary[controlScheme][actionName] = sprite;
                }
            }

            return bindingSpriteDictionary;
        }
    }
}
