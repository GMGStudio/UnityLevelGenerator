using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHelper : MonoBehaviour
{

    Vector3 XAxis;
    Vector3 ZAxis;
    Vector3 leftTop;
    int checkAttemps = 0;

    internal void Initialize(List<Vector3> verticeList)
    {

        Vector3 rightTop = gameObject.transform.TransformPoint(GetTopRight(verticeList));
        Vector3 leftBottom = gameObject.transform.TransformPoint(GetLeftBottom(verticeList));
        leftTop = gameObject.transform.TransformPoint(GetLeftTop(verticeList));

        XAxis = rightTop - leftTop;
        ZAxis = leftBottom - leftTop;
    }

    private Vector3 GetTopRight(List<Vector3> verticeList)
    {
        Vector3 topRight = Vector3.zero;

        foreach (var item in verticeList)
        {
            if (item.x >= topRight.x && item.z >= topRight.z)
            {
                topRight = item;
            }
        }
        return topRight;
    }

    private Vector3 GetLeftBottom(List<Vector3> verticeList)
    {
        Vector3 leftBottom = Vector3.zero;

        foreach (var item in verticeList)
        {
            if (item.x <= leftBottom.x && item.z <= leftBottom.z)
            {
                leftBottom = item;
            }
        }
        return leftBottom;
    }


    private Vector3 GetLeftTop(List<Vector3> verticeList)
    {
        Vector3 leftTop = Vector3.zero;
        
        foreach (var item in verticeList)
        {
            if (item.x <= leftTop.x && item.z >= leftTop.z)
            {
                leftTop = item;
            }
        }
        return leftTop;
    }



    internal Vector3 GetRandomPositionAround(Vector3 position, float minDistance, float maxDistance)
    {
        Vector3 newPosition;
        checkAttemps = 0;
        do
        {
            newPosition = position;
            newPosition.x += Random.Range(0, 1f) > 0.5f ? Random.Range(minDistance, maxDistance) : Random.Range(-minDistance, -maxDistance);
            newPosition.z += Random.Range(0, 1f) > 0.5f ? Random.Range(minDistance, maxDistance) : Random.Range(-minDistance, -maxDistance);

            newPosition.y = GetPositionY(newPosition);
        } while (CollisionDetected(newPosition, minDistance));
        return newPosition;
    }

    private float GetPositionY(Vector3 rayCastPosition)
    {
        float positionY = 0;
        rayCastPosition.y = 400;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(rayCastPosition, Vector3.down, out RaycastHit hitInfo, 500f, mask))
        {
            positionY = rayCastPosition.y - hitInfo.distance;

        }
        return positionY;
    }

    internal Vector3 GetRandomPointOnGround(float minDistance)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = leftTop + XAxis * Random.value + ZAxis * Random.value;
            randomPosition.y = GetPositionY(randomPosition);

        } while (CollisionDetected(randomPosition, minDistance));
        return randomPosition;
    }

    private bool CollisionDetected(Vector3 position, float minDistance)
    {
        checkAttemps++;
        LayerMask mask = LayerMask.GetMask("Object");
        Physics.SyncTransforms();
        bool check = Physics.CheckSphere(position, minDistance, mask);

        if (checkAttemps == 200)
        {
            checkAttemps = 0;
            throw new NoFreePosition("No more space");
        }
        return check;
    }
}
