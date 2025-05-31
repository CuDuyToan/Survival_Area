using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeItem : MonoBehaviour //Quantity change notification
{
    [SerializeField] private GameObject _content;

    [SerializeField] private GameObject _noticePrefab;


    private void OnEnable()
    {
        InventoryPlayer.OnReceivedItem_notice += DisplayNoticeAddItem;
        RepairStructure.itemNeedNotice += DisplayNoticeNeedItem;
        InventoryPlayer.OnConsumeItem_notice += DisplayNoticeRemoveItem;
    }

    private void OnDisable()
    {
        InventoryPlayer.OnReceivedItem_notice -= DisplayNoticeAddItem;
        RepairStructure.itemNeedNotice -= DisplayNoticeNeedItem;
        InventoryPlayer.OnConsumeItem_notice -= DisplayNoticeRemoveItem;
    }

    private void DisplayNoticeAddItem(ItemSO item, int amount)
    {
        if (item == null) return;

        GameObject _noticeObj = Instantiate(_noticePrefab, _content.transform);

        _noticeObj.transform.SetAsFirstSibling(); // Đẩy lên trên cùng

        NoticeItemCollection itemCollection = _noticeObj.GetComponent<NoticeItemCollection>();

        if(itemCollection)
        {
            itemCollection.SetItemDisplay("ADD", item, amount);
        }
    }

    private void DisplayNoticeNeedItem(ItemSO item, int amount)
    {
        if (item == null) return;

        GameObject _noticeObj = Instantiate(_noticePrefab, _content.transform);

        _noticeObj.transform.SetAsFirstSibling(); // Đẩy lên trên cùng

        NoticeItemCollection itemCollection = _noticeObj.GetComponent<NoticeItemCollection>();

        if (itemCollection)
        {
            itemCollection.SetItemDisplay("NEED" , item, amount);
        }
    }

    private void DisplayNoticeRemoveItem(ItemSO item, int amount)
    {
        if (item == null) return;

        GameObject _noticeObj = Instantiate(_noticePrefab, _content.transform);

        _noticeObj.transform.SetAsFirstSibling(); // Đẩy lên trên cùng

        NoticeItemCollection itemCollection = _noticeObj.GetComponent<NoticeItemCollection>();

        if (itemCollection)
        {
            itemCollection.SetItemDisplay("Remove", item, amount);
        }
    }
}
