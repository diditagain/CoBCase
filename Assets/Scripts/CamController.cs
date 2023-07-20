using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    private void Start()
    {
        GameManager.OnGameStatechanged += GameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStatechanged -= GameStateChanged;
    }

    private void GameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.ArcherTurn:
                MoveCamera(FindObjectOfType<Archer>().transform.position);
                break;
            case GameState.DragonTurn:
                MoveCamera(FindObjectOfType<Dragon>().transform.position + Vector3.up * 5f);
                break;
            case GameState.Projectile:
                break;
        }
    }
    
    public void MoveCamera(Vector3 target)
    {
        transform.DOMove(target + offset, 0.7f);
    }

    public async UniTask FollowProjectile(Transform target)
    {
        while (GameManager.Instance.State == GameState.Projectile)
        {
            transform.position = target.position + offset;
            await UniTask.Yield(PlayerLoopTiming.PreLateUpdate, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }
}