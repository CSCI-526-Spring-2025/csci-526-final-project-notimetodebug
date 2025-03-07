using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public Slider scoreSlider;  
    public Image star1;       
    public Image star2;       
    public Image star3;       

    private void Update()
    {
        float scoreValue = scoreSlider.value; 

        star1.enabled = scoreValue >= 42f; 
        star2.enabled = scoreValue >= 71f; 
        star3.enabled = scoreValue >= 98f;  
    }
}
