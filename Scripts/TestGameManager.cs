using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    private SimpleRuntimeUI uiUpdater;

    private int money = 0;
    private int score = 0;
    private float rep = 25f;

    void Start()
    {
        // Assuming the SimpleRuntimeUI script is on the same GameObject as this script
        if (!uiUpdater)
        {
            uiUpdater = GetComponent<SimpleRuntimeUI>();
        }

        // Start the update coroutine
        StartCoroutine(PeriodicUpdate());
    }

    IEnumerator PeriodicUpdate()
    {
        while (true)
        {
            money++;
            score++;
            rep += 0.2f;
            if (rep > 99f) rep = 3f;

            uiUpdater.UpdateMoneyLabel(money.ToString());
            uiUpdater.UpdateScoreLabel(score.ToString());
            uiUpdater.UpdateRepProgressBar(rep);

            // Wait for 0.5 seconds before the next update
            yield return new WaitForSeconds(0.1f);
        }
    }
}
