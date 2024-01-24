using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    #region // インスペクターで設定する
    [Header("移動速度")] public float speed;
    public float gravity;
    [Header("画面外でも行動させるか")] public bool nonVisible;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;

    public int scorePoint;
    #endregion

    #region
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCllision oc = null;
    private BoxCollider2D col = null;

    private bool rightTleftF = false; // right=true , left=false
    private bool isDead = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCllision>();
        col = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {
        if( !oc.playerStepOn )
        {
            if( sr.isVisible || nonVisible )
            {
                // 障害物に衝突したら移動する向きを反転
                if( checkCollision.isOn )
                {
                    rightTleftF = !rightTleftF;
                }

                // 移動する方向によって、画像を左右反転しながら移動
                int xVector = -1;
                if( rightTleftF )
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2( xVector * speed, -gravity );
            }
            else
            {
                rb.Sleep();
            }
        }
        else
        {
            // プレイヤーに踏まれた時の処理
            if( !isDead )
            {
                anim.Play("EnemyType1Death");
                rb.velocity = new Vector2( 0, -gravity );
                isDead = true;
                col.enabled = false;

                GManager.instance.score += scorePoint;

                #region // 子のオブジェクトにもコライダーを設定している場合はこんな感じらしい
                /*
                Transform t = transform.Find( "ColliderObj" );
                if( t != null )
                {
                    t.gameObject.SetActive( false );
                }
                */
                #endregion

                Destroy( gameObject, 3f );
            }
            else
            {
                // 回転しながら死ぬ
                transform.Rotate( new Vector3(0,0,5) );
            }
        }
    }
}
