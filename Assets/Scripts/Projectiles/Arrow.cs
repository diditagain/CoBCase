using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ProjectileBase
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Dragon>())
        {
            transform.parent = collision.gameObject.transform;
            collision.gameObject.GetComponent<Dragon>().TakeHit(damagePower);
        }
        OnHit(GameState.DragonTurn).GetAwaiter();
    }
}
