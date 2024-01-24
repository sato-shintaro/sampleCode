using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPinBall : MonoBehaviour
{
    [SerializeField] private float limitSpeed = 10.0f;
    [SerializeField] private Transform dirLine = null;
    private int  strikeCnt = 0;  // ピンボールを弾いた回数

    private Rigidbody myRigidbody = null;

    private Vector3 defaultPosition = new Vector3();  // 初期位置記録用
    private Vector3 mousePosition   = new Vector3();  // ピンボールを弾く方向を計算するためのマウス位置記録用


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
        myRigidbody.sleepThreshold = 3;  // default：0.005

        defaultPosition   = this.transform.localPosition;

        dirLine.gameObject.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Debug.Log("S：" + myRigidbody.IsSleeping() + "／" + "V：" + myRigidbody.velocity + "／" + "R：" + myRigidbody.velocity.magnitude);
        //UnityEditor.EditorApplication.isPaused = true;
#endif

        // ピンボールが止まっているならピンボールを弾く動作を行える
        if( myRigidbody.IsSleeping() )
        {
            myRigidbody.isKinematic = false;
            PlayBall();
        }
    }


    /// <summary>
    /// ピンボールを弾くためのマウス動作
    /// </summary>
    private void PlayBall()
    {
        // 左クリック開始時
        if( Input.GetMouseButtonDown( 0 ) )
        {
            mousePosition = Input.mousePosition; // クリック開始位置を保管
            dirLine.gameObject.SetActive( true );  // 方向線を表示
            return;
        }

        // 左クリック中
        if( Input.GetMouseButton( 0 ) )
        {
            Vector3 _nowMousePosition = Input.mousePosition; // マウスの現在位置を随時保管

            // 角度を算出
            Vector3 _def = ( mousePosition - _nowMousePosition ).normalized;

            float _rad = Mathf.Atan2( _def.x, _def.y );
            float _angle = _rad * Mathf.Rad2Deg;
            Vector3 _rot = new Vector3( 0, _angle, 0 );
            Quaternion _qua = Quaternion.Euler( _rot );

            // 方向線の位置角度を調整
            dirLine.localRotation = _qua;
        }

        // 左クリック終了時
        if( Input.GetMouseButtonUp( 0 ) )
        {
            Vector3 _upMousePosition = Input.mousePosition;  // マウスの現在位置保管

            // 開始位置と終了位置のベクトル計算から打ち出す方向を算出
            Vector3 _def = mousePosition - _upMousePosition;
            Vector3 _add = new Vector3( _def.x, 0, _def.y );

            while( _add.magnitude > limitSpeed )
            {
                _add = new Vector3( _add.x * 0.9f, _add.y * 0.9f, _add.z * 0.9f );
            }

            myRigidbody.AddForce( _add, ForceMode.Impulse );
            dirLine.gameObject.SetActive( false );

            strikeCnt++;
        }
    }


    private void Stop()
    {
        //myRigidbody.velocity = Vector3.zero;        // 一応移動速度を0に
        //myRigidbody.angularVelocity = Vector3.zero; // 一応回転速度を0に
        //myRigidbody.ResetInertiaTensor();
        myRigidbody.isKinematic = true;
    }


    public void Reset()
    {
        Stop();
        this.transform.localPosition = defaultPosition;  // 初期位置に移動
        strikeCnt = 0;
        myRigidbody.isKinematic = false;
    }
}
