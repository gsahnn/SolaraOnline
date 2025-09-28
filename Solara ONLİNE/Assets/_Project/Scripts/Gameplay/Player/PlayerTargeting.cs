using UnityEngine;
using System;

public class PlayerTargeting : MonoBehaviour
{
    public Transform currentTarget { get; private set; }

    // Bu event, yeni bir hedef se�ildi�inde veya hedef b�rak�ld���nda tetiklenir.
    public event Action<Transform> OnTargetSelected;

    private Camera mainCamera;
    [SerializeField] private LayerMask targetableLayers; // Inspector'dan Monster katman�n� se�ece�iz.

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Bu fonksiyonu, giri�leri tek bir yerden y�netmek i�in PlayerController �a��racak.
    public void HandleTargetingInput()
    {
        // Fareye t�kland���nda hedef se�meyi dene
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetableLayers))
            {
                SetTarget(hit.transform);
            }
            else
            {
                // Bo�lu�a t�klad�ysak, hedefi kald�r.
                ClearTarget();
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (currentTarget != newTarget)
        {
            currentTarget = newTarget;
            OnTargetSelected?.Invoke(currentTarget); // Yeni hedefi UI'a ve di�er sistemlere duyur.
        }
    }

    public void ClearTarget()
    {
        if (currentTarget != null)
        {
            currentTarget = null;
            OnTargetSelected?.Invoke(null); // Hedefin kalkt���n� duyur.
        }
    }
}