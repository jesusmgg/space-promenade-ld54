using UnityEngine;

public class RtsCamera : MonoBehaviour
{
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        {
            transform.Translate(new Vector3(horizontal, 0f, vertical), Space.World);
        }
    }
}