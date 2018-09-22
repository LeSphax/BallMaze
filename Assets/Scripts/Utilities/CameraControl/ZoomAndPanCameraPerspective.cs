using UnityEngine;

public class ZoomAndPanCameraPerspective : MonoBehaviour
{

    Vector3 originMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            originMousePosition = GetMouseWorldPosition();
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            transform.position += originMousePosition - mousePosition;
        }
        Zoom();
    }

    private static Vector3 GetMouseWorldPosition()
    {
        RaycastHit hit;
        if (Functions.RaycastMouse(out hit))
            return hit.point;
        Debug.LogError("No object was hit !!!! ");
        return hit.point;
    }

    private static void Zoom()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 mousePosition = GetMouseWorldPosition();

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && origin.z < -1) // back
        {
            Camera.main.transform.position += 0.5f * Vector3.forward;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && origin.z > -30) // back
        {
            Camera.main.transform.position += 0.5f * Vector3.back;
        }
        else
            return;
        Vector3 newMousePosition = GetMouseWorldPosition();
        Camera.main.transform.position += Vector3.right * (mousePosition.x - newMousePosition.x);
        Camera.main.transform.position += Vector3.up * (mousePosition.y - newMousePosition.y);
    }

}
