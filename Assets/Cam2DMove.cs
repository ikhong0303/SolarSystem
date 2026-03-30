using UnityEngine;

public class Cam2DMove : MonoBehaviour
{
    [Header("Camera Move Setting")]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Camera Limit")]
    [SerializeField] private bool useLimit = false;
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;

    private Camera targetCamera;
    private bool isDragging;
    private Vector3 lastPointerPosition;

    private float fixedY;
    private float fixedZ;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    private void Update()
    {
        HandlePointerInput();
    }

    private void HandlePointerInput()
    {
        // 모바일 터치
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                StartDrag(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                MoveCamera(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                EndDrag();
            }

            return;
        }

        // 마우스 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            MoveCamera(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void StartDrag(Vector3 pointerPosition)
    {
        isDragging = true;
        lastPointerPosition = pointerPosition;
    }

    private void MoveCamera(Vector3 currentPointerPosition)
    {
        if (!isDragging) return;

        Vector3 lastWorldPos = targetCamera.ScreenToWorldPoint(lastPointerPosition);
        Vector3 currentWorldPos = targetCamera.ScreenToWorldPoint(currentPointerPosition);

        float deltaX = currentWorldPos.x - lastWorldPos.x;

        Vector3 cameraPosition = transform.position;

        // 드래그 방향 반대로 이동
        cameraPosition.x -= deltaX * moveSpeed;

        cameraPosition.y = fixedY;
        cameraPosition.z = fixedZ;

        if (useLimit)
        {
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
        }

        transform.position = cameraPosition;

        lastPointerPosition = currentPointerPosition;
    }

    private void EndDrag()
    {
        isDragging = false;
    }
}
