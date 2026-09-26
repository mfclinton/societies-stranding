using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

struct GravityJob : IJobParallelFor
{
    public NativeArray<Vector2> forces;
    
    [ReadOnly] public float gravitationalConstant;
    [ReadOnly] public float maxGravityDistance;

    [ReadOnly] public NativeArray<Vector2> gravitationalBodiesPositions;
    [ReadOnly] public NativeArray<float> gravitationalBodiesMasses;
    [ReadOnly] public NativeArray<Vector2> gravitySourcePositions;
    [ReadOnly] public NativeArray<float> gravitySourceMasses;

    public void Execute(int index)
    {
        Vector2 currentPos = gravitationalBodiesPositions[index];
        float currentMass = gravitationalBodiesMasses[index];
        
        Vector2 totalForce = Vector2.zero;
        for (int i = 0; i < gravitySourcePositions.Length; i++)
        {
            Vector2 directionToSource = (gravitySourcePositions[i] - currentPos).normalized;
            float distance = Vector2.Distance(gravitySourcePositions[i], currentPos);

            if (distance < maxGravityDistance && distance > 0.1f)  // Avoid extreme forces at very small distances
            {
                float forceMagnitude = gravitationalConstant * (gravitySourceMasses[i] * currentMass) / (distance * distance);
                Vector2 force = directionToSource * forceMagnitude;
                totalForce += force;
            }
        }

        forces[index] = totalForce;
    }
}