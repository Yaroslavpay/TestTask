using UnityEngine;

public class RouteMarkerAnimation : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.4f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 startPosition;

    private void OnEnable()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.localPosition = startPosition + Vector3.up * offset;
    }
}