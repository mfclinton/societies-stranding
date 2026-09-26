using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

public class GravityManager : MonoBehaviour
{
    [SerializeField] private float gravitationalConstant = 1f;
    [SerializeField] private float maxGravityDistance = 1000f;
    
    public float GravitationalConstant => gravitationalConstant;
    
    public static GravityManager Instance;

    private List<GravitySource> gravitySources = new List<GravitySource>();
    private List<GravitationalBody> gravitationalBodies = new List<GravitationalBody>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Gravity List Registration

    public void RegisterGravitySource(GravitySource gravitySource)
    {
        if (!gravitySources.Contains(gravitySource))
        {
            gravitySources.Add(gravitySource);
        }
    }

    public void DeregisterGravitySource(GravitySource gravitySource)
    {
        if (gravitySources.Contains(gravitySource))
        {
            gravitySources.Remove(gravitySource);
        }
    }
    
    public void RegisterGravitationalBody(GravitationalBody gravitationalBody)
    {
        if (!gravitationalBodies.Contains(gravitationalBody))
        {
            gravitationalBodies.Add(gravitationalBody);
        }
    }

    public void DeregisterGravitationalBody(GravitationalBody gravitationalBody)
    {
        if (gravitationalBodies.Contains(gravitationalBody))
        {
            gravitationalBodies.Remove(gravitationalBody);
        }
    }

    #endregion
    
    void FixedUpdate()
    {
        NativeArray<Vector2> forces = new NativeArray<Vector2>(gravitationalBodies.Count, Allocator.TempJob);
        
        NativeArray<Vector2> gravitationalBodiesPositions = new NativeArray<Vector2>(gravitationalBodies.Count, Allocator.TempJob);
        NativeArray<float> gravitationalBodiesMasses = new NativeArray<float>(gravitationalBodies.Count, Allocator.TempJob);
        
        NativeArray<Vector2> gravitySourcePositions = new NativeArray<Vector2>(gravitySources.Count, Allocator.TempJob);
        NativeArray<float> gravitySourceMasses = new NativeArray<float>(gravitySources.Count, Allocator.TempJob);
    
        for (int i = 0; i < gravitationalBodies.Count; i++)
        {
            gravitationalBodiesPositions[i] = gravitationalBodies[i].transform.position;
            gravitationalBodiesMasses[i] = gravitationalBodies[i].Rb.mass;
        }
        
        for (int i = 0; i < gravitySources.Count; i++)
        {
            gravitySourcePositions[i] = gravitySources[i].transform.position;
            gravitySourceMasses[i] = gravitySources[i].Rb.mass;
        }

        var gravityJob = new GravityJob
        {
            forces = forces,
            gravitationalConstant = gravitationalConstant,
            maxGravityDistance = maxGravityDistance,
            gravitationalBodiesPositions = gravitationalBodiesPositions,
            gravitationalBodiesMasses = gravitationalBodiesMasses,
            gravitySourcePositions = gravitySourcePositions,
            gravitySourceMasses = gravitySourceMasses
        };

        JobHandle handle = gravityJob.Schedule(gravitationalBodies.Count, 64);
        handle.Complete();

        for (int i = 0; i < gravitationalBodies.Count; i++)
        {
            // print("Applying force to " + gravitationalBodies[i].name + ": " + forces[i] + "");
            gravitationalBodies[i].ApplyForce(forces[i]);
        }

        forces.Dispose();
        gravitationalBodiesPositions.Dispose();
        gravitationalBodiesMasses.Dispose();
        gravitySourcePositions.Dispose();
        gravitySourceMasses.Dispose();
    }
}