using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] TargetingSystem targetingSystem;
    [SerializeField] float Speed;
    [SerializeField] float AttackDistance;

    [SerializeField] float AttackTime;
    [SerializeField] float AttackDamage;
    float attackTimer = 0;
    bool isAttacking;

    [SerializeField] public Health Health;
    [SerializeField] PickupItem PickupDropPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Health.CurrentHealth <= 0)
        {
            Instantiate(PickupDropPrefab, transform.position, new Quaternion());
            Destroy(transform.parent.gameObject);
        }

        if (isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer <= 0)
                isAttacking = false;
        }
        if (!isAttacking && targetingSystem.HasTarget)
        {
            LookAt((Vector2)targetingSystem.TargetedObject.transform.position);
            var vectorToPlayer = targetingSystem.TargetedObject.transform.position - transform.position;
            if (vectorToPlayer.magnitude > AttackDistance)
                transform.position += vectorToPlayer.normalized * Time.fixedDeltaTime * Speed;
            else
            {
                isAttacking = true;
                attackTimer = AttackTime;
                targetingSystem.TargetedObject.GetComponentInChildren<Health>().TakeDamage(AttackDamage);
            }
        }
    }

    public void LookAt(Vector3 position)
    {
        transform.right = position - transform.position;
    }
}
