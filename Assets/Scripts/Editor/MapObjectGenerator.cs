using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class MapObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> rocks = null;
    [SerializeField]
    private List<GameObject> trees = null;
    [SerializeField]
    private int maxTrees;
    [SerializeField]
    private int maxRocks;


    internal void GenerateMap()
    {
        SpawnObjects(trees, maxTrees, true);
        SpawnObjects(rocks, maxRocks);
    }

    internal void DeleteObjects()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void SpawnObjects(List<GameObject> gameObjects, int amount, bool randomRotation = false)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject prefab = GetRandomObject(gameObjects);
            GameObject spawned = Instantiate(prefab,
                                             GetRandomPointOnGround(),
                                             prefab.transform.localRotation,
                                             gameObject.transform);
            spawned.transform.localScale = Vector3.one;
            if (randomRotation)
            {
                spawned.transform.eulerAngles = new Vector3(spawned.transform.rotation.eulerAngles.x,
                                                            Random.Range(1f, 180f),
                                                            spawned.transform.rotation.eulerAngles.z);
            }
        }
    }

    private GameObject GetRandomObject(List<GameObject> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    private Vector3 GetRandomPointOnGround()
    {
        List<Vector3> VerticeList = new List<Vector3>(gameObject.GetComponent<MeshFilter>().sharedMesh.vertices);
        Vector3 leftTop     = gameObject.transform.TransformPoint(VerticeList[0]);
        Vector3 rightTop    = gameObject.transform.TransformPoint(VerticeList[10]);
        Vector3 leftBottom  = gameObject.transform.TransformPoint(VerticeList[110]);
        Vector3 rightBottom = gameObject.transform.TransformPoint(VerticeList[120]);
        Vector3 XAxis = rightTop - leftTop;
        Vector3 ZAxis = leftBottom - leftTop;
        Vector3 position = leftTop + XAxis * Random.value + ZAxis * Random.value;
        position.y = 0;
        return position;
    }



}
