using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] GameObject mDicePre;
    
    private int mDiceMaxCnt = 5;
    private List<GameObject> mDiceList = new List<GameObject>();
    private GameObject mStorageBox;


    // Start is called before the first frame update
    void Start()
    {
        mStorageBox = GameObject.Find("StorageBox");
        
        for( int i = 0; i < mDiceMaxCnt; i++ )
        {
            Vector3    _randomDicePos = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized * 0.3f;
            Vector3    _dicePos       = mStorageBox.GetComponent<Transform>().position + _randomDicePos;
            Quaternion _diceRotation  = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            _dicePos.y += 0.2f * (i + 1); // サイコロごとに初期高さ変えたい

            mDiceList.Add( Instantiate( mDicePre, _dicePos, _diceRotation ) );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( IsAllSleepDice() && mStorageBox.GetComponent<StorageBox>().IsDiceRoll)
        {
            DiceRollResult();
        }
    }


    private bool IsAllSleepDice()
    {
        bool _result = true;

        foreach(var item in mDiceList)
        {
            if( item.GetComponent<Dice>().Moving )
            {
                _result = false;
                break;
            }
        }

        return _result;
    }


    /// <summary>
    /// サイコロの出目を個数に応じて中央から均等に横並びで画面に近づける
    /// </summary>
    private void DiceRollResult()
    {
        //※※：位置を等倍で配置するものがあるらしい：参考サイトブックマークしてる
        foreach (var item in mDiceList)
        {
            Debug.Log(item.transform.rotation.x);
        }
    }


    /// <summary>
    /// 全サイコロの初期化を行う（StorageBoxに戻す）
    /// </summary>
    public void Init()
    {
        foreach (var item in mDiceList)
        {
            item.GetComponent<Dice>().Init();
        }
    }
}
