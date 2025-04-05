using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIWeaponIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image outerCircle;
    public Image innerSphere;
    public GameObject descriptionPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    [SerializeField] private Sprite activateStatusSprite;
    [SerializeField] private Sprite inactivateStatusSprite;

    private Gun currentGun;

    private void Start()
    {
        innerSphere.enabled = false;
        descriptionPanel.SetActive(false);
    }

    public void SetGun(Gun gun)
    {
        currentGun = gun;

        Sprite icon = gun.GetGunIcon();
        if (icon != null)
        {
            innerSphere.sprite = icon;
            innerSphere.enabled = true;
        }
    }

    public void UpdateWeaponIndicator(bool isEquipped)
    {
        outerCircle.sprite = isEquipped ? activateStatusSprite : inactivateStatusSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentGun == null) return;
        Player player = FindObjectOfType<Player>();
        if (player == null || player.guns.Count <= 1)
        return;

        nameText.text = currentGun.GetGunName();
        descText.text = currentGun.GetGunDescription();
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}
