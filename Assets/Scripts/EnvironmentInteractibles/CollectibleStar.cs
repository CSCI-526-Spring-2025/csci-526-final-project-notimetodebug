using UnityEngine;

public class CollectibleStar : MonoBehaviour
{
    [Header("Floating Animation")]
    public float bobHeight = 0.5f;
    public float bobSpeed = 3.0f;
    public float rotationSpeed = 10.0f;

    [Header("Points")]
    public int points = 10;

    [Header("Collection Effect")]
    public GameObject sparklesPrefab;

    private Vector3 startPosition;
    private bool isCollected = false;


    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCollected)
        {
            // Bobbing up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected)
        {
            return;
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            isCollected = true;

            //LevelManager.Instance.AddCollectibleScore(points);
            LevelManager.Instance.AddCollectibleScore(points, transform.position);


            TelemetryManager.Log(TelemetryManager.EventName.COLLECTIBLE_PICKED_UP, "Star");

            Destroy(gameObject, 0.1f);
        }
    }
}
