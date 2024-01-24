using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class variousInfo : MonoBehaviour
{
    [SerializeField] protected GameObject mInfoTextPre;
    [SerializeField] protected TextAsset mCsvRollingChancesTable;
    protected List<string[]> mRollingChancesTable;
    protected string mPatchVer = "SET 3.5  PATCH 10.17";

    // Start is called before the first frame update
    void Start()
    {
        mRollingChancesTable = new List<string[]>();
        mRollingChancesTable = csvReader.csvSplit(mCsvRollingChancesTable, '\t' );

        MakeRollingChancesTable();
        MakePatchVer();
    }

    void MakeRollingChancesTable()
    {
        // ※※ そのうちシングルトンクラスとかにカラーテーブルまとめるか？
        string[] _tierHtmlColor = { "#ffffff", "#919191", "#1b8f4a", "#1d85b6", "#b21e8a", "#d98014" };

        float _startPosX = 0.0f;
        float _startPosY = -10.0f;
        float _posMargin = 3.0f;

        int _cnt = 0;
        foreach ( string[] i in mRollingChancesTable)
        {
            float _posX = _startPosX;
            float _posY = _startPosY - ((_posMargin + mInfoTextPre.GetComponent<RectTransform>().sizeDelta.y) * _cnt);
            int _colorCnt = 0;

            foreach ( string j in i)
            {
                GameObject _clone = Instantiate(mInfoTextPre, Vector2.zero, Quaternion.identity, this.transform);
                _clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);

                Text _cloneTextComponent = _clone.GetComponent<Text>();
                _cloneTextComponent.alignment = TextAnchor.MiddleRight;
                //_cloneTextComponent.fontSize = 14;

                Color _fontColor;
                ColorUtility.TryParseHtmlString(_tierHtmlColor[_colorCnt], out _fontColor);
                _cloneTextComponent.color = _fontColor;

                _cloneTextComponent.text = j;
                //_cloneTextComponent.text = "<color=#919191>te</color>st"; // ※※ テキストの中にＨＴＭＬみたいなコード仕込めるみたい

                _posX += _posMargin + mInfoTextPre.GetComponent<RectTransform>().sizeDelta.x;
                _colorCnt++;
            }

            _cnt++;
        }
    }


    public void MakePatchVer()
    {
        GameObject _clone = Instantiate(mInfoTextPre, Vector2.zero, Quaternion.identity, this.transform);
        _clone.name = "patchVerText";
        Text _cloneText = _clone.GetComponent<Text>();

        _cloneText.text     = "<color=#919191>" + mPatchVer + "</color>";
        _cloneText.fontSize = 20;
        _cloneText.horizontalOverflow = HorizontalWrapMode.Overflow;
        _cloneText.verticalOverflow   = VerticalWrapMode.Overflow;

        RectTransform _cloneRect = _clone.GetComponent<RectTransform>();
        RectTransform _thisRect  = this.GetComponent<RectTransform>();
        _cloneRect.sizeDelta = new Vector2(_cloneText.preferredWidth, _cloneText.preferredHeight);
        float _posX = 0.0f; // _thisRect.sizeDelta.x - _cloneRect.sizeDelta.x;
        float _posY = (_thisRect.sizeDelta.y - _cloneRect.sizeDelta.y) * -1;

        _clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);
    }
}
