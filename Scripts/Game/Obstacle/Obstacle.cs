
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Obstacle : MonoBehaviour
{
    public abstract void Execute(Collision2D collision);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Execute(collision);
    }
}