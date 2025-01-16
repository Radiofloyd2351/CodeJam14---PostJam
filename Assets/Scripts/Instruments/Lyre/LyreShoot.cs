using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject shootPoint;
    public float arrowSpeed = 10f;

    private Vector2 aimDirection;
    private bool isAiming = false;


    public AudioSource source;
    public InputSystem inputSystem;

    void Update()
    {
        if (AnimationSwitcher.currentMode == Level.Nature && AnimationSwitcher.collectedInstruments.Contains("Lyre"))
        {
            float distance = 10f;
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                distance = Vector2.Distance(transform.position, mousePos);
            }

            if (distance <= 2f && Input.GetMouseButtonDown(0) && !isAiming)
            {
                shootPoint.SetActive(true);
                isAiming = true;
            }

            if (isAiming && Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                aimDirection = (mousePos - shootPoint.transform.position).normalized;

                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                shootPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (isAiming && Input.GetMouseButtonUp(0))
            {
                shootPoint.SetActive(false);
                ShootArrow();
                isAiming = false;
            }
        }

    }
    void ShootArrow() {
        if (RecordingContainer.recordings.ContainsKey("Lyre")) {
            source.clip = RecordingContainer.recordings["Lyre"].internalClip;
            source.Stop();
            source.timeSamples = RecordingContainer.recordings["Lyre"].offset;
            source.Play();
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = -(mousePos - shootPoint.transform.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction * arrowSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

        Collider2D arrowCollider = arrow.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(arrowCollider, playerCollider);

        


    }


}
