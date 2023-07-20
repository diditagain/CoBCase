using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPool : MonoBehaviour
{
    [SerializeField] GameObject trajectoryPb;
    [SerializeField] int amountToPool;
    
    List<GameObject> pooledTrajectories;
    private float timeDif = 0.1f;

    private void Start()
    {
        GeneratePool();
    }

    public void GeneratePool()
    {
        pooledTrajectories = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(trajectoryPb);
            tmp.SetActive(false);
            pooledTrajectories.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledTrajectories[i].activeInHierarchy)
            {
                return pooledTrajectories[i];
            }
        }
        return null;
    }

    private Vector2 CalculatePoints(Vector2 position, Vector2 direction, float power, float time)
    {
        Vector2 pointPos = position + (direction * power * time) + 0.5f * Physics2D.gravity * time * time;
        return pointPos;
    }

    public void ActivateTrajectoryLine(Vector2 position, Vector2 direction, float power)
    {
        for (int i = 0; i < pooledTrajectories.Count; i++)
        {
            pooledTrajectories[i].SetActive(true);
            pooledTrajectories[i].transform.position = CalculatePoints(position, direction.normalized, power, i * timeDif);
        }        
    }

    public void DeactivateTrajectoryLine()
    {
        foreach (GameObject point in pooledTrajectories) {
            point.SetActive(false);
        }
    }
}
