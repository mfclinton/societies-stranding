using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Habitable))]
public class HabitableVisuals : MonoBehaviour
{
    [SerializeField] private Image popProgressRadialImage;
    [Header("Stage Icons")]
    [SerializeField] private Sprite stage1Icon;
    [SerializeField] private Sprite stage2Icon;
    [SerializeField] private Sprite stage3Icon;
    [SerializeField] private Sprite finalStageIcon;
    
    private Habitable habitable;

    #region Unity Callbacks

    private void Awake()
    {
        habitable = GetComponent<Habitable>();
    }

    private void OnEnable()
    {
        habitable.onInitializeHabitable += InitializeHabitableVisuals;
        habitable.OnTransitionStage += UpdateStageIcon;
        habitable.OnNewPopulationGrown += UpdatePopulationCount;
        habitable.OnMaxCapacityReached += UpdatePopulationCount;
        habitable.OnCapacityChanged += UpdatePopCapacity;
        habitable.OnPopGrowthProgressUpdated += UpdatePopGrowthProgress;
    }

    private void OnDisable()
    {
        habitable.onInitializeHabitable -= InitializeHabitableVisuals;
        habitable.OnTransitionStage -= UpdateStageIcon;
        habitable.OnNewPopulationGrown -= UpdatePopulationCount;
        habitable.OnMaxCapacityReached -= UpdatePopulationCount;
        habitable.OnCapacityChanged -= UpdatePopCapacity;
        habitable.OnPopGrowthProgressUpdated -= UpdatePopGrowthProgress;
    }

    #endregion

    #region Setters

    private void InitializeHabitableVisuals(Habitable.Stage stage, int population, int popCapacity, float popGrowthProgress)
    {
        UpdateStageIcon(stage);
        UpdatePopulationCount(population);
        UpdatePopGrowthProgress(popGrowthProgress);
        UpdatePopCapacity(popCapacity);
    }
    
    private void UpdateStageIcon(Habitable.Stage stage)
    {
        Sprite sprite;
        switch (stage)
        {
            case Habitable.Stage.Stage1:
                sprite = stage1Icon;
                break;
            case Habitable.Stage.Stage2:
                sprite = stage2Icon;
                break;
            case Habitable.Stage.Stage3:
                sprite = stage3Icon;
                break;
            case Habitable.Stage.FinalStage:
                sprite = finalStageIcon;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
        }
        
        popProgressRadialImage.sprite = sprite;
    }
    
    private void UpdatePopulationCount(int population)
    {
        // Does Nothing
    }
    
    private void UpdatePopGrowthProgress(float progress)
    {
        // Does Nothing
    }
    
    private void UpdatePopCapacity(int capacity)
    {
        // Does Nothing   
    }

    #endregion
}
