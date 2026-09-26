using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ControllerBase : MonoBehaviour
{
    // delegates
    public delegate void InteractHandler(ActionNames actionName);
    public event InteractHandler OnInteractPerformedEvent;
    
    public delegate void TetherHandler();
    public event TetherHandler OnUnTetherPerformedEvent;
    
    public delegate void BoostHandler();
    public event BoostHandler OnBoostPerformedEvent;
    public event BoostHandler OnBoostCancelledEvent;
    
    // local variables
    protected PlayerControls input;
    protected ControlSchemes currentControlScheme;
    protected Vector2 moveVector;

    #region Getters

    public virtual PlayerControls Input => input;
    public virtual ControlSchemes CurrentControlScheme => currentControlScheme;
    public virtual Vector2 GetMoveVector() => moveVector;

    #endregion
    
    #region Unity Callbacks

    protected virtual void Awake()
    {
        input = new PlayerControls();

        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        
        input.Player.PrimaryInteract.performed += OnPrimaryInteractPerformed;
        input.Player.SecondaryInteract.performed += OnSecondaryInteractPerformed;
        input.Player.UnTether.performed += OnUnTetherPerformed;
        
        input.Player.Boost.performed += OnBoostPerformed;
        input.Player.Boost.canceled += OnBoostCancelled;
    }

    protected virtual void OnEnable() => input.Player.Enable();
    protected virtual void OnDisable() => input.Player.Disable();

    #endregion

    #region Input Methods

    protected virtual void UpdateControlScheme(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Gamepad)
        {
            currentControlScheme = ControlSchemes.Gamepad; // TODO CONST
        }
        else if (ctx.control.device is Keyboard)
        {
            currentControlScheme = ControlSchemes.Keyboard;
        }
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
        UpdateControlScheme(ctx);
    }

    protected virtual void OnMovementCancelled(InputAction.CallbackContext ctx)
    {
        moveVector = Vector2.zero;
    }
    
    protected virtual void OnPrimaryInteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteractPerformedEvent?.Invoke(ActionNames.PrimaryInteract);
        UpdateControlScheme(ctx);
    }

    protected virtual void OnSecondaryInteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteractPerformedEvent?.Invoke(ActionNames.SecondaryInteract);
        UpdateControlScheme(ctx);
    }
    
    protected virtual void OnUnTetherPerformed(InputAction.CallbackContext ctx)
    {
        OnUnTetherPerformedEvent?.Invoke();
        UpdateControlScheme(ctx);
    }
    
    protected virtual void OnBoostPerformed(InputAction.CallbackContext ctx)
    {
        OnBoostPerformedEvent?.Invoke();
        UpdateControlScheme(ctx);
    }
    
    protected virtual void OnBoostCancelled(InputAction.CallbackContext ctx)
    {
        OnBoostCancelledEvent?.Invoke();
        UpdateControlScheme(ctx);
    }

    #endregion

}
