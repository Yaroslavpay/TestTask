using TMPro;
using UnityEngine;

public class NavigationUI : MonoBehaviour
{
    [SerializeField] private RouteController routeController;
    [SerializeField] private Transform distanceOrigin;

    [SerializeField] private GameObject routeCompletePanel;

    [SerializeField] private float completionMessageDuration = 3f;

    private float completionTimer;
    private bool completionShown;

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
                // Убираем обычную информацию о маршруте
                targetText.gameObject.SetActive(false);
                distanceText.gameObject.SetActive(false);

                // Показываем сообщение о завершении только один раз
                if (!completionShown)
                {
                    completionShown = true;
                    completionTimer = completionMessageDuration;

                    if (routeCompletePanel != null)
                    {
                        routeCompletePanel.SetActive(true);
                    }
                }

                // Отсчитываем время
                completionTimer -= Time.deltaTime;

                // Через заданное время скрываем сообщение
                if (completionTimer <= 0f)
                {
                    if (routeCompletePanel != null)
                    {
                        routeCompletePanel.SetActive(false);
                    }
                }
            }

            return;
        }

        // Пока маршрут идёт
        targetText.gameObject.SetActive(true);
        distanceText.gameObject.SetActive(true);

        if (routeCompletePanel != null)
        {
            routeCompletePanel.SetActive(false);
        }

        targetText.text = "Target: " + currentPoint.PointName;

        if (distanceOrigin != null)
        {
            float distance = Vector3.Distance(distanceOrigin.position, currentPoint.transform.position);

            distanceText.text = Mathf.RoundToInt(distance) + " m";
        }
    }
}