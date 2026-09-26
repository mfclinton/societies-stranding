using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class GravitationalBody : MonoBehaviour
{
    public Rigidbody2D Rb { get; private set; }

    #region Unity Callbacks

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.gravityScale = 0;
    }
    
    void OnEnable()
    {
        GravityManager.Instance.RegisterGravitationalBody(this);
    }

    void OnDisable()
    {
        GravityManager.Instance.DeregisterGravitationalBody(this);
    }

    #endregion
    
    public void ApplyForce(Vector2 force)
    {
        Rb.AddForce(force);
    }
}