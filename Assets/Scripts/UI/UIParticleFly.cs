using UnityEngine;

public class UIParticleFly : MonoBehaviour
{
    public float speed = 5f;
    public float delayBeforeFly = 0.5f;
    public Transform uiTarget;

    private float timer = 0f;
    private bool isFlying = false;
    private Camera mainCamera;
    private Vector3 fixedWorldTarget;
    private bool reachedTarget = false;
    private float holdTimer = 0f; 

    void Start()
    {
        mainCamera = Camera.main;


    }

    void Update()
    {

        if (uiTarget != null)
        {
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiTarget.position);
            fixedWorldTarget = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f)); 
        }
        timer += Time.deltaTime;

        if (!isFlying && timer >= delayBeforeFly)
        {
            isFlying = true;
            UIScore uiScore = FindObjectOfType<UIScore>();
            if (uiScore != null)
            {
                uiScore.AnimateScoreIncrease();
            }
        }

        if (isFlying)
        {
            if (!reachedTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, fixedWorldTarget, speed * Time.deltaTime);

                float distance = Vector3.Distance(transform.position, fixedWorldTarget);
                transform.localScale = Vector3.one * Mathf.Clamp01(distance / 2f);

                if (distance < 0.05f)
                {
                    reachedTarget = true;
                }
            }
            else
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= 0.5f)
                {
                    UIScore uiScore = FindObjectOfType<UIScore>();
                    if (uiScore != null && uiScore.shouldUpdateUIAfterAnimation)
                    {
                        uiScore.FinishAnimationAndRestore();
                    }
                    
                    Destroy(gameObject);
                }
            }
        }
    }
}
