using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class itemMixList : MonoBehaviour
{
    [SerializeField] protected GameObject mIconItemPre;
    protected List<GameObject> mIconItemList;

    [SerializeField] protected TextAsset mCsvItemList;
    [SerializeField] protected TextAsset mCsvItemMixTable;

    protected List<string[]> mItemList;
    protected List<string[]> mItemMixTable;

    // Start is called before the first frame update
    void Start()
    {
        mIconItemList = new List<GameObject>();

        mItemList     = new List<string[]>();
        mItemMixTable = new List<string[]>();

        mItemList     = csvReader.csvSplit(mCsvItemList, '\t');
        mItemMixTable = csvReader.csvSplit(mCsvItemMixTable, '\t' );

        MakeMixTable();
    }


    void MakeMixTable()
    {
        //string _texFullPath      = "Assets/TFTProject/Images/Resources/set3.5/item/";
        string _texturePath = "set3.5/item/";

        float _startPosX = 7.0f;
        float _startPosY = -7.0f;
        float _posMargin = 4.0f;
        int _cnt = 0;

        foreach( string[] i in mItemMixTable)
        {
            float _posX = _startPosX;
            float _posY = _startPosY - ((mIconItemPre.GetComponent<RectTransform>().sizeDelta.y + _posMargin) * _cnt);

            foreach( string j in i)
            {
                /* ※※ Resourcesにあるデータの存在チェックしてから読み込もうとしたときってどうしたらいいんだろう？
                if (!System.IO.File.Exists(_texFullPath + j + ".png") && !System.IO.File.Exists(_texFullPath + j))
                {
                    _posX += iconItemPre.GetComponent<RectTransform>().sizeDelta.x + _posMargin;
                    this.GetComponent<RectTransform>().localScale = new Vector2(0.5f, 0.5f);
                    continue;
                }
                */
                if (j == "")
                {
                    _posX += mIconItemPre.GetComponent<RectTransform>().sizeDelta.x + _posMargin;
                    continue;
                }

                GameObject _clone = Instantiate(mIconItemPre, Vector2.zero, Quaternion.identity, this.transform );
                _clone.name = "iconItem" + j;

                _clone.GetComponent<iconItem>().SetTexture(_texturePath + j);
                _clone.GetComponent<iconItem>().SetPos(new Vector2(_posX, _posY));

                _posX += _clone.GetComponent<RectTransform>().sizeDelta.x + _posMargin;

                mIconItemList.Add( _clone );
            }

            _cnt++;
        }
    }
}
