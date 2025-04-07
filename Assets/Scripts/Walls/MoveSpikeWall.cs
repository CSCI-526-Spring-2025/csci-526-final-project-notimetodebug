using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.EnvironmentInteractibles;
public class MoveSpikeWall : Wall
{
    public int Damage = 40;
    public int BounceBackVelocity = 10;

    public List<GameObject> spikeControllersObjects;
    private List<IItemController> spikeControllers;

    public Transform targetTransform;
    private Vector3 startPosition;
    public float moveSpeed = 4f;
    private bool hasMoved = false;
    private bool enableMovement = false;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        enableMovement = targetTransform != null && spikeControllersObjects != null && spikeControllersObjects.Count > 0;
        if (enableMovement){
            spikeControllers = spikeControllersObjects
                .Select(spikeControllersObject => spikeControllersObject?.GetComponent<IItemController>())
                .Where(controller => controller != null)
                .ToList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableMovement || hasMoved){
            return;
        }

        if (AllTriggersActivated()){
            if (!isMoving){
                isMoving = true;
            }
            if (isMoving){
                Vector3 newPos = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
                newPos.z = transform.position.z;
                transform.position = newPos;

                Vector3 currentPos = transform.position;
                Vector3 targetPos = targetTransform.position;
                if (Vector3.Distance(currentPos, targetPos) < 0.05f){
                    transform.position = targetTransform.position;
                    hasMoved = true;
                    isMoving = false;
                }
            }
        }
    }

    private bool AllTriggersActivated(){
        return spikeControllers != null && spikeControllers.All(controller => controller.IsItemActive());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Creature creature))
        {
            Vector2 normal = collision.contacts[0].normal;

            Rigidbody2D creatureRb = creature.GetComponent<Rigidbody2D>();
            creatureRb.velocity += -normal * BounceBackVelocity;

             creature.TakeDamage(Damage, "Spikes");
        }
    }


}