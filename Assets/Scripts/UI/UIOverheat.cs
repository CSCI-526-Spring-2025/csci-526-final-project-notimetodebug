using UnityEngine;
using UnityEngine.UI;

public class UIOverheat : MonoBehaviour
{
    public Image bulletBar; 
    private Gun currentGun;

    public void SetGun(Gun gun)
    {
        currentGun = gun;
        UpdateBulletUI();
    }

    private void Update()
    {
        if (currentGun != null)
        {
            UpdateBulletUI();
        }
    }

    public void UpdateBulletUI()
    {
        if (currentGun == null) return;

        int remainingBullets = currentGun.GetRemainingBullets();
        int bulletCapacity = currentGun.GetBulletCapacity();

        if (bulletCapacity <= 0) return; 

        float bulletRatio = (float)remainingBullets / bulletCapacity;
        bulletBar.fillAmount = bulletRatio;
        ShowUI(remainingBullets < bulletCapacity);
    }

    public void ShowUI(bool show)
    {
        gameObject.SetActive(show);
    }
}
