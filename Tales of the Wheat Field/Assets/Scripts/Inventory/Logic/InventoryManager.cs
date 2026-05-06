using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("背包数据")]
        public InventoryBag_SO playerBag_SO;
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;
        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }
        /// <summary>
        /// 通过id获得详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails( int id)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == id);
        }
        public void AddItem(Item item,bool toDestroy)
        { 
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID, index, 1);
            if(toDestroy)
            {
                Destroy(item.gameObject);
            }
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }
        /// <summary>
        /// 检查背包是否有空余空间
        /// </summary>
        /// <returns></returns>
        bool CheckBagCapacity() 
        {
            for (int i = 0; i < playerBag_SO.itemList.Count; i++)
            {
                if (playerBag_SO.itemList[i].itemID ==0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 返回对应ID的序号
        /// </summary>
        /// <param name="ID">itemID</param>
        /// <returns></returns>
        int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag_SO.itemList.Count; i++)
            {
                if (playerBag_SO.itemList[i].itemID ==ID)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 在指定背包序号位置添加物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">序号</param>
        /// <param name="amount">数量</param>
        void AddItemAtIndex(int ID,int index, int amount)
        {
            if(index==-1&&CheckBagCapacity())//背包中没找到 背包有空余
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag_SO.itemList.Count; i++)
                {
                    if (playerBag_SO.itemList[i].itemID ==0)
                    {
                        playerBag_SO.itemList[i] = item;
                        break;
                    }
                }
            }
            else
            {
                int currentAmount = playerBag_SO.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                playerBag_SO.itemList[index] = item;
            }
        }
        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag_SO.itemList[fromIndex];
            InventoryItem targetItem = playerBag_SO.itemList[targetIndex];
            if(targetItem.itemID!=0)
            {
                playerBag_SO.itemList[fromIndex] = targetItem;
                playerBag_SO.itemList[targetIndex] = currentItem;
            }
            else
            {
                playerBag_SO.itemList[targetIndex] = currentItem;
                playerBag_SO.itemList[fromIndex] = new InventoryItem();
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }
    }
}

