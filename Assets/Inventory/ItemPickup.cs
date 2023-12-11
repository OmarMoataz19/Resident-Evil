using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem Item;

    void Pickup()
    {
        if (InventoryManager.Instance.Add(Item))
        {
            Destroy(gameObject);
        }
        InventoryManager.Instance.ListItems();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }
        } 
    }



    

}
