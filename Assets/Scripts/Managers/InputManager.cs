using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public event Action<Vector2> onDrag;
    public event Action onMouseButtonDown;
    public event Action onMouseButtonUp;
    Vector2 value;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (GameManager.Instance.State == GameState.ArcherTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                onMouseButtonDown?.Invoke();
            }
            if (Input.GetMouseButton(0))
            {
                value.x = Input.GetAxis("Mouse X");
                value.y = Input.GetAxis("Mouse Y");
                onDrag?.Invoke(value);
            }
            if (Input.GetMouseButtonUp(0))
            {
                onMouseButtonUp?.Invoke();
            }
        }

    }


}
