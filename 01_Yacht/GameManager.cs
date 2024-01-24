/*
    内容：ゲームの進行などを管理する
          アイコンが歯車になるのはなんで？
    予定：
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager mInstance = null; // getを使用して読み取り専用にしたほうがいい


    private void Awake()
    {
        if(mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad( this.gameObject );
        }
        // 複数シーンにプレハブなどで配置する場合は余計なものを削除するために必要
        /*else
        {
            Destroy(this.gameObject);
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
