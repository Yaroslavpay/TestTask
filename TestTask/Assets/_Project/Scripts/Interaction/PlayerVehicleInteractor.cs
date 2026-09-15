using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerVehicleInteractor : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Behaviour playerMovementController;
    [SerializeField] private GameObject playerVisuals;
    [SerializeField] private GameObject playerCamera;

    [Header("Interaction UI")]
    [SerializeField] private TMP_Text interactionPromptText;

    private VehicleSeat nearbyVehicle;
    private VehicleSeat activeVehicle;

    private Transform originalParent;

    private void Awake()
    {
        originalParent = transform.parent;

        UpdateInteractionPrompt();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (!Keyboard.current.fKey.wasPressedThisFrame)
        {
            return;
        }

        if (activeVehicle != null)
        {
            ExitVehicle();
        }
        else if (nearbyVehicle != null)
        {
            EnterVehicle(nearbyVehicle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        VehicleSeat vehicle = other.GetComponent<VehicleSeat>();

        if (vehicle != null)
        {
            nearbyVehicle = vehicle;

            UpdateInteractionPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        VehicleSeat vehicle = other.GetComponent<VehicleSeat>();

        if (vehicle == nearbyVehicle && activeVehicle == null)
        {
            nearbyVehicle = null;

            UpdateInteractionPrompt();
        }
    }

    private void EnterVehicle(VehicleSeat vehicle)
    {
        activeVehicle = vehicle;

        playerMovementController.enabled = false;
        characterController.enabled = false;

        transform.SetParent(vehicle.DriverSeat);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        playerVisuals.SetActive(false);

        vehicle.SetDriving(true);

        playerCamera.SetActive(false);

        UpdateInteractionPrompt();

    }

    private void ExitVehicle()
    {
        VehicleSeat vehicle = activeVehicle;

        vehicle.SetDriving(false);

        transform.SetParent(originalParent);

        transform.position = vehicle.ExitPoint.position;
        transform.rotation = vehicle.ExitPoint.rotation;

        playerVisuals.SetActive(true);

        characterController.enabled = true;
        playerMovementController.enabled = true;

        playerCamera.SetActive(true);

        activeVehicle = null;
        nearbyVehicle = null;
        
        UpdateInteractionPrompt();

    }

    private void UpdateInteractionPrompt()
    {
        if (interactionPromptText == null)
            return;

        if (activeVehicle != null)
        {
            interactionPromptText.gameObject.SetActive(true);
            interactionPromptText.text = "[F] Exit Vehicle";
        }
        else if (nearbyVehicle != null)
        {
             interactionPromptText.gameObject.SetActive(true);
            interactionPromptText.text = "[F] Enter Vehicle";
        }
        else
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }
}