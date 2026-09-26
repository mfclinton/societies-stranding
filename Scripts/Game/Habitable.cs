using UnityEngine;
using System.Collections;

public class Habitable : MonoBehaviour
{
    public enum Stage { Stage1, Stage2, Stage3, FinalStage }
    
    [Header("Population Settings")]
    [SerializeField] private int startingPopulation = 0;
    [SerializeField] private int popCapacity = 1000;
    [SerializeField] private float populationGrowthMultiplier = 10f;

    [Header("Prefab Settings")]
    [SerializeField] private Pod[] populationPodPrefabs;
    [SerializeField] private Vector2 minMaxPodSpawnRadius = new Vector2(5f, 10f);
    
    [Header("Stage Settings")]
    [SerializeField] private Stage stage = Stage.Stage1;
    [SerializeField] private float transitionFreq = 1f * 5f;
    [SerializeField] private float eventFreq = 1f;
    
    public Stage CurrentStage => stage;
    public int Population { get; private set; }
    public int PopCapacity => popCapacity;
    public float PopGrowthProgress { get; private set; } = 0f;
    
    private Transform populationPodParent;
    private Coroutine eventCoroutine;

    #region Events

    public delegate void OnInitializeHabitable(Stage stage, int population, int popCapacity, float popGrowthProgress);
    public delegate void OnTransitionStageHandler(Stage stage);
    public delegate void PopulationChangeHandler(int population);
    public delegate void CapacityChangeHandler(int capacity);
    public delegate void PopGrowthProgressHandler(float progress);
    public delegate void PopPodSpawnedHandler(Pod pod);
    
    public event OnTransitionStageHandler OnTransitionStage;
    public event PopulationChangeHandler OnNewPopulationGrown;
    public event PopulationChangeHandler OnMaxCapacityReached;
    public event CapacityChangeHandler OnCapacityChanged;
    public event PopGrowthProgressHandler OnPopGrowthProgressUpdated;
    public event OnInitializeHabitable onInitializeHabitable;
    public event PopPodSpawnedHandler OnPopPodSpawned;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        populationPodParent = GameObject.FindWithTag("PodParent")?.transform;
        
        Population = startingPopulation;

        //If the name of the parent game object is not "Planet Variant 2" or "Planet Variant 5", then the stage is randomized
        if (!transform.parent.name.Equals("Planet Variant 2") && !transform.parent.name.Equals("Planet Variant 5"))
            stage = (Stage)Random.Range(0, 3);

        onInitializeHabitable?.Invoke(stage, Population, PopCapacity, PopGrowthProgress);
        
        StartEventCoroutine();
    }

    private void OnEnable()
    {
        HabitableManager habitableManager = FindObjectOfType<HabitableManager>();
        if(habitableManager != null)
            habitableManager.onHabitableTimeStep += HandlePopulationGrowth;
    }
    
    private void OnDisable()
    {
        HabitableManager habitableManager = FindObjectOfType<HabitableManager>();
        if(habitableManager != null)
            habitableManager.onHabitableTimeStep -= HandlePopulationGrowth;
    }

    #endregion

    public void HandlePopulationGrowth(float timeStep)
    {
        PopGrowthProgress += timeStep * populationGrowthMultiplier;
        OnPopGrowthProgressUpdated?.Invoke(PopGrowthProgress);
        
        if (PopGrowthProgress >= 100f)
            CreateNewPopulation();
    }
    
    public void CreateNewPopulation()
    {
        PopGrowthProgress = 0f;
        if (Population == PopCapacity)
            return;
        
        Population++;

        OnNewPopulationGrown?.Invoke(Population);
        if (Population == PopCapacity)
            OnMaxCapacityReached?.Invoke(Population);
    }
    
    public void AddCapacity(int amount)
    {
        popCapacity += amount;
        OnCapacityChanged?.Invoke(popCapacity);
    }
    
    private void StartEventCoroutine()
    {
        if(eventCoroutine != null)
            StopCoroutine(eventCoroutine);
        
        eventCoroutine = StartCoroutine(EventCoroutine());
    }
    
    private IEnumerator EventCoroutine()
    {
        while (true)
        {
            if (stage == Stage.FinalStage)
            {
                PopPodEvent();
                yield return new WaitForSeconds(eventFreq);
            }
            else
            {
                TransitionStageEvent();
                yield return new WaitForSeconds(transitionFreq);
            }
        }
    }

    #region Transition Events

    private void TransitionStageEvent()
    {
        bool transitionStageEventFired = Poisson.SamplePoisson();
        if (transitionStageEventFired)
            TransitionStage();
    }

    private void TransitionStage()
    {
        switch (stage)
        {
            case Stage.Stage1:
                stage = Stage.Stage2;
                break;
            case Stage.Stage2:
                stage = Stage.Stage3;
                break;
            case Stage.Stage3:
                stage = Stage.FinalStage;
                break;
            case Stage.FinalStage:
                break;
        }
        
        OnTransitionStage?.Invoke(stage);
    }

    #endregion

    #region Final Stage Events

    private void PopPodEvent()
    {
        bool newPodEventFired = Poisson.SamplePoisson();
        if (newPodEventFired)
            SpawnPod();
    }
    
    private void SpawnPod()
    {
        Vector3 spawnPos = transform.position + (Vector3)Random.insideUnitCircle.normalized * Random.Range(minMaxPodSpawnRadius.x, minMaxPodSpawnRadius.y);
        Pod podPrefabChosen = populationPodPrefabs[Random.Range(0, populationPodPrefabs.Length)];
        
        Pod pod = Instantiate(podPrefabChosen);
        pod.transform.position = spawnPos;
        pod.transform.SetParent(populationPodParent);
        
        OnPopPodSpawned?.Invoke(pod);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minMaxPodSpawnRadius.x);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minMaxPodSpawnRadius.y);
    }
}