using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour
{
    [SerializeField] private GameObject description;
    [SerializeField] private Text itemTitle;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemDescription;

    private MenuShopItem selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        description.SetActive(false);
    }

    public void SelectItem(MenuShopItem item)
    {
        this.selectedItem = item;
        description.SetActive(true);
        itemTitle.text = item.name;
        itemImage.sprite = item.icon;
        itemDescription.text = item.description;
    }

    public void PurchaseItem()
    {
        print("Purchased " + itemTitle);
    }
}
