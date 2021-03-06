using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnvironmentObject", order = 1)]
 
public class EnvironmentObject : ScriptableObject
{
    public List<GameObject> prefab;
    public int maxAmount;
    public bool randomRotation;
    [Range(0.1f,5)]
    public float scaleMultiply = 1;

    [Header("Grouping")]
    public int minGroupAmount;
    public int maxGroupAmount;

    [Header("Spacing")]
    public int minSpaceDistance;
    public int maxSpaceDistance;

}
