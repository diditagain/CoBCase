using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject dragonPb;
    [SerializeField] AssetReference environmentReference;
    [SerializeField] public CamController camController;
    public GameState State;
    public static event Action<GameState> OnGameStatechanged;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadData();
    }


    private void LoadData()
    {
        AsyncOperationHandle<GameObject> handle = environmentReference.LoadAssetAsync<GameObject>();
        handle.Completed += OnDataLoaded;
    }

    private void OnDataLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
            Instantiate(dragonPb);
            Debug.Log($"Environment Loaded");
            UpdateGameState(GameState.ArcherTurn);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.ArcherTurn:
                break;
            case GameState.DragonTurn:
                break;
            case GameState.Projectile:
                break;
            case GameState.LoseState:
                break;
        }

        OnGameStatechanged?.Invoke(newState);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

    public enum GameState { 
    ArcherTurn,
    DragonTurn,
    Projectile,
    LoseState,
}