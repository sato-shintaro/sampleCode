using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iconChamp : iconBase
{
    [SerializeField] protected GameObject mIconFramePre;
    [SerializeField] protected GameObject mNameplatePre;
    [SerializeField] protected GameObject mCostBgPre;
    [SerializeField] protected GameObject mCostTextPre;

    protected GameObject mIconFrame;
    protected GameObject mNameplate;
    protected GameObject mCostBg;
    protected GameObject mCostText;

    protected int mCost;
    protected string mOrigin;
    protected string mClass;

    public void SetCost( int cost ){ mCost = cost; }
    public void SetOrigin( string originSynergy ){ mOrigin = originSynergy; }
    public void SetClass( string classSynergy ){ mClass = classSynergy; }
    public void SetFrameColor()
    {
        // ※※ そのうちシングルトンクラスとかにカラーテーブルまとめるか？
        string[] _tierHtmlColor = { "#ffffff", "#919191", "#1b8f4a", "#1d85b6", "#b21e8a", "#d98014" };
        Color _frameColor;
        ColorUtility.TryParseHtmlString(_tierHtmlColor[mCost], out _frameColor);
        mIconFrame.GetComponent<Image>().color = _frameColor;
    }


    public void MakeFlame()
    {
        mIconFrame = Instantiate(mIconFramePre, Vector2.zero, Quaternion.identity, this.transform);
        mIconFrame.name = "Frame_"+mNameJP;
        mIconFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f,0.0f);
        SetFrameColor();
    }


    public void MakeNameplate()
    {
        mNameplate = Instantiate(mNameplatePre, Vector2.zero, Quaternion.identity, this.transform);
        mNameplate.name = "Nameplate_" + mNameJP;

        RectTransform _frameRect = mIconFrame.GetComponent<RectTransform>();
        float _posX = (this.GetComponent<RectTransform>().sizeDelta.x - _frameRect.sizeDelta.x) * 0.5f; // フレームの左端と同じ開始位置になるように
        float _posY = _frameRect.sizeDelta.y * -1;
        mNameplate.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);

        mNameplate.GetComponent<Text>().text = mNameJP;
    }


    public void MakeCostplate()
    {
        mCostBg = Instantiate(mCostBgPre, Vector2.zero, Quaternion.identity, this.transform);
        mCostBg.name = "CostBg_" + mNameJP;

        RectTransform _icon = this.GetComponent<RectTransform>();
        float _posX = _icon.sizeDelta.x;
        float _posY = -_icon.sizeDelta.y;
        mCostBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);

        // ※※ そのうちシングルトンクラスとかにカラーテーブルまとめるか？
        //string[] _tierHtmlColor = { "#ffffff", "#919191", "#1b8f4a", "#1d85b6", "#b21e8a", "#d98014" };
        string[] _tierHtmlColor = { "#ffffff", "#666666", "#146334", "#1E6485", "#8A1C6C", "#AB6611" };
        Color _costBgColor;
        ColorUtility.TryParseHtmlString(_tierHtmlColor[mCost], out _costBgColor);
        mCostBg.GetComponent<Image>().color = _costBgColor;

        mCostText = Instantiate(mCostTextPre, Vector2.zero, Quaternion.identity, mCostBg.transform);
        mCostText.name = "Cost_" + mNameJP;
        mCostText.GetComponent<Text>().text = mCost.ToString();
        mCostText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
    }
}
