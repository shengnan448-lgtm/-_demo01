using MFarm.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [Header("组件获取")]
    [SerializeField] Image slotImage;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] public Image slotHightlight;
    [SerializeField] Button button;
    [Header("格子类型")]
    public SlotType slotType;
    public int itemAmount;
   [SerializeField]public bool isSelected;
   public ItemDetails itemDetails;
    [HideInInspector] public int slotIndex;
    InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
    private void Start()
    {
        isSelected = false;
        if(itemDetails==null)
        {
            UpdateEmptySlot();
        }
    }
    /// <summary>
    /// 时刻更新Slot的UI
    /// </summary>
    /// <param name="item">详细信息</param>
    /// <param name="amount">所对应的数量</param>
    public void UpdateSlot(ItemDetails item,int amount)
    {
        itemDetails = item;
        slotImage.sprite = item.itemIcon;
        itemAmount = amount;
        amountText.text = amount.ToString();
        button.interactable = true;
        slotImage.enabled = true;
    }
    public void UpdateEmptySlot()
    {
        if(isSelected)
        {
            isSelected = false;
        }
        slotImage.enabled = false;
        amountText.text = string.Empty;
        button.interactable = false;
        itemAmount = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemAmount == 0) return;
        isSelected = !isSelected;
        inventoryUI.UpdateSlotHightLight(slotIndex);
        if(slotType==SlotType.Bag)
        {
            EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
         if(itemAmount!=0)
        {
            inventoryUI.dragItem.enabled = true;
            inventoryUI.dragItem.sprite = slotImage.sprite;
            inventoryUI.dragItem.SetNativeSize();

            isSelected = true;
            inventoryUI.UpdateSlotHightLight(slotIndex);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        inventoryUI.dragItem.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inventoryUI.dragItem.enabled = false; 
        if(eventData.pointerCurrentRaycast.gameObject!=null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                return;
            var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
            int targetInex = targetSlot.slotIndex;
            if(slotType ==SlotType.Bag&&targetSlot.slotType ==SlotType.Bag)
            {
                InventoryManager.Instance.SwapItem(slotIndex, targetInex);
            }
            inventoryUI.UpdateSlotHightLight(-1);
        }
    }

}
