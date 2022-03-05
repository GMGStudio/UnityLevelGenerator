using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHelper : MonoBehaviour
{

    Vector3 XAxis;
    Vector3 ZAxis;
    Vector3 leftTop;

    int checkAttemps = 0;
    int GroundcheckAttemps = 0;

    internal void Initialize(List<Vector3> VerticeList)
    {
        leftTop = gameObject.transform.TransformPoint(VerticeList[0]);
        Vector3 rightTop = gameObject.transform.TransformPoint(VerticeList[10]);
        Vector3 leftBottom = gameObject.transform.TransformPoint(VerticeList[110]);
        Vector3 rightBottom = gameObject.transform.TransformPoint(VerticeList[120]);
        XAxis = rightTop - leftTop;
        ZAxis = leftBottom - leftTop;
    }

    internal Vector3 GetRandomPositionAround(Vector3 position, float minDistance, float maxDistance)
    {
        Vector3 newPosition;
        GroundcheckAttemps = 0;

        checkAttemps = 0;
        do
        {
            newPosition = position;
            newPosition.x += Random.Range(0, 1f) > 0.5f ? Random.Range(minDistance, maxDistance) : Random.Range(-minDistance, -maxDistance);
            newPosition.y = 0;
            newPosition.z += Random.Range(0, 1f) > 0.5f ? Random.Range(minDistance, maxDistance) : Random.Range(-minDistance, -maxDistance);
        } while (CollisionDetected(newPosition, minDistance));
        return newPosition;
    }

    internal Vector3 GetRandomPointOnGround(float minDistance)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = leftTop + XAxis * Random.value + ZAxis * Random.value;
            randomPosition.y = 0;

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
            return false;
        }
        return check;
    }

    private bool CheckGround(Vector3 position)
    {
        GroundcheckAttemps++;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (GroundcheckAttemps == 10)
        {
            Debug.Log("Many attemps");
            GroundcheckAttemps = 0;
            return false;
        }
        return Physics.CheckSphere(position, 1f, mask);

    }
}
