using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenstions
{
    public static bool RayCastCheck(this Rigidbody2D rigidbody2D, Vector2 dir,LayerMask layerMask)
    {
        if(rigidbody2D.isKinematic) return false;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody2D.position, 0.25f, dir.normalized, .375f, layerMask);
        return hit.collider != null;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = transform.position - other.position;
        return Vector2.Dot(direction.normalized,testDirection) > .0001f;
    }
}

