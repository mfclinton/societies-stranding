using System;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter thrustserEmitter;
    [SerializeField] private StudioEventEmitter orientationThrusterEmitter;
    [SerializeField] private StudioEventEmitter acquiredEmitter;
    [SerializeField] private StudioEventEmitter droppedOffEmitter;
    [SerializeField] private StudioEventEmitter rockSmashEmitter;

    SpaceshipMovement spaceshipMovement;

    private void Awake()
    {
        spaceshipMovement = FindObjectOfType<SpaceshipMovement>();
    }

    private void OnEnable()
    {
        spaceshipMovement.onShipThrust += PlayThrusterSound;
        spaceshipMovement.onShipRotation += PlayOrientationThrusterSound;
    }
    
    private void OnDisable()
    {
        spaceshipMovement.onShipThrust -= PlayThrusterSound;
        spaceshipMovement.onShipRotation -= PlayOrientationThrusterSound;
    }
    
    private void PlayThrusterSound(float thrust)
    {
        if (!FMODUnity.RuntimeManager.HasBankLoaded("SPACE JAM"))
            return;
        
        float t = Mathf.Abs(thrust);
        
        if(t <= Mathf.Epsilon && thrustserEmitter.IsPlaying())
            thrustserEmitter.Stop();
        else if (!thrustserEmitter.IsPlaying()) 
            thrustserEmitter.Play();
        
        thrustserEmitter.SetParameter("Speed", thrust);
    }
    
    private void PlayOrientationThrusterSound(float orientationThrust)
    {
        // TODO: Remove this since orientation thrusters do nothing
        // float t = Mathf.Abs(orientationThrust);
        //
        // if(t <= Mathf.Epsilon && orientationThrusterEmitter.IsPlaying())
        //     orientationThrusterEmitter.Stop();
        // else if (!orientationThrusterEmitter.IsPlaying()) 
        //     orientationThrusterEmitter.Play();
    }
}