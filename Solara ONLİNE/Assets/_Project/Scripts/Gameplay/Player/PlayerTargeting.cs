using UnityEngine;
using System;
using System.Linq;

public class PlayerTargeting : MonoBehaviour
{
    public Transform currentTarget { get; private set; }
    public event Action<Transform> OnTargetSelected;

    [Header("Hedefleme Ayarlar�")]
    [SerializeField] private LayerMask targetableLayers;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void HandleTargetingInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // E�er fare bir UI eleman�n�n �zerindeyse, hedefleme yapma.
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetableLayers))
            {
                SetTarget(hit.transform);
            }
            else
            {
                ClearTarget();
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (currentTarget != newTarget)
        {
            currentTarget = newTarget;
            OnTargetSelected?.Invoke(currentTarget);
        }
    }

    public void ClearTarget()
    {
        if (currentTarget != null)
        {
            currentTarget = null;
            OnTargetSelected?.Invoke(null);
        }
    }
}