using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject mainCamera;
    public float parallaxSpeedX;
    public float parallaxSpeedY;
    private Vector2 startPosition;
    private Vector2 spriteSize;

    // Start is called before the first frame update
    void Start()
    {
        if(mainCamera == null) mainCamera = GameObject.Find("Main Camera");
        
        startPosition = transform.position;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSize = spriteRenderer.bounds.size;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        float tempX = cameraPosition.x * (1 - parallaxSpeedX);
        float tempY = cameraPosition.y * (1 - parallaxSpeedY);
        float distanceX = cameraPosition.x * parallaxSpeedX;
        float distanceY = cameraPosition.y * parallaxSpeedY;
        transform.position = new Vector3(startPosition.x + distanceX, startPosition.y + distanceY, transform.position.z);

        while (tempX > startPosition.x + spriteSize.x / 2) startPosition.x += spriteSize.x;
        while (tempX < startPosition.x - spriteSize.x / 2) startPosition.x -= spriteSize.x;
        while (tempY > startPosition.y + spriteSize.y / 2) startPosition.y += spriteSize.y;
        while (tempY < startPosition.y - spriteSize.y / 2) startPosition.y -= spriteSize.y;
    }
}