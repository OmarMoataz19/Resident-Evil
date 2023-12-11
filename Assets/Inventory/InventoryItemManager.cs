using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemManager : MonoBehaviour
{

    public void OnInventoryItemClicked()
    {
        InventoryManager.Instance.SelectItem(
            gameObject.
            transform.
            Find("ItemIndex").
            GetComponent<TextMeshProUGUI>().text);
    }
}
