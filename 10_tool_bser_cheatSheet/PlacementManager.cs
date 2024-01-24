using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] protected GameObject mAreaNameBgPre;
    protected GameObject mAreaNameBg;

    [SerializeField] protected GameObject mAreaNamePre;
    protected GameObject mAreaName;

    protected string mAreaNameJP;

    [SerializeField] protected GameObject mItemListBgPre;
    protected GameObject mItemListBg;

    [SerializeField] protected GameObject mItemPanelPre;
    protected List<GameObject> mItemPanel;

    protected List<string[]> mItemList;


    void Awake()
    {
        mItemList  = new List<string[]>();
        mItemPanel = new List<GameObject>();

        mAreaNameBg = Instantiate(mAreaNameBgPre, Vector2.zero, Quaternion.identity, this.transform);
        mAreaNameBg.name = "areaNameBg";
        mAreaNameBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

        mAreaName = Instantiate(mAreaNamePre, Vector2.zero, Quaternion.identity, mAreaNameBg.transform);
        mAreaName.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

        mItemListBg = Instantiate(mItemListBgPre, Vector2.zero, Quaternion.identity, this.transform);
        mItemListBg.name = "itemListBg";
        mItemListBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(mAreaNameBg.GetComponent<RectTransform>().sizeDelta.x-2.0f, 0.0f);
    }

    public void Setting()
    {
        var _placeList = new Dictionary<string, string>(){
            {"洋弓場",  "0"}, {"路地裏",  "1"}, {"下町"      ,  "2"}, {"学校",  "3"}, {"寺"  ,  "4"},
            {"ホテル",  "5"}, {"池"    ,  "6"}, {"浜辺"      ,  "7"}, {"病院",  "8"}, {"森"  ,  "9"},
            {"教会"  , "10"}, {"墓場"  , "11"}, {"高級住宅街", "12"}, {"港"  , "13"}, {"工場", "14"}
        };

        float[,] _placePos = new float[,]{
            {  2.0f,   -1.0f}, {447.0f,   -1.0f}, {892.0f,   -1.0f}, {  2.0f, -181.0f}, {892.0f, -181.0f},
            {  2.0f, -361.0f}, {892.0f, -361.0f}, {  2.0f, -541.0f}, {892.0f, -541.0f}, {  2.0f, -721.0f},
            {457.0f, -721.0f}, {892.0f, -721.0f}, {  2.0f, -901.0f}, {437.0f, -901.0f}, {892.0f, -901.0f}
        };

        string[] _placeColor = { "#550000", "#005500", "#000055", "#555500", "#005555" };

        int _placeNum = int.Parse( _placeList[this.name] );

        this.GetComponent<RectTransform>().anchoredPosition = new Vector2( _placePos[_placeNum, 0], _placePos[_placeNum, 1] );

        Color _bgColor;
        ColorUtility.TryParseHtmlString(_placeColor[_placeNum%5], out _bgColor);
        mAreaNameBg.GetComponent<Image>().color = _bgColor;
        mItemListBg.GetComponent<Image>().color = _bgColor;

        CreateItemData();
    }

    public void   SetAreaName( string name ) { mAreaNameJP = name; }
    public string GetAreaName()              { return mAreaNameJP; }

    public void SetAreaNameTexture( string texPath )
    {
        Image _imageComponent = mAreaName.GetComponent<Image>();
        Texture2D _texture = Resources.Load<Texture2D>(texPath);

        _imageComponent.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
        _imageComponent.SetNativeSize();
    }


    public void AddItemList( string[] itemData )
    {
        mItemList.Add( itemData );
    }


    public void CreateItemData()
    {
        const int _maxCnt = 6;
        int _cnt = 0;
        int _collectCnt = 0;
        int _monsterCnt = 0;

        float _defPosX = 3.0f;
        float _defPosY = -3.0f;
        float _defCollectPosX = 3.0f;
        float _defCollectPosY = -105.0f;
        float _defMonsterPosX = 3.0f;
        float _defMonsterPosY = -143.0f;

        float _marginX = 59.0f;
        float _marginY = 33.0f;

        float _posX = _defPosX;
        float _posY = _defPosY;

        float _collectPosX = _defCollectPosX;
        float _collectPosY = _defCollectPosY;

        float _monsterPosX = _defMonsterPosX;
        float _monsterPosY = _defMonsterPosY;

        foreach( string[] i in mItemList)
        {
            GameObject _clone = Instantiate(mItemPanelPre, Vector2.zero, Quaternion.identity, mItemListBg.transform);

            if( i[1] == "-")           SetItemPanelPos(ref _clone, ref _cnt, _maxCnt, ref _posX, ref _posY, _defPosX, _defPosY, _marginX, _marginY );
            else if ( i[1] == "採取" ) SetItemPanelPos(ref _clone, ref _collectCnt, _maxCnt, ref _collectPosX, ref _collectPosY, _defCollectPosX, _defCollectPosY, _marginX, _marginY);
            else if ( i[1] == "動物" ) SetItemPanelPos(ref _clone, ref _monsterCnt, _maxCnt, ref _monsterPosX, ref _monsterPosY, _defMonsterPosX, _defMonsterPosY, _marginX, _marginY);

            _clone.GetComponent<ItemData>().MakeData( i );

            mItemPanel.Add( _clone );
        }
    }

    private void SetItemPanelPos(ref GameObject obj, ref int cnt, int maxCnt, ref float posX, ref float posY, float defPosX, float defPosY, float marginX, float marginY)
    {
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

        posX += marginX;
        cnt++;

        if (cnt >= maxCnt)
        {
            cnt   = 0;
            posX  = defPosX;
            posY -= marginY;
        }
    }
}
