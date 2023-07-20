using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ProjectileBase : MonoBehaviour
{


    [SerializeField] protected int damagePower;
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    protected bool hasHit;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!hasHit)
        {
            RotateProjectile();
        }
    }

    public virtual void Shoot(Vector3 direction, float power)
    {
        rb.velocity = direction.normalized * power;
    }

    protected void RotateProjectile()
    {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public virtual void ResetProjectile()
    {
        if (!rb.simulated)
        {
            hasHit = false;
            boxCollider.enabled = true;
            rb.simulated = true;
            transform.parent = null;
        }
    }

    public async UniTask OnHit(GameState newState)
    {
        hasHit = true;
        boxCollider.enabled = false;
        rb.simulated = false;
        await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken:this.GetCancellationTokenOnDestroy());
        if (GameManager.Instance.State == GameState.Projectile) GameManager.Instance.UpdateGameState(newState);
        gameObject.SetActive(false);
        ResetProjectile();
    }

}
