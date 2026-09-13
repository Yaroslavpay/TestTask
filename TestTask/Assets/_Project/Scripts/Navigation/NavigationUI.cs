using TMPro;
using UnityEngine;

public class NavigationUI : MonoBehaviour
{
    [SerializeField] private RouteController routeController;
    [SerializeField] private Transform distanceOrigin;

    [Header("UI")]
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text distanceText;

    private void Update()
    {
        UpdateNavigationUI();
    }

    private void UpdateNavigationUI()
    {
        if (routeController == null)
            return;

        RoutePoint currentPoint = routeController.CurrentPoint;

        if (currentPoint == null)
        {
            if (routeController.IsCompleted)
            {
                targetText.text = "Route Complete";
                distanceText.text = "";
            }

            return;
        }

        targetText.text = "Target: " + currentPoint.PointName;

        if (distanceOrigin != null)
        {
            float distance = Vector3.Distance(
                distanceOrigin.position,
                currentPoint.transform.position
            );

            distanceText.text = Mathf.RoundToInt(distance) + " m";
        }
    }
}