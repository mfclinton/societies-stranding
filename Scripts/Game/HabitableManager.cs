using System;
using System.Collections;
using UnityEngine;

public class HabitableManager : MonoBehaviour
{
    [SerializeField] private float timeStep = 1f;

    // Delegate for Habitable Time Step
    public delegate void OnHabitableTimeStep(float timeStep);
    public event OnHabitableTimeStep onHabitableTimeStep;

    public static HabitableManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(TimeStepCoroutine());
    }

    private IEnumerator TimeStepCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeStep);
            onHabitableTimeStep?.Invoke(timeStep);
        }
    }
}