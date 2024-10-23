using UnityEngine;

public interface Idamageable
{
    float Health { set; get; }
    bool Targetable { set; get; }

    void OnHit(float damage, Vector2 knockback);

    void OnHit(float damage);

    void DestroySelf();
}
