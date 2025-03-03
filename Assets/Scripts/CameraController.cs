using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement Values")]
    public float cameraMoveSpeed = 5f; //Test
    private Vector3 middleMouseDragOrigin;
    private Vector3 locationDifference;
    private Vector3 currentMousePosition => camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    bool isDragging;


    [Header("Zoom Values")]
    public float zoomSpeed = 2f; //Test
    public float minZoom = 2f; //Test
    public float maxZoom = 10f; //Test

    [Header("Bound Values")]
    private float minX, maxX, minY, maxY;

    private Camera camera;

   

    void Start()
    {
        camera = Camera.main;


        //Get from GRIDMANAGER
        GridManager gridManager = Object.FindFirstObjectByType<GridManager>();
        Vector2 bounds = gridManager.GetWorldBounds();

        float halfHeight = camera.orthographicSize;
        float halfWidth = halfHeight * camera.aspect;

        minX = -halfWidth;
        maxX = bounds.x + halfWidth;

        minY = -halfHeight;
        maxY = bounds.y + halfHeight;
    }


    //-----------------------------------MOVEMENT -----------------------------------------------
    private void CameraMovementWASD()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY, 0f);

        transform.position += move * cameraMoveSpeed * Time.deltaTime;

        ClampCamera();

    }

    public void CameraMovementDrag(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            middleMouseDragOrigin = (currentMousePosition);
            isDragging = context.started || context.performed;
        }
        else if (context.canceled)
        {
            isDragging = false;
        }
    }


    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

        CameraZoomBound();
    }

    //-----------------------BOUNDARIES------------------------------------

    private void CameraZoomBound()
    {
        float halfHeight = camera.orthographicSize;
        float halfWidth = halfHeight * camera.aspect;

        minX = -halfWidth;
        maxX = Object.FindFirstObjectByType<GridManager>().GetWorldBounds().x + halfWidth;

        minY = -halfHeight;
        maxY = Object.FindFirstObjectByType<GridManager>().GetWorldBounds().y + halfHeight;
    }

    private void ClampCamera()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = position;
    }


    // -------------------------------------------------------------------------------------------------------

    void LateUpdate()
    {
        CameraMovementWASD();
        CameraZoom();
        if (!isDragging) { return; }

        locationDifference = currentMousePosition - transform.position;
        transform.position = middleMouseDragOrigin - locationDifference;

        ClampCamera();

    }




}
