// GameManager.cs
using System; // Event'ler i�in bu k�t�phane gerekli.
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    // Bu, projenin herhangi bir yerinden GameManager'a kolayca eri�memizi sa�layan bir tasar�m desenidir.
    // "GameManager.Instance" yazd���m�zda, bu tek ve �zel GameManager'a ula�aca��z.
    public static GameManager Instance { get; private set; }
    // -------------------------

    // --- OYUN DURUMLARI (GAME STATES) ---
    // Oyunun mevcut durumunu tutan de�i�ken. Ba�lang��ta Ana Men�'deyiz.
    private GameState _currentState = GameState.MainMenu;

    // Oyun durumu de�i�ti�inde di�er script'lere haber vermek i�in bir "event" (olay) olu�turuyoruz.
    // Di�er script'ler bu olaya abone olup, durum de�i�ti�inde kendi fonksiyonlar�n� �al��t�rabilirler.
    public static event Action<GameState> OnGameStateChanged;
    // ------------------------------------

    // --- UNITY FONKS�YONLARI ---

    // Awake, oyun ba�lad���nda �al��an ilk fonksiyondur (Start'tan bile �nce).
    // Singleton'� kurmak i�in en ideal yerdir.
    private void Awake()
    {
        // E�er sahnede zaten bir GameManager varsa, bu yenisini yok et.
        // Bu, sahneler aras� ge�i�te birden fazla GameManager olu�mas�n� engeller.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // E�er yoksa, bu objeyi tek ve �zel GameManager olarak ata.
        Instance = this;

        // Bu GameManager'�n sahneler aras�nda ge�i� yapsak bile yok olmamas�n� sa�lar.
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // NOT: Build Settings'e sahneleri ekledi�imizden emin olmal�y�z!
        // �lk oyun sahnesini (�rne�in ana men� veya ilk harita) y�kle.
        // "LoadSceneMode.Additive" sayesinde mevcut sahnenin (Manager_Scene) �zerine ekler.
        SceneManager.LoadScene("Test_E�yaToplama", LoadSceneMode.Additive);
    }
    // ---------------------------

    // --- GENEL FONKS�YONLAR ---

    // Bu fonksiyon, oyunun durumunu de�i�tirmek i�in kullan�lacak.
    // D��ar�dan �a�r�lacak ve yeni durumu ayarlayacak.
    public void UpdateGameState(GameState newState)
    {
        // E�er yeni durum mevcut durumla ayn�ysa, bir �ey yapmaya gerek yok.
        if (newState == _currentState) return;

        // Mevcut durumu g�ncelle.
        _currentState = newState;

        // Durum de�i�ti�inde, bu de�i�ikli�i dinleyen t�m di�er script'lere haber ver.
        // Soru i�areti (?), kimsenin dinlemedi�i (abone olmad���) durumda hata vermesini engeller.
        OnGameStateChanged?.Invoke(newState);

        // Konsola hangi duruma ge�ti�imizi yazarak test etmemizi sa�lar.
        Debug.Log("Game State changed to: " + newState);
    }
    // ------------------------
}