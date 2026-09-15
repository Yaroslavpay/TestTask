using UnityEngine;

public class VehicleRecoveryPrompt : MonoBehaviour
{
    [SerializeField] private CarController carController;
    [SerializeField] private GameObject recoveryPrompt;

    private void Update()
    {
        if (carController == null || recoveryPrompt == null)
            return;

        bool shouldShow =
            carController.isActiveAndEnabled &&
            carController.CanRecover;

        recoveryPrompt.SetActive(shouldShow);
    }
}