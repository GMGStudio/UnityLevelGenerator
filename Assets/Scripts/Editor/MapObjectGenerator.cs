using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PositionHelper))]
public class MapObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private List<EnvironmentObject> environmentObjects;

    private PositionHelper positionHelper;


    internal void Initialize()
    {
        if (positionHelper == null)
        {
            positionHelper = gameObject.GetComponent<PositionHelper>();
        }
        positionHelper.Initialize(new List<Vector3>(gameObject.GetComponent<MeshFilter>().sharedMesh.vertices));
    }

    internal void GenerateMap()
    {
        ClearConsole();
        foreach (var environmentObject in environmentObjects)
        {
            SpawnObjects(environmentObject);
        }

        Debug.Log(CountDecendants(transform) + " Objects generated");
    }


    internal void DeleteObjects()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void SpawnObjects(EnvironmentObject environmentObject)
    {
        for (int i = 0; i < environmentObject.maxAmount; i++)
        {
            GameObject prefab = GetRandomObject(environmentObject.prefab);
            GameObject spawned;
            try { 
            spawned = SpawnObject(prefab,
                      positionHelper.GetRandomPointOnGround(environmentObject.minSpaceDistance),
                      environmentObject.randomRotation,
                      gameObject.transform);
            if (environmentObject.maxGroupAmount > 0)
            {
                int groupAmount = Random.Range(environmentObject.minGroupAmount, environmentObject.maxGroupAmount);

                for (int y = 0; y < groupAmount; y++)
                {
                    GameObject childSpawn = SpawnObject(GetRandomObject(environmentObject.prefab),
                                                        positionHelper.GetRandomPositionAround(spawned.transform.position,
                                                                                               environmentObject.minSpaceDistance,
                                                                                               environmentObject.maxSpaceDistance),
                                                        environmentObject.randomRotation,
                                                        spawned.transform);
                    i++;

                    DestroyIfNotOnGround(childSpawn);
                    if (i == environmentObject.maxAmount)
                        return;

                }
            }
            }
            catch (NoFreePosition noFree)
            {
                Debug.Log(noFree.Message);
            }

        }
    }

    private static void DestroyIfNotOnGround(GameObject childSpawn)
    {
        LayerMask mask = LayerMask.GetMask("Ground");
        Vector3 rayCastPosition = childSpawn.transform.position;
        rayCastPosition.y = 0.1f;
        if (!Physics.Raycast(rayCastPosition, Vector3.down, 4f, mask))
        {
            DestroyImmediate(childSpawn);
        }
    }

    private GameObject SpawnObject(GameObject prefab, Vector3 position, bool rotation, Transform parent)
    {
        GameObject spawned = Instantiate(prefab,
                    position,
                    prefab.transform.localRotation,
                    parent);

        spawned.transform.localScale = Vector3.one;
        if (rotation)
        {
            RandomRotateObject(spawned);
        }
        return spawned;

    }

    private void RandomRotateObject(GameObject spawned)
    {
        spawned.transform.eulerAngles = new Vector3(spawned.transform.rotation.eulerAngles.x,
                                                    Random.Range(1f, 180f),
                                                    spawned.transform.rotation.eulerAngles.z);
    }

    private GameObject GetRandomObject(List<GameObject> list)
    {
        return list[Random.Range(0, list.Count)];
    }


    public static int CountDecendants(Transform transform)
    {
        int childCount = transform.childCount;
        foreach (Transform child in transform)
        {
            childCount += CountDecendants(child);
        }
        return childCount;
    }

    private void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

}
