using UnityEngine;
using UnityEngine.UI;

public class UIBullet : MonoBehaviour
{
    public Image bulletBar; 
    public Color startColor = Color.white;
    public Color overheatColor = Color.red;
    public Image overheatIcon;

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

        float bulletRatio = 1f - ((float)remainingBullets / bulletCapacity);
        bulletBar.fillAmount = bulletRatio;

        bulletBar.color = Color.Lerp(startColor, overheatColor, bulletRatio);

        ShowUI(remainingBullets < bulletCapacity);

        if (overheatIcon != null)
        {
            overheatIcon.color = Color.Lerp(startColor, overheatColor, bulletRatio);
            overheatIcon.gameObject.SetActive(gun.IsOverheated());
        }
    }

    public void ShowUI(bool show)
    {
        gameObject.SetActive(show);
    }
}
