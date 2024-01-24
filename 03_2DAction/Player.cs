using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region // インスペクターで設定する。
    [Header("移動速度")] public float speed;
    public float jumpSpeed;
    public float jumpMaxHeight;
    public float jumpLimitTime; // 何かに引っかかったとき用
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    public float grabity;

    public GroundCheck ground; // 設置判定用
    public GroundCheck head;   // 頭をぶつけた判定用

    public AnimationCurve dashCurve; // ダッシュの加速度表現
    public AnimationCurve jumpCurve; // ジャンプの加速度表現
    #endregion

    #region // プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capcol = null;

    private bool isGround = false;  // 接地しているか
    private bool isHead   = false;  // 頭が接地しているか
    private bool isJump   = false;  // ジャンプしているか
    private bool isOtherJump = false;  // 踏みつけた時などのジャンプ
    private bool isRun    = false;  // 走っているか
    private bool isDown   = false;  // ダメージを受けてダウンしているか

    private float jumpPos = 0.0f;   // ジャンプした位置。ジャンプできる最高点計算用
    private float otherJumpHeight = 0.0f;  // 踏みつけた時のジャンプ最高点
    private float jumpTime = 0.0f;  // ジャンプ時間。加速度計算用
    private float dashTime = 0.0f;  // ダッシュ時間。加速度計算用

    private float beforeKey = 0.0f;

    private string enemyTag = "Enemy"; // 敵当たり判定用タグ
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if( !isDown ) { 
            // 地面判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();

            // アニメーションの適応
            SetAnimation();

            // 移動速度を設定
            rb.velocity = new Vector2( GetXSpeed(), GetYSpeed() );
        }
        else
        {
            // 移動速度を設定
            rb.velocity = new Vector2( 0, -grabity );
        }
    }


    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        // キー入力されたら行動する
        // 「Input.GetKey("hoge")」でもいいけれど、「Input.GetAxis("hoge")」を使用したほうが、キーバインドを一括で変えやすい
        float _horizontalKey = Input.GetAxis( "Horizontal" );
        float _xSpeed = 0.0f;

        if( _horizontalKey > 0 )
        {   // 左が押された場合
            transform.localScale = new Vector3( 1, 1, 1 ); // 絵の向きを正の向きに変更
            isRun = true;
            dashTime += Time.deltaTime;
            //_xSpeed = _horizontalKey * speed;
            _xSpeed = speed;
        }
        else if( _horizontalKey < 0 )
        {  // 右が押された場合
            transform.localScale = new Vector3( -1, 1, 1 ); // 絵の向きを逆の向きに変更
            isRun = true;
            dashTime += Time.deltaTime;
            //_xSpeed = _horizontalKey * speed;
            _xSpeed = -speed;
        }
        else
        {
            isRun = false;
            _xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        // 前回の入力からダッシュの反転を判断して速度を変える
        if( _horizontalKey > 0 && beforeKey < 0 )
        {
            dashTime = 0.0f;
        }
        else if( _horizontalKey < 0 && beforeKey > 0 )
        {
            dashTime = 0.0f;
        }

        beforeKey = _horizontalKey;

        // アニメーションカーブを速度に適用
        _xSpeed *= dashCurve.Evaluate( dashTime );

        return _xSpeed;
    }


    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()
    {
        // ジャンプ処理
        float _verticalKey = Input.GetAxis( "Vertical" );
        float _ySpeed = -grabity;

        // 何かを踏んだ時
        if( isOtherJump )
        {
            bool canJumpHeight = jumpPos + otherJumpHeight > transform.position.y;    // 現在の高さが飛べる高さより下か
            bool canJumpTime = jumpLimitTime > jumpTime;    // ジャンプ時間が長くなりすぎていないか

            if( canJumpHeight && canJumpTime && !isHead )
            {
                _ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime; // 最後のフレームからの経過時間を加算
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        // 接地していた時のジャンプ処理
        else if( isGround )
        {
            if( _verticalKey > 0 )
            {
                _ySpeed = jumpSpeed;
                jumpPos = transform.position.y; // ジャンプした位置を記録しておく
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        // ジャンプ中の処理
        else if( isJump )
        {
            bool pushUpKey = _verticalKey > 0;  // 上方向キーが押されているか
            bool canJumpHeight = jumpPos + jumpMaxHeight > transform.position.y;    // 現在の高さが飛べる高さより下か
            bool canJumpTime   = jumpLimitTime > jumpTime;    // ジャンプ時間が長くなりすぎていないか

            if( pushUpKey && canJumpHeight && canJumpTime && !isHead )
            {
                _ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime; // 最後のフレームからの経過時間を加算
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        // アニメーションカーブを速度に適用
        if( isJump || isOtherJump ) _ySpeed *= jumpCurve.Evaluate( jumpTime );

        return _ySpeed;
    }

    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("isJump", isJump || isOtherJump);
        anim.SetBool("isGround", isGround);
        anim.SetBool("isRun", isRun);
    }


    #region // 接触判定
    private void OnCollisionEnter2D( Collision2D collision )
    {
        if( collision.collider.tag == enemyTag )
        {
            float _stepOnHeight = ( capcol.size.y * ( stepOnRate / 100f ) );  // 踏みつけ判定になる高さ
            float _judgePos = transform.position.y - ( capcol.size.y / 2f ) + _stepOnHeight;  // 踏みつけ判定のワールド座標

            foreach( ContactPoint2D p in collision.contacts )
            {
                if( p.point.y < _judgePos ) // 敵と衝突した位置が足元だったらもう一度跳ねる
                {
                    ObjectCllision o = collision.gameObject.GetComponent<ObjectCllision>();

                    if( o != null )
                    {
                        otherJumpHeight = o.boundHeight;  // 踏んづけたものから跳ねる高さを取得
                        o.playerStepOn  = true;  // 踏んづけたものに対して踏んづけたことを通知する
                        jumpPos         = transform.position.y;  // 準ぷした位置を記録する
                        
                        isOtherJump = true;
                        isJump      = false;
                        
                        jumpTime = 0.0f;
                    }
                    else
                    {
                        Debug.Log("ObjectCollisionがついていません");
                    }
                }
                else  // ダウンする
                {
                    anim.Play( "PlayerDown" );
                    isDown = true;
                    break;
                }
            }
        }
    }
    #endregion
}
