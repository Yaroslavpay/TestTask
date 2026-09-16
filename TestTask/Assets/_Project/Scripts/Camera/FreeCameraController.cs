using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class FreeCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float fastMoveSpeed = 25f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 0.15f;

    [Header("Gameplay Controls")]
    [SerializeField] private Behaviour[] controlsToDisable;

    [Header("UI")]
    [SerializeField] private GameObject[] uiToHide;
    [SerializeField] private GameObject freeCameraHUD;
    [SerializeField] private GameObject freeCameraStartHint;

    [Header("Free Camera Bounds")]
    [SerializeField] private BoxCollider[] freeCameraBounds;

    private bool[] previousUIStates;

    private CinemachineBrain cinemachineBrain;

    private bool isFreeCamera;

    private float yaw;
    private float pitch;

    private bool[] previousControlStates;

    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    private void Awake()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        if (freeCameraHUD != null)
        {
            freeCameraHUD.SetActive(false);
        }

        if (freeCameraStartHint != null)
        {
            freeCameraStartHint.SetActive(true);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        bool ctrlPressed =
            Keyboard.current.leftCtrlKey.isPressed ||
            Keyboard.current.rightCtrlKey.isPressed;

        if (ctrlPressed && Keyboard.current.cKey.wasPressedThisFrame)
        {
            ToggleFreeCamera();
        }

        if (!isFreeCamera)
            return;

        HandleMovement();
        HandleLook();
    }

    private bool IsInsideAnyBounds(Vector3 position)
    {
        if (freeCameraBounds == null || freeCameraBounds.Length == 0)
            return true;

        foreach (BoxCollider bounds in freeCameraBounds)
        {
            if (bounds == null)
                continue;

            Vector3 closestPoint = bounds.ClosestPoint(position);

            if ((closestPoint - position).sqrMagnitude < 0.0001f)
            {
                return true;
            }
        }

        return false;
    }

    private void ToggleFreeCamera()
    {
        if (isFreeCamera)
        {
            DisableFreeCamera();
        }
        else
        {
            EnableFreeCamera();
        }
    }

    private void EnableFreeCamera()
    {
        isFreeCamera = true;

        // Запоминаем текущее направление камеры
        Vector3 currentRotation = transform.eulerAngles;

        yaw = currentRotation.y;
        pitch = currentRotation.x;

        // Запоминаем состояние курсора
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DisableGameplayControls();
        
        // Запоминаем состояние игрового UI и скрываем его
        if (uiToHide != null)
        {
            previousUIStates = new bool[uiToHide.Length];

            for (int i = 0; i < uiToHide.Length; i++)
            {
                if (uiToHide[i] == null)
                    continue;

                previousUIStates[i] = uiToHide[i].activeSelf;
                uiToHide[i].SetActive(false);
            }
        }

        // Стартовая подсказка больше не нужна
        if (freeCameraStartHint != null)
        {
            freeCameraStartHint.SetActive(false);
        }

        // Показываем управление Free Camera
        if (freeCameraHUD != null)
        {
            freeCameraHUD.SetActive(true);
        }

        // Теперь Cinemachine больше не управляет MainCamera
        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = false;
        }
    }

    private void DisableFreeCamera()
    {
        isFreeCamera = false;

        RestoreGameplayControls();

        // Возвращаем UI ровно в то состояние,
        // в котором он был до Free Camera
        if (uiToHide != null && previousUIStates != null)
        {
            for (int i = 0; i < uiToHide.Length; i++)
            {
                if (uiToHide[i] == null)
                    continue;

                uiToHide[i].SetActive(previousUIStates[i]);
            }
        }

        if (freeCameraHUD != null)
        {
            freeCameraHUD.SetActive(false);
        }

        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = true;
        }

        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisible;
    }

    private void HandleMovement()
    {
        Vector3 movement = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            movement += transform.forward;

        if (Keyboard.current.sKey.isPressed)
            movement -= transform.forward;

        if (Keyboard.current.dKey.isPressed)
            movement += transform.right;

        if (Keyboard.current.aKey.isPressed)
            movement -= transform.right;

        if (Keyboard.current.eKey.isPressed)
            movement += Vector3.up;

        if (Keyboard.current.qKey.isPressed)
            movement -= Vector3.up;

        float currentSpeed = moveSpeed;

        if (Keyboard.current.leftShiftKey.isPressed)
            currentSpeed = fastMoveSpeed;

        Vector3 targetPosition = transform.position + movement.normalized * currentSpeed * Time.unscaledDeltaTime;

        // Перемещаем камеру только если новая позиция
        // находится хотя бы в одной разрешённой зоне
        if (IsInsideAnyBounds(targetPosition))
        {
            transform.position = targetPosition;
        }
    }

    private void HandleLook()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * lookSensitivity;
        pitch -= mouseDelta.y * lookSensitivity;

        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void DisableGameplayControls()
    {
        if (controlsToDisable == null)
            return;

        previousControlStates = new bool[controlsToDisable.Length];

        for (int i = 0; i < controlsToDisable.Length; i++)
        {
            if (controlsToDisable[i] == null)
                continue;

            previousControlStates[i] =
                controlsToDisable[i].enabled;

            controlsToDisable[i].enabled = false;
        }
    }

    private void RestoreGameplayControls()
    {
        if (controlsToDisable == null ||
            previousControlStates == null)
            return;

        for (int i = 0; i < controlsToDisable.Length; i++)
        {
            if (controlsToDisable[i] == null)
                continue;

            controlsToDisable[i].enabled =
                previousControlStates[i];
        }
    }
}