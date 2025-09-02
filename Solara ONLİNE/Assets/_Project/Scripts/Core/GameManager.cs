// GameManager.cs
using System; // Event'ler için bu kütüphane gerekli.
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    // Bu, projenin herhangi bir yerinden GameManager'a kolayca eriþmemizi saðlayan bir tasarým desenidir.
    // "GameManager.Instance" yazdýðýmýzda, bu tek ve özel GameManager'a ulaþacaðýz.
    public static GameManager Instance { get; private set; }
    // -------------------------

    // --- OYUN DURUMLARI (GAME STATES) ---
    // Oyunun mevcut durumunu tutan deðiþken. Baþlangýçta Ana Menü'deyiz.
    private GameState _currentState = GameState.MainMenu;

    // Oyun durumu deðiþtiðinde diðer script'lere haber vermek için bir "event" (olay) oluþturuyoruz.
    // Diðer script'ler bu olaya abone olup, durum deðiþtiðinde kendi fonksiyonlarýný çalýþtýrabilirler.
    public static event Action<GameState> OnGameStateChanged;
    // ------------------------------------

    // --- UNITY FONKSÝYONLARI ---

    // Awake, oyun baþladýðýnda çalýþan ilk fonksiyondur (Start'tan bile önce).
    // Singleton'ý kurmak için en ideal yerdir.
    private void Awake()
    {
        // Eðer sahnede zaten bir GameManager varsa, bu yenisini yok et.
        // Bu, sahneler arasý geçiþte birden fazla GameManager oluþmasýný engeller.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Eðer yoksa, bu objeyi tek ve özel GameManager olarak ata.
        Instance = this;

        // Bu GameManager'ýn sahneler arasýnda geçiþ yapsak bile yok olmamasýný saðlar.
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // NOT: Build Settings'e sahneleri eklediðimizden emin olmalýyýz!
        // Ýlk oyun sahnesini (örneðin ana menü veya ilk harita) yükle.
        // "LoadSceneMode.Additive" sayesinde mevcut sahnenin (Manager_Scene) üzerine ekler.
        SceneManager.LoadScene("Test_EþyaToplama", LoadSceneMode.Additive);
    }
    // ---------------------------

    // --- GENEL FONKSÝYONLAR ---

    // Bu fonksiyon, oyunun durumunu deðiþtirmek için kullanýlacak.
    // Dýþarýdan çaðrýlacak ve yeni durumu ayarlayacak.
    public void UpdateGameState(GameState newState)
    {
        // Eðer yeni durum mevcut durumla aynýysa, bir þey yapmaya gerek yok.
        if (newState == _currentState) return;

        // Mevcut durumu güncelle.
        _currentState = newState;

        // Durum deðiþtiðinde, bu deðiþikliði dinleyen tüm diðer script'lere haber ver.
        // Soru iþareti (?), kimsenin dinlemediði (abone olmadýðý) durumda hata vermesini engeller.
        OnGameStateChanged?.Invoke(newState);

        // Konsola hangi duruma geçtiðimizi yazarak test etmemizi saðlar.
        Debug.Log("Game State changed to: " + newState);
    }
    // ------------------------
}