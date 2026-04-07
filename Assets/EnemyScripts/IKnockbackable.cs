using UnityEngine;

public interface IKnockbackable
{
    void applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f);
}
