using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScrollBackground : MonoBehaviour
{
    private Transform playerTransform;
    private Material backgroundMaterial;
    
    private void Awake()
    {
        playerTransform = FindObjectOfType<ControllerBase>().transform;
        backgroundMaterial = GetComponent<Image>().material;
        
        if (backgroundMaterial == null)
            throw new NullReferenceException("Background material is null");
        if (playerTransform == null)
            throw new NullReferenceException("Player transform is null");
    }

    void Update()
    {
        Vector3 playerPosition = playerTransform.position;
        backgroundMaterial.SetVector("_PlayerPos", playerPosition);
    }
}