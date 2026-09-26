using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private Label moneyLabel;
    private Label scoreLabel;
    private ProgressBar repProgressBar;

    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;

            if (root != null)
            {
                // Get the TopRow VisualElement
                var topRow = root.Q<VisualElement>("TopRow");
                if (topRow != null)
                {
                    moneyLabel = topRow.Q<Label>("MoneyLabel");
                    scoreLabel = topRow.Q<Label>("ScoreLabel");
                    repProgressBar = topRow.Q<ProgressBar>("RepProgressBar");
                }
                else
                {
                    Debug.LogError("TopRow visual element is null!");
                }
            }
            else
            {
                Debug.LogError("Root visual element is null!");
            }
        }
        else
        {
            Debug.LogError("UIDocument is missing or not found!");
        }

    }

    private void UpdateLabelValue(Label labelToUpdate, string value)
    {
        if (labelToUpdate != null)
        {
            labelToUpdate.text = value;
        }
    }

    public void UpdateMoneyLabel(string value)
    {
        UpdateLabelValue(moneyLabel, value);
    }

    public void UpdateScoreLabel(string value)
    {
        UpdateLabelValue(scoreLabel, value);
    }

    public void UpdateRepProgressBar(float value)
    {
        if (repProgressBar != null)
        {
            repProgressBar.value = value;
        }
    }

    private void OnDisable()
    {
        moneyLabel = null;
        scoreLabel = null;
        repProgressBar = null;
    }
}