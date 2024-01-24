using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class synergyTable : MonoBehaviour
{
    [SerializeField] protected GameObject mIconSynergyPre;
    [SerializeField] protected GameObject mIconChampPre;

    protected GameObject mIconSynergy;
    protected List<GameObject> mIconChamps;

    public synergyTable()
    {
        mIconChamps = new List<GameObject>();
    }

    protected GameObject MakeIcon( GameObject iconPrefub, string objName, string texPath, Vector2 pos )
    {
        GameObject _clone;
        _clone = Instantiate(iconPrefub, Vector2.zero, Quaternion.identity, this.transform);
        _clone.name = objName;
        _clone.GetComponent<iconBase>().SetTexture(texPath);
        _clone.GetComponent<iconBase>().SetPos(pos);

        return _clone;
    }


    public void MakeSynergyIcon( string[] synergyData, string synergyType)
    {
        string _nameJP  = synergyData[(int)synergyTableBg.CSV_SYNERGY_DATA.NAME_JP];
        string _nameENG = synergyData[(int)synergyTableBg.CSV_SYNERGY_DATA.NAME_ENG];
        string _texPath = "set3.5/" + synergyType + "/" + _nameENG;

        mIconSynergy = MakeIcon( mIconSynergyPre, _nameJP, _texPath, new Vector2(1.0f, -1.0f));
        mIconSynergy.name = synergyType + "_" + _nameJP;

        iconSynergy _iconSynergy = mIconSynergy.GetComponent<iconSynergy>();
        _iconSynergy.SetName( _nameJP, _nameENG );

        string _helpText = synergyData[(int)synergyTableBg.CSV_SYNERGY_DATA.EXPLANATION];
        _iconSynergy.MakeSynergyHelp(_helpText);

        string _activationHelpText = "";
        int _cnt = 0;

        foreach ( string i in synergyData)
        {
            if( _cnt >= (int)synergyTableBg.CSV_SYNERGY_DATA.ACTIVATION_LEVEL)
            {
                _activationHelpText += i + "\r\n";
            }

            _cnt++;
        }
        _iconSynergy.MakeSynergyConditionsHelp(_activationHelpText);
    }


    public void MakeChampions( List<string[]> champList, string synergyType )
    {
        // オリジンかクラスか判断して、変数を保持
        int _referSynergyType = (int)synergyTableBg.CSV_CHAMP_DATA.ORIGIN;
        if (synergyType == "class") _referSynergyType = (int)synergyTableBg.CSV_CHAMP_DATA.CLASS;

        // 
        float _startPosX = 230.0f;
        float _startPosY = (mIconSynergy.GetComponent<RectTransform>().sizeDelta.y + 2.0f) * -1;
        float _posMargin = 12.0f;

        float _posX = _startPosX;
        float _posY = _startPosY;


        // チャンピオンアイコン作成
        foreach (string[] i in champList)
        {
            if (!i[_referSynergyType].Contains(mIconSynergy.GetComponent<iconSynergy>().GetNameJP())) continue;

            GameObject _clone = MakeIcon(mIconChampPre, i[(int)synergyTableBg.CSV_CHAMP_DATA.NAME_JP], "set3.5/champion/" + i[(int)synergyTableBg.CSV_CHAMP_DATA.NAME_ENG], new Vector2(_posX, _posY));

            iconChamp _iconChamp = _clone.GetComponent<iconChamp>();
            _iconChamp.SetName(i[(int)synergyTableBg.CSV_CHAMP_DATA.NAME_JP], i[(int)synergyTableBg.CSV_CHAMP_DATA.NAME_ENG]);
            _iconChamp.SetCost( int.Parse(i[(int)synergyTableBg.CSV_CHAMP_DATA.COST]));
            _iconChamp.SetOrigin(i[(int)synergyTableBg.CSV_CHAMP_DATA.ORIGIN]);
            _iconChamp.SetClass(i[(int)synergyTableBg.CSV_CHAMP_DATA.CLASS]);

            _iconChamp.MakeFlame();     // チャンピオンアイコンにフレーム設定
            _iconChamp.MakeCostplate(); // コスト設定
            _iconChamp.MakeNameplate(); // チャンピオン名設定

            _posX += _clone.GetComponent<RectTransform>().sizeDelta.x + _posMargin;
            mIconChamps.Add(_clone);
        }
    }
}
