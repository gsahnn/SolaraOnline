using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // Inspector'dan NPC'nin kafasýnýn üstüne koyacaðýmýz 'Interaction_Canvas' prefab'ýný buraya sürükleyeceðiz.
    [SerializeField] private GameObject interactionPromptPrefab;

    // Bu objeyle etkileþime girildiðinde ne olacaðýný Inspector'dan ayarlayacaðýz.
    public UnityEvent OnInteract;

    private GameObject promptInstance; // Oluþturulan prompt objesinin referansý

    // Oyuncu bu objeyi etkileþim için "seçtiðinde" çaðrýlacak.
    public void ShowPrompt()
    {
        if (promptInstance == null)
        {
            // NPC'nin pozisyonunun biraz üzerinde prompt'u oluþtur.
            promptInstance = Instantiate(interactionPromptPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            // Prompt'un sürekli kameraya bakmasýný saðla (opsiyonel ama güzel bir özellik)
            promptInstance.AddComponent<FaceCamera>();
        }
        promptInstance.SetActive(true);
    }

    // Oyuncu etkileþim alanýndan çýktýðýnda çaðrýlacak.
    public void HidePrompt()
    {
        if (promptInstance != null)
        {
            promptInstance.SetActive(false);
        }
    }

    // "E" tuþuna basýldýðýnda bu fonksiyon çaðrýlacak.
    public void Interact()
    {
        OnInteract.Invoke();
    }
}

// Bu küçük yardýmcý script, World Space Canvas'ýn her zaman kameraya dönük olmasýný saðlar.
// Ayrý bir dosyaya koymaya gerek yok, Interactable.cs'in altýnda kalabilir.
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