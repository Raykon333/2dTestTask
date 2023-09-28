using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    PlayerInput playerInput;
    InputActionAsset actions;
    [SerializeField] float Speed;

    [SerializeField] TargetingSystem targetingSystem;

    [SerializeField] float Damage;
    [SerializeField] float ShootCooldown;
    private float shootCooldownTimer = 0;

    [SerializeField] Button ShootButton;
    [SerializeField] Inventory Inventory;

    [SerializeField] public Health Health;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        actions = playerInput.actions;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Health.CurrentHealth <= 0)
        {
            ShootButton.interactable = false;
            transform.parent.gameObject.SetActive(false);
            return;
        }

        if (shootCooldownTimer > 0)
            shootCooldownTimer -= Time.fixedDeltaTime;

        var moveVector = actions["Move"].ReadValue<Vector2>();
        transform.position += (Vector3)moveVector * Time.fixedDeltaTime * Speed;

        if (targetingSystem.HasTarget)
        {
            LookAt((Vector2)targetingSystem.TargetedObject.transform.position);
            ShootButton.interactable = true;
        }
        else
        {
            ShootButton.interactable = false;
            if (moveVector.magnitude != 0)
                LookAt(moveVector + (Vector2)transform.position);
        }
    }


    public void LookAt(Vector3 position)
    {
        transform.right = position - transform.position;
    }

    public void Shoot()
    {
        if (!targetingSystem.HasTarget || shootCooldownTimer > 0 || Inventory.GetItemAmount("bullet") <= 0)
            return;
        var enemyHealth = targetingSystem.TargetedObject.GetComponentInChildren<Health>();
        enemyHealth.TakeDamage(Damage);

        Inventory.RemoveItem("bullet", 1);
        shootCooldownTimer = ShootCooldown;
    }
}
