using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeItemCollection : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    [SerializeField] private Text _quantity;

    [SerializeField, Min(1)] private float _timeDestroy;
    private float _timeCount;

    [SerializeField] private Image _background;

    private void Awake()
    {
        _timeCount = _timeDestroy;
    }

    private void Update()
    {
        _timeCount -= Time.deltaTime;
        AutoDestroy();
    }

    private void AutoDestroy()
    {
        DrawNotice();

        if(_timeCount <= 0) Destroy(this.gameObject);
    }

    private void DrawNotice()
    {
        Color colorImg = _itemImage.color;
        Color colorText = _quantity.color;
        Color colorBackGr = _background.color;

        float rate = (_timeCount / _timeDestroy);

        colorImg.a = rate;
        colorText.a = rate;
        colorBackGr.a = rate;

        _itemImage.color = colorImg;
        _quantity.color = colorText;
        _background.color = colorBackGr;
    }

    public void SetItemDisplay(string formNotice , ItemSO item, int quantity)
    {
        _itemImage.sprite = item.ItemSprite;
        _quantity.text = formNotice + " : x " + quantity.ToString();
    }
}
