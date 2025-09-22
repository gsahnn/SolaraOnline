using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RegionTrigger : MonoBehaviour
{
    [SerializeField] private RegionData regionData;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Bir obje trigger'a girdi. Adý: " + other.gameObject.name + ", Tag'i: " + other.gameObject.tag);
        {
            WorldUI_Controller.Instance?.ShowRegionName(regionData);
        }
    }
}