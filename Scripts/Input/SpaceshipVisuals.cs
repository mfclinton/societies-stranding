using System;
using UnityEngine;

[RequireComponent(typeof(SpaceshipMovement))]
public class SpaceshipVisuals : MonoBehaviour
{
    [Header("Engine Particles")]
    [SerializeField] private ParticleSystem leftEngineParticles;
    [SerializeField] private ParticleSystem rightEngineParticles;

    private void Awake()
    {
        SpaceshipMovement spaceshipMovement = GetComponent<SpaceshipMovement>();
        spaceshipMovement.onShipThrust += UpdateThrustParticles;
        
        InitializeThrusterParticles();
    }
    
    private void InitializeThrusterParticles()
    {
        var leftEmission = leftEngineParticles.emission;
        var rightEmission = rightEngineParticles.emission;

        leftEmission.rateOverTime = 0;
        rightEmission.rateOverTime = 0;
    }

    public void UpdateThrustParticles(float thrustIntensity)
    {
        float emissionRate = Mathf.Lerp(0, 50, thrustIntensity); 
        float particleSpeed = Mathf.Lerp(1, 3, thrustIntensity); 
        float particleLifetime = Mathf.Lerp(1, 1.2f, thrustIntensity); 

        var leftEmission = leftEngineParticles.emission;
        var rightEmission = rightEngineParticles.emission;
        leftEmission.rateOverTime = emissionRate;
        rightEmission.rateOverTime = emissionRate;
        
        var leftMain = leftEngineParticles.main;
        var rightMain = rightEngineParticles.main;
        leftMain.startSpeed = particleSpeed;
        rightMain.startSpeed = particleSpeed;
        leftMain.startLifetime = particleLifetime;
        rightMain.startLifetime = particleLifetime;
    }
}