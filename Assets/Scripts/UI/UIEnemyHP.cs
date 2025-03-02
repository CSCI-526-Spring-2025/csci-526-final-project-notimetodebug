using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHP : MonoBehaviour
{
    public Slider healthSlider; 
    private Transform enemyTransform; 

    public void Setup(Enemy enemy)
    {
        enemyTransform = enemy.transform;
        healthSlider.maxValue = enemy.maxHP;
        healthSlider.value = enemy.HP;
    }

    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    void LateUpdate()
    {
        if (enemyTransform != null)
        {
            transform.position = enemyTransform.position + new Vector3(0, 2, 0);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
