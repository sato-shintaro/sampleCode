using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class synergyTableBg : MonoBehaviour
{
    [SerializeField] protected GameObject mSynergyTableBgPre;
    [SerializeField] protected TextAsset mCsvSynergyList;
    [SerializeField] protected TextAsset mCsvChampList;
    [SerializeField] protected string mSynergyType;

    protected List<string[]> mSynergyList;
    public enum CSV_SYNERGY_DATA // CSVのデータ並び
    {
        NO,                 // 通し番号
        NAME_JP,            // 日本語
        NAME_ENG,           // 英語
        EXPLANATION,        // 効果説明
        ACTIVATION_LEVEL    // 発動条件
    }

    protected List<string[]> mChampList;
    public enum CSV_CHAMP_DATA // CSVのデータ並び
    {
        NO,         // 通し番号
        NAME_JP,    // 名前_日本語
        NAME_ENG,   // 名前_英語
        COST,       // コスト
        ORIGIN,     // オリジン
        CLASS       // クラス
    }



    // Start is called before the first frame update
    void Start()
    {
        // シナジーリストの読み込み
        mSynergyList = new List<string[]>();
        mSynergyList = csvReader.csvSplit(mCsvSynergyList, '\t');
        mSynergyList.RemoveAt(0); // 最初の行は不要なので削除

        // チャンピオンリストの読み込み
        mChampList = new List<string[]>();
        mChampList = csvReader.csvSplit(mCsvChampList, '\t');
        mChampList.RemoveAt(0); // 最初の行は不要なので削除

        // シナジーテーブルの作成
        MakeSynergyTable();
    }


    void MakeSynergyTable()
    {
        float _posX = 5.0f;
        float _posY = -2.0f;
        float _posMargin = 2.0f;

        foreach (string[] i in mSynergyList)
        {
            // 各シナジー情報の背景を作成設置
            GameObject _clone = Instantiate(mSynergyTableBgPre, Vector2.zero, Quaternion.identity, this.transform);
            _clone.name = i[(int)CSV_SYNERGY_DATA.NAME_ENG];
            _clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);

            // シナジー情報の作成設置
            synergyTable _synergyTable = _clone.GetComponent<synergyTable>();
            _synergyTable.MakeSynergyIcon(i, mSynergyType);
            _synergyTable.MakeChampions(mChampList, mSynergyType);

            _posY += (mSynergyTableBgPre.GetComponent<RectTransform>().sizeDelta.y + _posMargin) * -1;
        }
    }
}
