

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : Obstacle
{
    [SerializeField] private float untetherKineticEnergyThresh = 40f;
    
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Execute(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TetherSystem ts = collision.gameObject.GetComponent<TetherSystem>();
            Vector2 relativeVelocity = rb.velocity - ts.rb.velocity;
            float kineticEnergy = 0.5f * rb.mass * relativeVelocity.sqrMagnitude;
            Debug.Log("Kinetic Energy: " + kineticEnergy);
            
            if(ts != null && kineticEnergy > untetherKineticEnergyThresh)
                ts.TryUntetherLast();
        }
    }
}