using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TetherSystem), typeof(LineRenderer))]
public class TetherSystemVisuals : MonoBehaviour
{
    private TetherSystem tetherSystem;
    private LineRenderer lineRenderer;

    public int numPointsPerTether = 100;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 2f;

    private void Awake()
    {
        tetherSystem = GetComponent<TetherSystem>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        tetherSystem.onTetherableRemoved += ClearLines;
    }

    private void OnDisable()
    {
        tetherSystem.onTetherableRemoved -= ClearLines;
    }

    private void FixedUpdate()
    {
        var tetherables = tetherSystem.tetherables;
        if (tetherables.Count <= 0)
            return;

        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        List<Tetherable> tetherables = tetherSystem.tetherables;

        int totalPoints = numPointsPerTether * tetherables.Count + 1; // +1 for the starting point
        lineRenderer.positionCount = totalPoints;

        Vector3 currentStartPoint = transform.position;
        int currentPointIndex = 0;

        for (int tIndex = 0; tIndex < tetherables.Count; tIndex++)
        {
            Vector3 endPoint = tetherables[tIndex].transform.position;
            Vector3 direction = (endPoint - currentStartPoint).normalized;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;

            for (int i = 0; i < numPointsPerTether; i++)
            {
                float t = i / (float)numPointsPerTether;
                Vector3 interpolatedPoint = Vector3.Lerp(currentStartPoint, endPoint, t);

                if (i != 0 && i != numPointsPerTether - 1) // Add wavy effect only to the middle points, not the start or end
                {
                    float yOffset = Mathf.Sin(t * waveFrequency * Mathf.PI * 2) * waveAmplitude;
                    interpolatedPoint += perpendicular * yOffset;
                }

                lineRenderer.SetPosition(currentPointIndex++, interpolatedPoint);
            }

            currentStartPoint = endPoint; // Update the start point for the next segment
        }

        lineRenderer.SetPosition(currentPointIndex, tetherables[tetherables.Count - 1].transform.position); // Set the endpoint for the last tetherable
    }


    private void ClearLines(int numLines)
    {
        lineRenderer.positionCount = 0;
    }
}