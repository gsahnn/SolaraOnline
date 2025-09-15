using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // Inspector'dan NPC'nin kafas�n�n �st�ne koyaca��m�z 'Interaction_Canvas' prefab'�n� buraya s�r�kleyece�iz.
    [SerializeField] private GameObject interactionPromptPrefab;

    // Bu objeyle etkile�ime girildi�inde ne olaca��n� Inspector'dan ayarlayaca��z.
    public UnityEvent OnInteract;

    private GameObject promptInstance; // Olu�turulan prompt objesinin referans�

    // Oyuncu bu objeyi etkile�im i�in "se�ti�inde" �a�r�lacak.
    public void ShowPrompt()
    {
        if (promptInstance == null)
        {
            // NPC'nin pozisyonunun biraz �zerinde prompt'u olu�tur.
            promptInstance = Instantiate(interactionPromptPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            // Prompt'un s�rekli kameraya bakmas�n� sa�la (opsiyonel ama g�zel bir �zellik)
            promptInstance.AddComponent<FaceCamera>();
        }
        promptInstance.SetActive(true);
    }

    // Oyuncu etkile�im alan�ndan ��kt���nda �a�r�lacak.
    public void HidePrompt()
    {
        if (promptInstance != null)
        {
            promptInstance.SetActive(false);
        }
    }

    // "E" tu�una bas�ld���nda bu fonksiyon �a�r�lacak.
    public void Interact()
    {
        OnInteract.Invoke();
    }
}

// Bu k���k yard�mc� script, World Space Canvas'�n her zaman kameraya d�n�k olmas�n� sa�lar.
// Ayr� bir dosyaya koymaya gerek yok, Interactable.cs'in alt�nda kalabilir.
public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    private void Start() { mainCamera = Camera.main; }
    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}