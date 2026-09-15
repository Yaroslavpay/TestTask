using UnityEngine;

public class RoutePoint : MonoBehaviour
{
    [SerializeField] private string pointName;
    [SerializeField] private GameObject marker;

    private RouteController routeController;

    public string PointName => pointName;

    private void Awake()
    {
        routeController = FindFirstObjectByType<RouteController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        RouteParticipant participant =
            other.GetComponentInParent<RouteParticipant>();

        if (participant == null)
        {
            return;
        }

        if (routeController == null)
        {
            return;
        }

        routeController.ReachPoint(this);
    }

    public void SetActive(bool isActive)
    {
        if (marker != null)
        {
            marker.SetActive(isActive);
        }
    }
}