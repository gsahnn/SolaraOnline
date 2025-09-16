// Interactable.cs (GÜNCELLENMÝÞ HALÝ)
using UnityEngine;
using UnityEngine.Events;

// GameObject parametresi alan yeni bir UnityEvent türü tanýmlýyoruz.
[System.Serializable]
public class InteractionEvent : UnityEvent<GameObject> { }

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPromptPrefab;
    public InteractionEvent OnInteract; // Artýk GameObject alabilen yeni event'imizi kullanýyoruz.

    private GameObject promptInstance;

    public void ShowPrompt()
    {
        if (promptInstance == null)
        {
            promptInstance = Instantiate(interactionPromptPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            promptInstance.AddComponent<FaceCamera>();
        }
        promptInstance.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptInstance != null) promptInstance.SetActive(false);
    }

    public void Interact(GameObject interactor)
    {
        OnInteract.Invoke(interactor);
    }
}

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