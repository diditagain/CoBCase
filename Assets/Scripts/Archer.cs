using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Archer : MonoBehaviour
{
    [SerializeField] private AssetReference archerDataReference;
    [SerializeField] GameObject trajectoryPb;
    [SerializeField] GameObject healthIndicator;

    TrajectoryPool trajectoryPool;

    GameObject archerPb;
    GameObject[] healthBar;
    GameObject arrow;
    Transform archerBody;
    private int health;
    private float maxPower;
    private Transform arrowPoint, arrowButt;
    private void Start()
    {
        LoadData();
        trajectoryPool = GetComponent<TrajectoryPool>();
    }

    private void OnEnable()
    {
        InputManager.instance.onDrag += SetPower;
        InputManager.instance.onDrag += MoveBody;
        InputManager.instance.onDrag += StartTrajectory;
        InputManager.instance.onMouseButtonUp += ShootArrow;
        InputManager.instance.onMouseButtonUp += StopTrajectory;
    }

    private void OnDisable()
    {
        InputManager.instance.onDrag -= SetPower;
        InputManager.instance.onDrag -= MoveBody;
        InputManager.instance.onDrag -= StartTrajectory;
        InputManager.instance.onMouseButtonUp -= ShootArrow;
        InputManager.instance.onMouseButtonUp -= StopTrajectory;
    }

    void LoadData()
    {
        AsyncOperationHandle<ArcherData> handle = archerDataReference.LoadAssetAsync<ArcherData>();
        handle.Completed += OnDataLoaded;
    }

    void OnDataLoaded(AsyncOperationHandle<ArcherData> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            ArcherData archerData = handle.Result;
            SetArcher(archerData);
            Debug.Log($"Loaded archer: {archerData.archerName}");
        }
        else
        {
            Debug.LogError("Failed to load ArcherData");
        }
    }

    void SetArcher(ArcherData archerData)
    {
        archerPb = archerData.archerPb;
        maxPower = archerData.maxPower;
        GameObject newArcher = Instantiate(archerPb);
        InstantiateArrow(archerData.arrowPb);
        newArcher.transform.SetParent(transform);
        newArcher.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        ArcherBody archerBodyComponent;
        archerBodyComponent = newArcher.GetComponentInChildren<ArcherBody>();
        health = archerData.health;
        archerBody = archerBodyComponent.transform;
        arrowPoint = archerBodyComponent.arrowPoint;
        arrowButt = archerBodyComponent.arrowButt;
        InstantiateUI();
    }

    private float power;
    void SetPower(Vector2 value)
    {
        power -= value.x * 2;
        power = Mathf.Clamp(power, 0f, maxPower);
    }

    void MoveBody(Vector2 value)
    {
        archerBody.transform.localEulerAngles += new Vector3(0, 0, value.y * 5f);
    }

    void StartTrajectory(Vector2 value)
    {
        trajectoryPool.ActivateTrajectoryLine(arrowButt.position, GetDirection(), power);
    }

    void StopTrajectory()
    {
        trajectoryPool.DeactivateTrajectoryLine();
    }

    void ShootArrow()

    { 
        arrow.SetActive(true);
        arrow.transform.position = arrowButt.position;
        arrow.transform.rotation = Quaternion.identity;
        arrow.GetComponent<Arrow>().Shoot(GetDirection(), power);
        GameManager.Instance.UpdateGameState(GameState.Projectile);
        GameManager.Instance.camController.FollowProjectile(arrow.transform).GetAwaiter();
    }

    Vector2 GetDirection()
    {
        return arrowPoint.position - arrowButt.position;
    }

    void InstantiateArrow(GameObject arrowPb)
    {
        arrow = Instantiate(arrowPb, transform.position, Quaternion.identity);
        arrow.SetActive(false);
    }

    public void TakeHit(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            GameManager.Instance.UpdateGameState(GameState.LoseState);
        }
    }

    void InstantiateUI()
    {
        healthBar = new GameObject[health];
        for (int i = 0; i < health; i++)
        {
            var newHealthIndicator = Instantiate(healthIndicator, transform.position + new Vector3(-(health/2f) + i, 3, 0), Quaternion.identity);
            healthBar[i] = newHealthIndicator;
        }
    }

    void UpdateHealthBar()
    {

        foreach (GameObject healthIndicator in healthBar)
        {
            healthIndicator.SetActive(false);
        }
        for (int i = 0; i < health; i++)
        {
            healthBar[i].SetActive(true);
        }
    }
}
