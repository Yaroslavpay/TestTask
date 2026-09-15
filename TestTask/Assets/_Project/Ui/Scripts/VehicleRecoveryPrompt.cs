using UnityEngine;
using TMPro;

public class VehicleRecoveryPrompt : MonoBehaviour
{
    [SerializeField] private CarController carController;
    [SerializeField] private GameObject recoveryPrompt;
    [SerializeField] private TMP_Text recoveryPromptText;

    private void Update()
    {
        if (carController == null || recoveryPrompt == null || recoveryPromptText == null)
        {
            return;
        }

        if (!carController.isActiveAndEnabled)
        {
            recoveryPrompt.SetActive(false);
            return;
        }

        if (carController.CanRecover)
        {
            recoveryPrompt.SetActive(true);
            recoveryPromptText.text = "[R] Recover Vehicle";
            return;
        }

        if (carController.IsStuck)
        {
            recoveryPrompt.SetActive(true);
            recoveryPromptText.text = "[Hold R] Reset Stuck Vehicle";
            return;
        }

        recoveryPrompt.SetActive(false);
    }
}