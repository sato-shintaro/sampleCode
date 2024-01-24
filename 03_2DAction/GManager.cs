using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    public int score;        // ゲームスコア
    public int stageNum;     // 現在のステージ数
    public int continueNum;  // 現在のコンテニュー位置


    private void Awake()
    {
        // あんまりお行儀がよろしく無いらしい。
        // 恐らく「instance」をプライベートにして外からは「get」関数とかを作って呼び出される方が良さそう
        if(instance == null )
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
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
