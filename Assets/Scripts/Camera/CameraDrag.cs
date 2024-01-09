using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    public Camera targetCamera;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = targetCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);


        targetCamera.transform.Translate(move, Space.World);
        targetCamera.transform.position = new Vector3(
          Mathf.Clamp(targetCamera.transform.position.x, -162, 140),
          307,
          Mathf.Clamp(targetCamera.transform.position.z, -500, -150));
    }


}