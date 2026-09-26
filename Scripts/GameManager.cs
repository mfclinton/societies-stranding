using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;  // Added for IEnumerator
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int startingMoney = 0;
    [SerializeField] private int startingScore = 0;
    [SerializeField] private float startingRep = 100f;
    [SerializeField] private float repReductionAmount = -2f;
    [SerializeField] private float repReductionInterval = 20f;

    public int money { get; private set; }
    public int score { get; private set; }
    public float rep { get; private set; } = 100f;
    public LinkedList<Habitable> habitables = new LinkedList<Habitable>();

    #region Event Delegates

    public delegate void OnInitializeGame(int money, int score, float rep);
    public event OnInitializeGame onInitializeGame;

    public delegate void OnMoneyChanged(int total, int change);
    public event OnMoneyChanged onMoneyChanged;
    public delegate void OnScoreChanged(int total, int change);
    public event OnScoreChanged onScoreChanged;
    public delegate void OnRepChanged(float total, float change);
    public event OnRepChanged onRepChanged;

    #endregion

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        habitables = new LinkedList<Habitable>(FindObjectsOfType<Habitable>());
    }

    void Start()
    {
        InitializeGameState();
        StartCoroutine(ReduceReputationOverTime());
    }

    private void InitializeGameState()
    {
        money = startingMoney;
        score = startingScore;
        rep = startingRep;

        onInitializeGame?.Invoke(money, score, rep);
    }

    private IEnumerator ReduceReputationOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(repReductionInterval);
            AddRep(repReductionAmount);
        }
    }

    private void HandleGameOver()
    {
        PlayerPrefs.SetInt("HighScore", score);
        SceneManager.LoadScene(0);
    }

    #region Setters

    public void AddMoney(int amount)
    {
        money += amount;
        onMoneyChanged?.Invoke(money, amount);
    }

    public void AddScore(int amount)
    {
        score += amount;
        onScoreChanged?.Invoke(score, amount);
    }

    public void AddRep(float amount)
    {
        Debug.Log(amount);
        rep += amount;
        onRepChanged?.Invoke(rep, amount);

        if(rep <= 0)
        {
            // Game Over
            HandleGameOver();
        }
    }

    #endregion
}
