using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    [SerializeField] protected GameObject mItemPre;
    protected GameObject mItem;

    [SerializeField] protected GameObject mItemFramePre;
    protected GameObject mItemFrame;

    [SerializeField] protected GameObject mNumBgPre;
    protected GameObject mNumBg;

    [SerializeField] protected GameObject mNumTextPre;
    protected GameObject mNumText;

    protected string mHowToGet;  // 入手方法
    protected string mType;      // 種類
    protected string mName;      // 名前
    protected string mNameHira;  // ひらがな名
    protected string mNum;       // 数


    public void MakeData( string[] itemData)
    {
        mHowToGet = itemData[1];  // 
        mType     = itemData[2];  // 
        mName     = itemData[3];  // 
        this.name = itemData[3];  // 
        mNameHira = itemData[4];  // 
        mNum      = itemData[5];  // 

        mItem = Instantiate(mItemPre, Vector2.zero, Quaternion.identity, this.transform);
        mItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

        mItemFrame = Instantiate(mItemFramePre, Vector2.zero, Quaternion.identity, this.transform);
        mItemFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

        mNumBg = Instantiate(mNumBgPre, Vector2.zero, Quaternion.identity, mItemFrame.transform);
        mNumBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(43.0f, -16.0f);

        mNumText = Instantiate(mNumTextPre, Vector2.zero, Quaternion.identity, mNumBg.transform);
        mNumText.GetComponent<Text>().text = mNum;
        mNumText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -0.0f);


        var _itemTypeList = new Dictionary<string, string>(){
            {"武器", "0"},{"防具", "1"},{"食べ物", "2"},{"飲み物", "3"},{"設置物", "4"},{"材料", "5"},{"-", "6"}
        };
        int _itemType = int.Parse(_itemTypeList[mType]);
        string[] _bgColorList = { "#6a0f1e", "#0a2f0d", "#780a44", "#0a384b", "#a9570b", "#44260a", "#ffffff" };

        Color _bgColor;
        ColorUtility.TryParseHtmlString(_bgColorList[_itemType], out _bgColor);
        this.GetComponent<Image>().color = _bgColor;

        string _itemTexPath = mHowToGet != "動物" ? "Season1/item/" : "Season1/monster/";
        SetTexture(_itemTexPath + mName);
    }

    public void SetTexture( string texPath )
    {
        Image _imageComponent = mItem.GetComponent<Image>();
        Texture2D _texture = Resources.Load<Texture2D>(texPath);

        _imageComponent.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
        _imageComponent.SetNativeSize();
    }
}
