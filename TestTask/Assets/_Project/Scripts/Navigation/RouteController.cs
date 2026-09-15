using UnityEngine;

public class RouteController : MonoBehaviour
{
    [SerializeField] private RoutePoint[] routePoints;

    private int currentPointIndex;

    public bool IsCompleted => routePoints != null && routePoints.Length > 0 && currentPointIndex >= routePoints.Length;

    public RoutePoint CurrentPoint
    {
        get
        {
            if (routePoints == null || routePoints.Length == 0)
            {
                return null;
            }

            if (currentPointIndex >= routePoints.Length)
            {
                return null;
            }

            return routePoints[currentPointIndex];
        }
    }

    private void Start()
    {
        currentPointIndex = 0;
        currentPointIndex = 0;

        HideAllRouteMarkers();

        if (routePoints != null && routePoints.Length > 0 && routePoints[0] != null)
        {
            routePoints[0].SetActive(true);
        }

        UpdateRoutePoints();
    }

    private void HideAllRouteMarkers()
    {
        RoutePoint[] allRoutePoints = FindObjectsByType<RoutePoint>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (RoutePoint point in allRoutePoints)
        {
            point.SetActive(false);
        }
    }       

    public void ReachPoint(RoutePoint point)
    {
        if (CurrentPoint == null)
        {
            return;
        }

        if (point != CurrentPoint)
        {
            return;
        }

        currentPointIndex++;

        if (currentPointIndex >= routePoints.Length)
        {
            Debug.Log("Route completed!");
        }
        else
        {
            Debug.Log("Next target: " + CurrentPoint.PointName);
        }

        UpdateRoutePoints();
    }

    private void UpdateRoutePoints()
    {
        if (routePoints == null)
        {
            return;
        }

        for (int i = 0; i < routePoints.Length; i++)
        {
            if (routePoints[i] == null)
            {
                continue;
            }

            bool isCurrentPoint = i == currentPointIndex;

            routePoints[i].SetActive(isCurrentPoint);
        }
    }
    
}