using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrackingBullet : Bullet
{
    [SerializeField] private float angularVelocity;
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private float startDetectionTime = 1f;

    private float initTime;
    private float currentAnglularVelocity = 0;
    private bool isTracking = false;

    private void FixedUpdate()
    {
        if (currentAnglularVelocity != 0)
        {
            float velocityAngleChange = currentAnglularVelocity * Time.fixedDeltaTime;
            rb.velocity = Quaternion.AngleAxis(velocityAngleChange, Vector3.forward) * rb.velocity;
        }
    }

    protected override void Start()
    {
        base.Start();
        initTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();

        if (!isTracking && Time.time - initTime >= startDetectionTime)
        {
            isTracking = true;
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (isTracking)
        {
            Vector3 target = DetectTarget();
            if (target != Vector3.zero)
            {
                RotateTowardsTarget(target);
            }
            else
            {
                currentAnglularVelocity = 0;
            }
        }
    }

    private Vector3 DetectTarget()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Player", "Enemy"));
        Physics2D.OverlapCircle(transform.position, detectionRadius, contactFilter, colliders);

        if (colliders.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 target = colliders[0].transform.position;
        float minDistance = float.MaxValue;
        foreach (Collider2D detectedCollider in colliders)
        {
            float distance = Vector2.Distance(transform.position, detectedCollider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = detectedCollider.transform.position;
            }
        }

        return target;
    }

    private void RotateTowardsTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Vector3 cross = Vector3.Cross(rb.velocity, direction);
        float angle = Vector3.Angle(rb.velocity, direction);
        if (cross.z > 0)
        {
            currentAnglularVelocity = angularVelocity;
        }
        else if (cross.z < 0)
        {
            currentAnglularVelocity = -angularVelocity;
        }
        else if (angle != 0)
        {
            currentAnglularVelocity = angularVelocity;
        }
        else
        {
            currentAnglularVelocity = 0;
        }
    }
}