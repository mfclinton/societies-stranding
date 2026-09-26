using System;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    [SerializeField] private float desiredOrbitalHeight = 10f;
    [SerializeField] private float stabilizationGain = 1f;
    [Tooltip("Orbital stabilization only applies for other GravitySources with <= 'orbitIndex'")]
    [SerializeField] private int orbitIndex;
    
    LinkedList<GravitationalBody> gravitationalBodiesInOrbit = new LinkedList<GravitationalBody>();
    public Rigidbody2D Rb { get; private set; }
    
    #region Registry

    void Awake()
    {
        gravitationalBodiesInOrbit = new LinkedList<GravitationalBody>();
        Rb = GetComponent<Rigidbody2D>();
        if (Rb == null)
            Rb = GetComponentInParent<Rigidbody2D>();
        Rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        foreach (GravitationalBody gravitationalBody in gravitationalBodiesInOrbit)
        {
            ApplyOrbitalStabilizationForce(gravitationalBody);
        }
    }

    void OnEnable()
    {
        GravityManager.Instance.RegisterGravitySource(this);
    }

    void OnDisable()
    {
        GravityManager.Instance.DeregisterGravitySource(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GravitySource otherGravitySource = other.GetComponent<GravitySource>();
        if (otherGravitySource != null && orbitIndex < otherGravitySource.orbitIndex)
            return;
        
        GravitationalBody gravitationalBody = other.GetComponent<GravitationalBody>();
        if (gravitationalBody != null)
        {
            gravitationalBodiesInOrbit.AddLast(gravitationalBody);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GravitationalBody gravitationalBody = other.GetComponent<GravitationalBody>();
        if (gravitationalBody != null)
        {
            gravitationalBodiesInOrbit.Remove(gravitationalBody);
        }
    }

    #endregion
    
    public void ApplyOrbitalStabilizationForce(GravitationalBody gravitationalBody)
    {
        float gravitationalConstant = GravityManager.Instance.GravitationalConstant;
        Rigidbody2D otherRigidbody = gravitationalBody.Rb;
        
        Vector2 offset = Rb.transform.position - otherRigidbody.transform.position;
        float distance = offset.magnitude;

        float orbitalVelocity = Mathf.Sqrt(gravitationalConstant * Rb.mass / Mathf.Max(distance, desiredOrbitalHeight));
        Vector2 desiredVelocityDirection = new Vector2(-offset.y, offset.x).normalized;
        Vector2 desiredVelocity = desiredVelocityDirection * orbitalVelocity;

        Vector2 velocityDifference = desiredVelocity - otherRigidbody.velocity;
        float forceMagnitude = stabilizationGain * velocityDifference.magnitude * otherRigidbody.mass / Time.fixedDeltaTime;
        Vector2 forceDirection = velocityDifference.normalized;

        gravitationalBody.ApplyForce(forceDirection * forceMagnitude);
    }
}