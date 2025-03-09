using UnityEngine;
using UnityEngine.UI;

public class UIBullet : MonoBehaviour
{
    public Image bulletBar; 

    public void UpdateBulletUI(Gun gun)
    {
        if (gun == null)
        {
            return;
        }

        int remainingBullets = gun.GetRemainingBullets();
        int bulletCapacity = gun.GetBulletCapacity();

        if (bulletCapacity <= 0)
        {
            return;
        }

        float bulletRatio = (float)remainingBullets / bulletCapacity;
        bulletBar.fillAmount = bulletRatio;
        ShowUI(remainingBullets < bulletCapacity);
    }

    public void ShowUI(bool show)
    {
        gameObject.SetActive(show);
    }
}
