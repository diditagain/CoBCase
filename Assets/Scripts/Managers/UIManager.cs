using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject loseScreen;

    private void OnEnable()
    {
        GameManager.OnGameStatechanged += ReloadUI;
    }

    private void OnDisable()
    {
        GameManager.OnGameStatechanged -= ReloadUI;
    }

    void ReloadUI(GameState newState)
    {
        loseScreen.SetActive(newState == GameState.LoseState);
    }
}
