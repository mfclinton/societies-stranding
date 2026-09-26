using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private Label moneyLabel;
    private Label scoreLabel;
    private ProgressBar repProgressBar;
    private VisualElement detachElement;
    private Label planetsRemainingLabel;
    
    private GameManager gameManager;
    private TetherSystem tetherSystem;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tetherSystem = FindObjectOfType<TetherSystem>();
        
        InitializeUIDocumentElements();
    }

    private void Update()
    {
        UpdatePlanetsRemaining();
    }

    private void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.onInitializeGame += InitializeLabels;
            gameManager.onMoneyChanged += UpdateMoneyLabel;
            gameManager.onScoreChanged += UpdateScoreLabel;
            gameManager.onRepChanged += UpdateRepProgressBar;
        }

        if (tetherSystem != null)
        {
            OnTetherModified(0);
            tetherSystem.onTetherableAdded += OnTetherModified;
            tetherSystem.onTetherableRemoved += OnTetherModified;
        }
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.onInitializeGame -= InitializeLabels;
            gameManager.onMoneyChanged -= UpdateMoneyLabel;
            gameManager.onScoreChanged -= UpdateScoreLabel;
            gameManager.onRepChanged -= UpdateRepProgressBar;
        }

        if (tetherSystem != null)
        {
            tetherSystem.onTetherableAdded -= OnTetherModified;
            tetherSystem.onTetherableRemoved -= OnTetherModified;
        }
    }
    
    private void InitializeUIDocumentElements()
    {
        UIDocument uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument != null)
        {
            moneyLabel = uiDocument.rootVisualElement.Q<Label>(UIConstants.MoneyLabel);
            scoreLabel = uiDocument.rootVisualElement.Q<Label>(UIConstants.ScoreLabel);
            repProgressBar = uiDocument.rootVisualElement.Q<ProgressBar>(UIConstants.RepProgressBar);
            detachElement = uiDocument.rootVisualElement.Q<VisualElement>(UIConstants.DetachElement);
            planetsRemainingLabel = uiDocument.rootVisualElement.Q<Label>(UIConstants.PlanetsRemainingLabel);
        }
        else
        {
            Debug.LogWarning("UIDocument not found!");
        }
    }

    #region Setters

    public void InitializeLabels(int money, int score, float rep)
    {
        UpdateMoneyLabel(money, 0);
        UpdateScoreLabel(score, 0);
        UpdateRepProgressBar(rep, 0);
    }
    
    public void UpdateMoneyLabel(int value, int change)
    {
        if(moneyLabel == null)
            return;
        
        moneyLabel.text = value.ToString();
    }

    public void UpdateScoreLabel(int value, int change)
    {
        if(scoreLabel == null)
            return;
        
        scoreLabel.text = value.ToString();
    }

    public void UpdateRepProgressBar(float value, float change)
    {
        if (repProgressBar == null)
            return;
        
        repProgressBar.value = value;
    }
    
    public void OnTetherModified(int numTetherables)
    {
        if (detachElement == null)
            return;

        bool visible = 0 < numTetherables;
        detachElement.visible = visible;
    }

    public void UpdatePlanetsRemaining()
    {
        if (planetsRemainingLabel == null || gameManager.habitables == null)
            return;
        // Iterate over Habitables to count the total number of planets and how many are in the final stage
        int totalPlanets = 0;
        int finalStagePlanets = 0;
        foreach (Habitable habitable in gameManager.habitables)
        {
            totalPlanets++;
            if (habitable.CurrentStage == Habitable.Stage.FinalStage)
                finalStagePlanets++;
        }

        planetsRemainingLabel.text = $"Remaining Planets: {totalPlanets - finalStagePlanets}";
    }

    #endregion
}
