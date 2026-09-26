using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Tetherable : MonoBehaviour
{
    [Header("Tether Settings")]
    [SerializeField] private float arrivalThreshold = 0.1f;
    [SerializeField] private float springConstant  = 100f;
    [SerializeField] private float dampingRatio = 1.0f;

    [Header("Collision Cooldown")]
    [SerializeField] private LayerMask collisionCooldownLayerMask;
    [SerializeField] private float collisionCooldownDuration = 0.5f;
    [SerializeField] private Color collisionCooldownColor = new Color(1f, 1f, 1f, 0.5f);

    public bool isTethered { get; private set; } = false;
    public TetherSystem tetherSystem { get; private set; }
    public Vector2 goalPosition { get; private set; }

    private LayerMask defaultExcludeLayerMask;
    private Coroutine disableCollisionCoroutine;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    #region Unity Callbacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultExcludeLayerMask = rb.excludeLayers;
    }

    private void OnDisable()
    {
        tetherSystem?.Untether(this);
        if(disableCollisionCoroutine != null)
            StopCoroutine(disableCollisionCoroutine);
    }

    #endregion

    public void SetGoalPosition(Vector2 goalPosition)
    {
        this.goalPosition = goalPosition;
    }
    
    public void SetTethered(bool tethered, TetherSystem tetherSystem)
    {
        isTethered = tethered;
        this.tetherSystem = tetherSystem;
        
        // On state change, disable collisions for a bit
        if(disableCollisionCoroutine != null)
            StopCoroutine(disableCollisionCoroutine);

        if(gameObject.activeInHierarchy)
            disableCollisionCoroutine = StartCoroutine(DisableCollisionCoroutine());
    }
    
    private IEnumerator DisableCollisionCoroutine()
    {
        rb.excludeLayers = collisionCooldownLayerMask;
        if (spriteRenderer != null)
            spriteRenderer.color = collisionCooldownColor;
        
        yield return new WaitForSeconds(collisionCooldownDuration);
        
        rb.excludeLayers = defaultExcludeLayerMask;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    public void MoveTowardsGoal()
    {
        Vector2 displacement = (Vector2)transform.position - goalPosition;
        Vector2 velocity = rb.velocity;

        float dampingCoefficient = 2.0f * Mathf.Sqrt(springConstant * rb.mass) * dampingRatio;
        Vector2 springForce = -springConstant * displacement;
        Vector2 dampingForce = -dampingCoefficient * velocity;

        Vector2 totalForce = springForce + dampingForce;

        if (displacement.magnitude > arrivalThreshold)
        {
            rb.AddForce(totalForce);
        }
    }
    
    public void AddForce(Vector2 force, ForceMode2D forceMode2D = ForceMode2D.Force)
    {
        rb.AddForce(force, forceMode2D);
    }
}