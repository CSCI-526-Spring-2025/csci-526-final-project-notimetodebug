using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector3 fireDirection;

    public Gun gun;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        fireDirection = (mousePos - transform.position).normalized;
        gun.SetDirection(fireDirection);
        // Debug.DrawLine(transform.position, transform.position + shootDirection, Color.red);

        if (gun is null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || gun.isFullAuto && Input.GetMouseButton(0))
        {
            Vector3 recoilForce = gun.Fire(fireDirection);
            rb.AddForce(recoilForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gun.Reload();
        }
    }
}