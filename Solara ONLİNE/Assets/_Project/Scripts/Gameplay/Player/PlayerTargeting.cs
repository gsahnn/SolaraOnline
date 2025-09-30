using UnityEngine;
using System;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance;

    public CharacterStats currentTarget { get; private set; }
    public static event Action<CharacterStats> OnTargetSelected;

    [SerializeField] private LayerMask targetableLayers;

    // --- REVÝZYON 1: Deðiþkeni public yapýp, Inspector'dan atanabilir hale getiriyoruz. ---
    // Bu, 'Camera.main' komutuna olan baðýmlýlýðý azaltýr ve en güvenli yöntemdir.
    [Header("Referanslar")]
    [Tooltip("Eðer boþ býrakýlýrsa, sahnede 'MainCamera' tag'ine sahip kamera aranýr.")]
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }

        // --- REVÝZYON 2: Güvenli Kamera Bulma Zinciri ---
        // 1. Önce Inspector'dan atanmýþ bir kamera var mý diye kontrol et.
        if (mainCamera == null)
        {
            // 2. Eðer yoksa, 'Camera.main' ile bulmayý dene.
            mainCamera = Camera.main;
        }
    }

    // Bu fonksiyon ana oyuncu kontrol script'inin Update'inden çaðrýlmalý.
    public void HandleTargetingInput()
    {
        // --- REVÝZYON 3: Nihai Güvenlik Kontrolü ---
        // Eðer TÜM denemelere raðmen kamera hala bulunamadýysa, hata ver ve iþlemi durdur.
        if (mainCamera == null)
        {
            Debug.LogError("PlayerTargeting için bir kamera bulunamadý! Lütfen Inspector'dan atayýn veya sahnede 'MainCamera' tag'ine sahip bir kamera olduðundan emin olun.");
            return; // Fonksiyonun çökmesini engelle.
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            // Hata artýk burada oluþmamalý.
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