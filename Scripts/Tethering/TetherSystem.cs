using System;
using UnityEngine;
using System.Collections.Generic;
using CircularBuffer;

[RequireComponent(typeof(ControllerBase))]
public class TetherSystem : MonoBehaviour
{
    [SerializeField] private int maxTetherables = 2;
    [SerializeField] private float untetherDistance = 20;
    [SerializeField] private int tetherableHistoryIndexOffset = 5;
    [SerializeField] private float historyDistanceStep = 1f;
    [SerializeField] private float untetherForceMultiplier = 2f;
    
    private ControllerBase controller;
    public Rigidbody2D rb { get; private set; } 
    
    private CircularBuffer<Vector2> historicalPositions;
    public List<Tetherable> tetherables { get; private set; } = new List<Tetherable>();

    // delegates for tethers and untethering
    public delegate void OnTetherablesModified(int numTetherables);
    public event OnTetherablesModified onTetherableAdded;
    public event OnTetherablesModified onTetherableRemoved;
    
    #region Unity Callbacks

    private void Awake()
    {
        controller = GetComponent<ControllerBase>();
        rb = GetComponent<Rigidbody2D>();
        
        int maxHistoricalPositions = (maxTetherables + 1) * tetherableHistoryIndexOffset;
        historicalPositions = new CircularBuffer<Vector2>(maxHistoricalPositions);
        historicalPositions.PushBack(transform.position);
    }

    private void OnEnable()
    {
        controller.OnUnTetherPerformedEvent += TryUntetherLast;
    }
    
    private void OnDisable()
    {
        controller.OnUnTetherPerformedEvent -= TryUntetherLast;
    }

    private void FixedUpdate()
    {
        UpdateHistoricalPositions();
        UpdateTetherables();
        CheckUntethering();
    }

    #endregion

    private void UpdateHistoricalPositions()
    {
        float distChange = Vector2.Distance(transform.position, historicalPositions.Back());
        if (distChange < historyDistanceStep)
            return;

        historicalPositions.PushBack(transform.position);
    }

    private void UpdateTetherables()
    {
        for (int i = 0; i < tetherables.Count; i++)
        {
            int historyIndex = historicalPositions.Size - (i + 1) * tetherableHistoryIndexOffset;
            if (historyIndex < 0)
                continue;

            tetherables[i].SetGoalPosition(historicalPositions[historyIndex]);
            tetherables[i].MoveTowardsGoal();
        }
    }

    private void CheckUntethering()
    {
        for (int i = 0; i < tetherables.Count; i++)
            if (Vector2.Distance(tetherables[i].transform.position, tetherables[i].goalPosition) > untetherDistance)
                Untether(i);
    }

    public void Untether(int index)
    {
        if (index < 0 || tetherables.Count <= index)
            return;
        
        Tetherable tetherable = tetherables[index];
        tetherables[index].SetTethered(false, this);
        tetherables.RemoveAt(index);
        
        tetherable.AddForce(rb.velocity * untetherForceMultiplier, ForceMode2D.Impulse);
        
        onTetherableRemoved?.Invoke(tetherables.Count);
    }
    
    public void Untether(Tetherable tetherable)
    {
        int index = tetherables.IndexOf(tetherable);
        Untether(index);
    }
    
    public void TryUntetherLast()
    {
        Untether(tetherables.Count - 1);
    }
    
    public void AddTetherable(Tetherable tetherable)
    {
        tetherable.SetTethered(true, this);
        tetherables.Add(tetherable);
        onTetherableAdded?.Invoke(tetherables.Count);
    }

    public bool TryAttachTetherable(Tetherable tetherable)
    {
        // TODO: Take into account the tetherable's dist when deciding whether to tether
        float dist = Vector2.Distance(tetherable.transform.position, transform.position);
        if (tetherables.Count < maxTetherables && dist < untetherDistance)
        {
            AddTetherable(tetherable);
            return true;
        }

        return false;
    }
    
    void OnDrawGizmos()
    {
        if (tetherables == null || historicalPositions == null)
            return;
        
        foreach (Tetherable tetherable in tetherables)
        {
            if(tetherable == null)
                continue;
        
            Vector2 goalPosition = tetherable.goalPosition;
        
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(goalPosition, 0.5f);
        
            Gizmos.color = Color.red;
            Gizmos.DrawLine(tetherable.transform.position, goalPosition);
        }

        for (int i = 0; i < historicalPositions.Size; i++)
        {
            Color color = Color.Lerp(Color.blue, Color.magenta, (float)i / historicalPositions.Size);
            Gizmos.color = color;
            Gizmos.DrawSphere(historicalPositions[i], 0.1f);
        }
    }
}