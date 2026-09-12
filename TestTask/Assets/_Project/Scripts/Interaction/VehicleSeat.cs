using UnityEngine;

public class VehicleSeat : MonoBehaviour
{
    [Header("Seat")]
    [SerializeField] private Transform driverSeat;
    [SerializeField] private Transform exitPoint;

    [Header("Vehicle")]
    [SerializeField] private Behaviour vehicleController;
    [SerializeField] private GameObject vehicleCamera;
    [SerializeField] private Rigidbody vehicleRigidbody;
    [SerializeField] private WheelCollider[] wheelColliders;

    public Transform DriverSeat => driverSeat;
    public Transform ExitPoint => exitPoint;

    private void Awake()
    {
        SetDriving(false);
    }
    
    private void StopVehicle()
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            if (wheel == null)
                continue;

            wheel.motorTorque = 0f;
            wheel.brakeTorque = 5000f;
        }

        if (vehicleRigidbody != null)
        {
            vehicleRigidbody.linearVelocity = Vector3.zero;
            vehicleRigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void SetDriving(bool isDriving)
    {
        if (isDriving)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                if (wheel != null)
                {
                     wheel.brakeTorque = 0f;
                }
            }
        }

         if (vehicleController != null)
        {
            vehicleController.enabled = isDriving;
        }

        if (vehicleCamera != null)
        {   
            vehicleCamera.SetActive(isDriving);
        }

        if (!isDriving)
        {
            StopVehicle();
        }
    }
}