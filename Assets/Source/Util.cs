using UnityEngine;

public static class Util
{
    public static Vector3 GetRandomPointInBounds(Collider collider)
    {
        Bounds bounds = collider.bounds;
        Transform transform = collider.transform;

        float minX = bounds.size.x * -0.5f;
        float minZ = bounds.size.z * -0.5f;

        Vector3 randomPoint = GetRandomVector3(minX, -minX, minZ, -minZ);
        return randomPoint;
        // return transform.TransformPoint(randomPoint);
    }

    public static Vector3 GetRandomVector3(float min, float max)
    {
        return GetRandomVector3(min, max, min, max);
    }

    public static Vector3 GetRandomVector3(float minX, float maxX, float minZ, float maxZ)
    {
        return new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
    }
}