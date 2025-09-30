using UnityEngine;
using System;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance;

    public CharacterStats currentTarget { get; private set; }
    public static event Action<CharacterStats> OnTargetSelected;

    [SerializeField] private LayerMask targetableLayers;

    // --- REV�ZYON 1: De�i�keni public yap�p, Inspector'dan atanabilir hale getiriyoruz. ---
    // Bu, 'Camera.main' komutuna olan ba��ml�l��� azalt�r ve en g�venli y�ntemdir.
    [Header("Referanslar")]
    [Tooltip("E�er bo� b�rak�l�rsa, sahnede 'MainCamera' tag'ine sahip kamera aran�r.")]
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }

        // --- REV�ZYON 2: G�venli Kamera Bulma Zinciri ---
        // 1. �nce Inspector'dan atanm�� bir kamera var m� diye kontrol et.
        if (mainCamera == null)
        {
            // 2. E�er yoksa, 'Camera.main' ile bulmay� dene.
            mainCamera = Camera.main;
        }
    }

    // Bu fonksiyon ana oyuncu kontrol script'inin Update'inden �a�r�lmal�.
    public void HandleTargetingInput()
    {
        // --- REV�ZYON 3: Nihai G�venlik Kontrol� ---
        // E�er T�M denemelere ra�men kamera hala bulunamad�ysa, hata ver ve i�lemi durdur.
        if (mainCamera == null)
        {
            Debug.LogError("PlayerTargeting i�in bir kamera bulunamad�! L�tfen Inspector'dan atay�n veya sahnede 'MainCamera' tag'ine sahip bir kamera oldu�undan emin olun.");
            return; // Fonksiyonun ��kmesini engelle.
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            // Hata art�k burada olu�mamal�.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetableLayers))
            {
                SetTarget(hit.collider.GetComponentInParent<CharacterStats>());
            }
            else
            {
                SetTarget(null);
            }
        }
    }

    private void SetTarget(CharacterStats newTarget)
    {
        if (currentTarget == newTarget) return;
        currentTarget = newTarget;
        OnTargetSelected?.Invoke(currentTarget);
    }

    public void ClearTarget()
    {
        SetTarget(null);
    }
}