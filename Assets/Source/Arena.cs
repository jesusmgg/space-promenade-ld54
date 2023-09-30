using UnityEngine;

public class Arena : MonoBehaviour
{
    void Update()
    {
        Transform tr = transform;
        Vector3 newScale = tr.localScale;
        newScale.x -= Time.deltaTime;
        tr.localScale = newScale;
    }
}