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

    // Bu fonksiyon, bir obje trigger'�n i�inde durdu�u S�RECE HER FRAME �A�RILIR.
    private void OnTriggerStay(Collider other)
    {
        // Bu mesaj� konsolda g�rmemiz �ART!
        Debug.Log("<size=20><color=orange>OnTriggerStay �ALI�IYOR! Giren obje: " + other.name + ", Tag: " + other.tag + "</color></size>");

        if (other.CompareTag("Player"))
        {
            // Bu mesaj� g�r�yorsak, tag de do�ru demektir.
            Debug.Log("<size=20><color=green>OYUNCU ALGILANDI!</color></size>");

            // WorldUI_Controller'�n null olup olmad���n� kontrol edelim.
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