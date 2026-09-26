using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [Header("Minimap Settings")]
    [SerializeField] private RectTransform minimapIconParent;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private float iconSizeScale = 1f;
    [SerializeField] private float extraMapSpacing = 10f;
    [SerializeField] private Color colorMultiplier = Color.white;
    
    [Header("Final Stage Settings")]
    [SerializeField] private GameObject finalStageIconPrefab;
    [SerializeField] private float finalStageIconSizeScale = 0.1f;

    [Header("Player Settings")]
    [SerializeField] private float playerIconSizeScale = 1f;
    
    [Header("Dot Settings")]
    [SerializeField] private Transform dotParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSizeScale = 0.03f;

    private Transform player;
    private Transform sunTransform;
    private Transform[] habitableTransforms;
    private Dictionary<Transform, Image> icons = new Dictionary<Transform, Image>();
    private float maxDistanceFromSun;
    private Queue<GameObject> activeDots = new Queue<GameObject>();
    private Queue<GameObject> inactiveDots = new Queue<GameObject>();

    private void Awake()
    {
        InitializeTransforms();
        InitializeMinimapIcons();
    }

    private void Update()
    {
        CalculateMaxDistanceFromSun();
        UpdateMinimapIcons();
        UpdateDotIcons();
    }

    private void InitializeTransforms()
    {
        player = FindObjectOfType<ControllerBase>().transform;
        sunTransform = GameObject.FindWithTag("Sun").transform;
        habitableTransforms = FindObjectsOfType<Habitable>().Select(h => h.transform.parent).ToArray();
    }

    private void InitializeMinimapIcons()
    {
        CreateAndRegisterIcon(player);
        CreateAndRegisterIcon(sunTransform);

        foreach (var habitable in habitableTransforms)
        {
            CreateAndRegisterIcon(habitable);
            Habitable habitableScript = habitable.GetComponentInChildren<Habitable>();
            if (habitableScript != null)
            {
                habitableScript.OnTransitionStage += (Habitable.Stage stage) => HandleHabitableStageTransition(habitableScript, stage);
                HandleHabitableStageTransition(habitableScript, habitableScript.CurrentStage);
            }
        }
    }

    private void CreateAndRegisterIcon(Transform target)
    {
        CreateMinimapIcon(target, out var iconImage);
        icons[target] = iconImage;
    }

    private void UpdateDotIcons()
    {
        ResetDotPool();
        PositionActiveDots();
    }

    private void ResetDotPool()
    {
        while (activeDots.Count > 0)
        {
            GameObject dot = activeDots.Dequeue();
            dot.SetActive(false);
            inactiveDots.Enqueue(dot);
        }
    }

    private void PositionActiveDots()
    {
        foreach (Transform child in dotParent)
        {
            GameObject dot = GetDotFromPool();
            UpdateDotIconPosition(dot, child.position);
            activeDots.Enqueue(dot);
        }
    }

    private GameObject GetDotFromPool()
    {
        if (inactiveDots.Count == 0)
        {
            return CreateNewDot();
        }

        GameObject dot = inactiveDots.Dequeue();
        dot.SetActive(true);
        return dot;
    }

    private GameObject CreateNewDot()
    {
        GameObject dot = Instantiate(dotPrefab, minimapIconParent);
        dot.SetActive(true);
        return dot;
    }

    private void UpdateDotIconPosition(GameObject dot, Vector3 worldPosition)
    {
        float minimapRadius = GetMinimapRadius();
        RectTransform dotRectTransform = dot.GetComponent<RectTransform>();
        dotRectTransform.anchoredPosition = GetPositionOnMinimap(worldPosition, minimapRadius);
        dotRectTransform.localScale = Vector3.one * dotSizeScale;
    }

    private void CreateMinimapIcon(Transform target, out Image iconImage)
    {
        GameObject icon = Instantiate(iconPrefab, minimapIconParent);
        iconImage = icon.GetComponent<Image>();
        SpriteRenderer targetRenderer = target.GetComponent<SpriteRenderer>();
        iconImage.sprite = targetRenderer.sprite;
        iconImage.color = targetRenderer.color * colorMultiplier;
        
        if (target == player)
        {
            iconImage.rectTransform.localScale = Vector3.one * playerIconSizeScale;
        }
        else
        {
            iconImage.rectTransform.localScale = Vector3.one * iconSizeScale;
        }
    }

    private void CalculateMaxDistanceFromSun()
    {
        maxDistanceFromSun = habitableTransforms.Max(h => Vector3.Distance(h.position, sunTransform.position)) + extraMapSpacing;
    }

    private void UpdateMinimapIcons()
    {
        float minimapRadius = GetMinimapRadius();

        foreach (var kvp in icons)
        {
            kvp.Value.rectTransform.anchoredPosition = GetPositionOnMinimap(kvp.Key.position, minimapRadius);
            if (kvp.Key == player)
            {
                RotatePlayerIconToMatchDirection(kvp.Value);
            }
        }
    }

    private float GetMinimapRadius()
    {
        return Mathf.Min(minimapIconParent.rect.width, minimapIconParent.rect.height) * 0.5f;
    }

    private Vector2 GetPositionOnMinimap(Vector3 worldPosition, float minimapRadius)
    {
        Vector3 difference = worldPosition - sunTransform.position;
        Vector2 normalizedPosition = new Vector2(difference.x, difference.y) / maxDistanceFromSun;
        return normalizedPosition * minimapRadius;
    }

    private void RotatePlayerIconToMatchDirection(Image playerIcon)
    {
        playerIcon.rectTransform.SetAsLastSibling();
        playerIcon.rectTransform.eulerAngles = new Vector3(0, 0, player.eulerAngles.z);
    }
    
    private void HandleHabitableStageTransition(Habitable habitable, Habitable.Stage stage)
    {
        if (stage == Habitable.Stage.FinalStage)
        {
            GameObject finalStageIcon = Instantiate(finalStageIconPrefab, icons[habitable.transform.parent].transform);
            finalStageIcon.transform.localScale = Vector3.one * finalStageIconSizeScale;
        }
    }
}
