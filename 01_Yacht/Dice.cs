/*
    内容：サイコロの状態を管理するクラス。
    予定：
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class Dice : MonoBehaviour, IPointerClickHandler
{
    private Rigidbody rb;
 
    private int diceNumber = -1;  // 出目の記録用
    private string upVector;      // 上を向いてるxyz軸保存用

    private Vector3 initPos;       // 初期位置
    private Vector3 moveTargetPos; // 移動対象位置

    private bool moving;        // ダイスが動いているか判定用のフラグ
    private bool movingTarget;  // ダイスが目標地点に向かって動いているかのフラグ
    private bool keep;          // ダイスの目をキープする用のフラグ


    public int DiceNumber
    {
        get => diceNumber;
        // set => diceNumber = value;  // 今回外部からは読み取り専用なのでコメントアウト
    }
    // 片方の場合はこういう書き方もできる
    // public int DiceNumber => diceNumber;
    // public int DiceNumber => diceNumber = value;

    public bool Moving { get => moving; }
    public bool Keep{ get => keep; }
    public string UpVector { get => upVector; }
    public Vector3 MoveTargetPos { set => moveTargetPos = value; }


    // Start is called before the first frame update
    void Start()
    {
        initPos = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        // レイヤーの変更：keepするかのクリックできるタイミング変更でいるかも
        /*
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.gameObject.layer = 0;
        */

        DiceRollDecision();

        if(movingTarget) MoveToTarget();
    }


    // メモ：物理挙動の更新の直前に呼ばれるらしい
    private void FixedUpdate()
    {
        moving = rb.IsSleeping() ? false : true;
    }


    public void Init()
    {
        this.transform.position = initPos;
    }


    /// <summary>
    /// サイコロの出目を判定する
    /// </summary>
    private void DiceRollDecision()
    {
        // 面の向きを元に、Y座標の絶対値が一番大きいものを出目とする
        Vector3 _d_forward = this.transform.TransformDirection(Vector3.forward);  // ダイスの前面
        Vector3 _d_right   = this.transform.TransformDirection(Vector3.right);    // ダイスの右面
        Vector3 _d_up      = this.transform.TransformDirection(Vector3.up);       // ダイスの上面

        if ( Mathf.Abs( Mathf.Round(_d_forward.y )) != 1 )
        {
            if(Mathf.Abs(Mathf.Round(_d_right.y )) != 1)
            {
                if(Mathf.Round(_d_up.y ) == 1) diceNumber = 2;
                else                           diceNumber = 5;
                upVector = "y";
            }
            else
            {
                if(Mathf.Round(_d_right.y) == 1) diceNumber = 4;
                else                             diceNumber = 3;
                upVector = "x";
            }
        }
        else
        {
            if(Mathf.Round(_d_forward.y) == 1) diceNumber = 1;
            else                               diceNumber = 6;
            upVector = "z";
        }
    }


    public void MoveToTarget()
    {
        if(!movingTarget)
        {
            movingTarget = true;
            //スタート時間の取得
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        //Debug.Log( diceNumber );
        keep = true;
    }
}
