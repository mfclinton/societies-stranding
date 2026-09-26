using UnityEngine;

[RequireComponent(typeof(ControllerBase), typeof(Rigidbody2D))]
public class SpaceshipMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrust = 90.0f;
    [SerializeField] private float rotationSpeed = 600.0f;

    [Header("Boost Settings")]
    [SerializeField] private float boostMultiplier = 2.0f;
    [SerializeField] private float boostMaxBuildUpTime = 8.0f;
    
    [Header("Neutralization Settings")]
    [SerializeField] private float neutralizeForce = 3.0f;
    [SerializeField] private float neutralizeTorque = 3.0f;

    private ControllerBase _controllerBase;
    private Rigidbody2D rb;

    private float prevThrustT;
    private float prevRotationIntensity;

    private bool isBoosting;
    private float timeBoostStarted;
    private float BoostT => isBoosting ? Mathf.Clamp01( (Time.time - timeBoostStarted) / boostMaxBuildUpTime) : 0;
    public delegate void OnShipThrust(float thrustIntensity);
    public OnShipThrust onShipThrust;
    public delegate void OnShipRotation(float rotationIntensity);
    public OnShipRotation onShipRotation;
    
    private void Awake()
    {
        _controllerBase = GetComponent<ControllerBase>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        _controllerBase.OnBoostPerformedEvent += StartBoost;
        _controllerBase.OnBoostCancelledEvent += EndBoost;
    }
    
    private void OnDisable()
    {
        _controllerBase.OnBoostPerformedEvent -= StartBoost;
        _controllerBase.OnBoostCancelledEvent -= EndBoost;
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = _controllerBase.GetMoveVector();
        ProcessMovement(moveVector);
    }
    
    private void ProcessMovement(Vector2 moveVector)
    {
        float thrust = moveVector.y;
        float rotation = -moveVector.x;
        
        if(Mathf.Abs(thrust) > Mathf.Epsilon)
            ApplyThrust(thrust);
        if(Mathf.Abs(rotation) > Mathf.Epsilon)
            ApplyRotation(rotation);
        
        // Handle Event Invokation
        float thrustT = 0.5f * (Mathf.Abs(thrust) + BoostT);
        if (Mathf.Abs(thrustT - prevThrustT) > Mathf.Epsilon)
            onShipThrust?.Invoke(thrustT);

        if (Mathf.Abs(rotation - prevRotationIntensity) > Mathf.Epsilon)
            onShipRotation?.Invoke(rotation);
        
        prevThrustT = thrustT;
        prevRotationIntensity = rotation;
        
        NeutralizeMovement();
    }
    
    private void ApplyThrust(float verticalInput)
    {
        float thrustAmount = verticalInput * thrust;
        float multiplier = Mathf.Lerp(1f, boostMultiplier, BoostT);
        thrustAmount *= multiplier;
        
        Vector2 thrustVector = transform.up * thrustAmount;
        rb.AddForce(thrustVector, ForceMode2D.Force);
    }
    
    private void ApplyRotation(float horizontalInput)
    {
        float rotationAmount = horizontalInput * rotationSpeed;
        rb.AddTorque(rotationAmount);
    }
    
    private void NeutralizeMovement()
    {
        Vector2 oppositeVelocity = -rb.velocity * neutralizeForce;
        rb.AddForce(oppositeVelocity, ForceMode2D.Force);

        float oppositeAngularVelocity = -rb.angularVelocity * neutralizeTorque;
        rb.AddTorque(oppositeAngularVelocity);
    }

    private void StartBoost()
    {
        timeBoostStarted = Time.time;
        isBoosting = true;
    }
    
    private void EndBoost()
    {
        isBoosting = false;    
    }
}
