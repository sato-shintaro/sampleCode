using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEngine;
using UnityEngine.UI;

public class PlacementMapManager : MonoBehaviour
{
    [SerializeField] protected GameObject mPlacePre;
    [SerializeField] protected TextAsset mCsvPlacementList;

    protected List<string[]> mPlacementMapList;
    protected List<GameObject> mPlacementList;

    // Start is called before the first frame update
    void Start()
    {
        mPlacementMapList = new List<string[]>();
        mPlacementList    = new List<GameObject>();

        mPlacementMapList = csvReader.csvSplit(mCsvPlacementList, '\t', true);

        foreach ( string[] i in mPlacementMapList)
        {
            // csv の1行目が空なら無視
            if (i[0] == "") continue;

            // すでに同じ場所のリストが作られていないかチェックして、なければ作成
            if (!CheckExistence(i[0]))
            {
                MakePlace(i[0]);
            }

            // リストから対象の場所を探して、その場所にデータを追加
            foreach( GameObject j in mPlacementList)
            {
                if( i[0] == j.name)
                {
                    j.GetComponent<PlacementManager>().AddItemList( i );
                    break;
                }
            }
        }

        foreach( GameObject i in mPlacementList)
        {
            i.GetComponent<PlacementManager>().Setting();
        }
    }

    void MakePlace( string name )
    {
        string _areaNameTexPath = "Season1/area/areaName_";

        //クローン作製
        GameObject _clone = Instantiate(mPlacePre, Vector2.zero, Quaternion.identity, this.transform);
        _clone.name = name;

        PlacementManager _temp = _clone.GetComponent<PlacementManager>();
        _temp.SetAreaName(name);
        _temp.SetAreaNameTexture(_areaNameTexPath + name);

        mPlacementList.Add( _clone );
    }

    bool CheckExistence( string target )
    {
        // Listにデータが無い場合はfalse
        if (mPlacementList.Count == 0) return false;

        // すでにListに同名の場所が存在した場合はtrue 無ければfalse
        foreach(GameObject i in mPlacementList)
        {
            PlacementManager _temp = i.GetComponent<PlacementManager>();

            if ( _temp.GetAreaName() == target )
            {
                return true;
            }
        }

        return false;
    }
}
