using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Collider))]
public class RegionTrigger : MonoBehaviour
{
    [SerializeField] private RegionData regionData;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    // Bu fonksiyon, bir obje trigger'ýn içinde durduðu SÜRECE HER FRAME ÇAÐRILIR.
    private void OnTriggerStay(Collider other)
    {
        // Bu mesajý konsolda görmemiz ÞART!
        Debug.Log("<size=20><color=orange>OnTriggerStay ÇALIÞIYOR! Giren obje: " + other.name + ", Tag: " + other.tag + "</color></size>");

        if (other.CompareTag("Player"))
        {
            // Bu mesajý görüyorsak, tag de doðru demektir.
            Debug.Log("<size=20><color=green>OYUNCU ALGILANDI!</color></size>");

            // WorldUI_Controller'ýn null olup olmadýðýný kontrol edelim.
            if (WorldUI_Controller.Instance != null)
            {
                WorldUI_Controller.Instance.ShowRegionName(regionData);
            }
            else
            {
                Debug.LogError("WorldUI_Controller.Instance BULUNAMADI!");
            }
        }
    }
} 