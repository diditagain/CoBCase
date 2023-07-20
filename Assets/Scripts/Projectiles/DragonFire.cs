using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class DragonFire : ProjectileBase
{
    
    public void ShootAtPlayer(Vector3 target)
    {
        var position = target;
        if (Random.Range(0, 1f) < 0.3f)
        {
            position = target - new Vector3(-1, 1, 0) * Random.Range(2f, 6f);
            Debug.LogError(position);
        }
        var distance = Vector3.Distance(position, transform.position);
        transform.DOJump(position, 5f, 1, distance * 0.05f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DOTween.KillAll();
        if (collision.gameObject.CompareTag("Archer"))
        {
            collision.gameObject.GetComponentInParent<Archer>().TakeHit(damagePower);
        }
        OnHit(GameState.ArcherTurn).GetAwaiter();
    }
}
