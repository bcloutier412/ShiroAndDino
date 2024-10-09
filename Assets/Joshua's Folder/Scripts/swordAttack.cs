using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;

    public float knockbackForce = 500f;

    public float damage = 1;

    Vector2 rightAttackOffset;

    void Start()
{
    rightAttackOffset = transform.localPosition; // Use localPosition relative to the parent (player)
}

public void AttackRight()
{
    swordCollider.enabled = true;
    transform.localPosition = rightAttackOffset;  // Adjust relative to player
}

public void AttackLeft()
{
    swordCollider.enabled = true;
    transform.localPosition = new Vector2(rightAttackOffset.x * -1, rightAttackOffset.y);  // Flip for left attack
}


    public void StopAttack()
    {
        swordCollider.enabled = false;
       // print ("turn off");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Idamageable damageableObject = collider.gameObject.GetComponent<Idamageable>();

    if (damageableObject != null)
    {
        Vector3 parentPosition = transform.parent.position;
        Vector2 direction = (Vector2)(collider.gameObject.transform.position - parentPosition).normalized;
        Vector2 knockback = direction * knockbackForce;
        damageableObject.OnHit(damage, knockback);
        print("attack");
    }
    else
    {
        Debug.LogWarning("Collider does not support Idamageable");
    }

    }

    void OnCollisionEnter2D(Collision2D other) {
        print("test");
    }
}
