using UnityEngine;
using System;

public class PlayerTargeting : MonoBehaviour
{
    public Transform currentTarget { get; private set; }

    // Bu event, yeni bir hedef seçildiðinde veya hedef býrakýldýðýnda tetiklenir.
    public event Action<Transform> OnTargetSelected;

    private Camera mainCamera;
    [SerializeField] private LayerMask targetableLayers; // Inspector'dan Monster katmanýný seçeceðiz.

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Bu fonksiyonu, giriþleri tek bir yerden yönetmek için PlayerController çaðýracak.
    public void HandleTargetingInput()
    {
        // Fareye týklandýðýnda hedef seçmeyi dene
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetableLayers))
            {
                SetTarget(hit.transform);
            }
            else
            {
                // Boþluða týkladýysak, hedefi kaldýr.
                ClearTarget();
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (currentTarget != newTarget)
        {
            currentTarget = newTarget;
            OnTargetSelected?.Invoke(currentTarget); // Yeni hedefi UI'a ve diðer sistemlere duyur.
        }
    }

    public void ClearTarget()
    {
        if (currentTarget != null)
        {
            currentTarget = null;
            OnTargetSelected?.Invoke(null); // Hedefin kalktýðýný duyur.
        }
    }
}