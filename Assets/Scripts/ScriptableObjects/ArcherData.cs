using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherData", menuName = "ScriptableObjects/ArcherData", order = 1)]
public class ArcherData : ScriptableObject
{
    public GameObject archerPb;
    public GameObject arrowPb;
    public string archerName;
    public float drawSpeed;  // Speed at which the Archer can draw the bow
    public float releaseSpeed;  // Speed of the released arrow
    public float maxPower;  // Maximum strength of Archer's shot
    public int health;  // Archer's health points
    
}
