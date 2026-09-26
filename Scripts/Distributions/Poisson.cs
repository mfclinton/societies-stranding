using System;
using UnityEngine;

public static class Poisson
{
    private const float lambdaPerFrame = 1f / 60f;
    private static System.Random rng = new System.Random();
    
    public static bool SamplePoisson()
    {
        // P(X = 0) for a Poisson distribution
        float pZero = Mathf.Exp(-lambdaPerFrame);

        // Generate a random value
        float u = (float)rng.NextDouble();

        // If the random value is greater than pZero, it means at least 1 event happened this frame
        return u > pZero;
    }
}