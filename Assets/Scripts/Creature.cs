using UnityEngine;

public class Creature : MonoBehaviour, IBulletIteractable
{
    public int HP;
    public int maxHP;
    protected UIPlayerHP healthUI; // UI Reference (only used for Player)

    protected virtual void Start()
    {
        HP = maxHP; 
    }

    public virtual void OnBulletCollision(Bullet bullet)
    {
        TakeDamage(bullet.damage);
        bullet.OnAbsorbed();
    }

    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHP);

        // Update UI only if this is the Player
        if (this is Player && healthUI != null)
        {
            healthUI.UpdateHealth(HP);
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has died!");
    }
}
