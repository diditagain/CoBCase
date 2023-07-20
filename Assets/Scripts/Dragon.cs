using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dragon : MonoBehaviour
{
    [SerializeField] GameObject dragonFirePb;
    [SerializeField] int startingHealth = 1;

    Animator anim;
    GameObject dragonFire;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        SetDragonPosition();
        InstantiateFire();
        GameManager.OnGameStatechanged += DragonTurn;
    }

    private void OnDisable()
    {
        GameManager.OnGameStatechanged -= DragonTurn;
    }

    private void SetDragonPosition()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Respawn");
        Vector3 pointA = points[0].transform.position;
        Vector3 pointB = points[1].transform.position;
        transform.position = new Vector3(Random.Range(pointA.x, pointB.x), 0, 0);
    }
    public void DragonTurn(GameState newState)
    {
        if (newState == GameState.DragonTurn && startingHealth > 0)
        {
            anim.Play("ANIM_Dragon-adult-shoot");
        }
    }
    private void Shoot()
    {
        if (startingHealth > 0)
        {
            Vector2 target = FindObjectOfType<Archer>().transform.position;
            dragonFire.SetActive(true);
            dragonFire.transform.position = transform.position + new Vector3(0, 5, 0);
            dragonFire.GetComponent<DragonFire>().ShootAtPlayer(target);
            GameManager.Instance.UpdateGameState(GameState.Projectile);
            GameManager.Instance.camController.FollowProjectile(dragonFire.transform).GetAwaiter();
        }

    }

    public void TakeHit(int damage)
    {
        startingHealth -= damage;
        {
            if (startingHealth <= 0)
            {
                transform.DOMove(transform.position + new Vector3(-1, 1, 0) * 10f, 3).OnComplete(()=> {
                    GameManager.Instance.ResetGame();
                });
            }
        }
    }

    void InstantiateFire()
    {
        dragonFire = Instantiate(dragonFirePb, transform.position, Quaternion.identity);
        dragonFire.SetActive(false);
    }
}
