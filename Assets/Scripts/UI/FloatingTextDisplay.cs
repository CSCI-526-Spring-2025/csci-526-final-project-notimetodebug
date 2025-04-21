using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingTextDisplay : MonoBehaviour
{
    public GameObject FloatingTextPrefab;

    private IEnumerator DestroyAferSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }

    public void DisplayFloatingText(string text)
    {

        Vector2 position = transform.position;

        GameObject floatingText = Instantiate(FloatingTextPrefab, position, Quaternion.identity, transform);

        GameObject canvas = floatingText.transform.Find("Canvas").gameObject;

        TextMeshProUGUI textMesh = canvas.GetComponentInChildren<TextMeshProUGUI>();

        textMesh.SetText(text);

        StartCoroutine(DestroyAferSeconds(floatingText, 1f));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
