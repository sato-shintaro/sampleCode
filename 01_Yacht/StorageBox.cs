using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBox : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)] float mMoveTime     = 0.5f;
    [SerializeField, Range(0.0f, 5.0f)] float mRotationTime = 0.3f;
    [SerializeField, Range(0.0f, 5.0f)] float mShakeTime    = 0.4f;

    [SerializeField] Vector3 mStartPosition = new Vector3(0.8f, 2.0f, 0.0f);
    [SerializeField] Vector3 mEndPosition   = new Vector3(1.5f, 5.0f, 0.0f);

    Quaternion mStartRotation = Quaternion.Euler( 0.0f, 0.0f, 0.0f );
    Quaternion mEndRotation   = Quaternion.Euler( 0.0f, 0.0f, 150.0f );

    [SerializeField] AnimationCurve mMoveCurve;

    private float mMoveStartTime;
    private float mShakeStartTime;

    private bool mIsDiceRoll = false;
    private bool mIsShake    = false;

    private Vector3 mPos;
    private Vector3 mOffsetPos;


    public bool IsDiceRoll{ get => mIsDiceRoll; }


    private void OnEnable()
    {
        if( mMoveTime <= 0)
        {
            this.transform.position = mEndPosition;
            this.transform.rotation = mEndRotation;
            enabled = false;
            return;
        }

        mMoveStartTime = Time.timeSinceLevelLoad;
    }


    // Update is called once per frame
    void Update()
    {
        if (mIsShake)   { Shake(); return; }
        if (mIsDiceRoll){ Roll();  return; }

        if ( Input.GetMouseButtonDown(1) )
        {
            mIsShake = true;

            mPos       = this.transform.position;
            mOffsetPos = Vector3.zero;

            mShakeStartTime = Time.timeSinceLevelLoad;

            return;
        }

        if ( Input.GetMouseButtonDown(0) ){
            mIsDiceRoll = true;
            mMoveStartTime = Time.timeSinceLevelLoad;
        }
     }


    /// <summary>
    /// 箱を揺らす
    /// </summary>
    public void Shake()
    {
        var _diff = Time.timeSinceLevelLoad - mShakeStartTime;

        if ( _diff > mShakeTime)
        {
            this.transform.position = mPos;
            mIsShake = false;
            return;
        }

        // ※：ランダムにベクトル作ってそれで動かすやり方でも良さそう
        var moveRangeMin = -0.1f;
        var moveRangeMax = 0.1f;
        mOffsetPos.x = Random.Range(moveRangeMin, moveRangeMax);
        mOffsetPos.y = Random.Range(moveRangeMin, moveRangeMax);
        mOffsetPos.z = Random.Range(moveRangeMin, moveRangeMax);

        this.transform.position = mPos + mOffsetPos;
    }


    /// <summary>
    /// 箱を傾ける
    /// </summary>
    public void Roll()
    {
        var _diff = Time.timeSinceLevelLoad - mMoveStartTime;

        if (_diff > mMoveTime)
        {
            this.transform.position = mEndPosition;
            this.transform.rotation = mEndRotation;
            enabled = false;
        }

        var _moveRate     = _diff / mMoveTime;
        var _rotationRate = _diff / mRotationTime;
        //var _pos = mMoveCurve.Evaluate(rate);

        this.transform.position = Vector3.Slerp( mStartPosition, mEndPosition, _moveRate);
        //this.transform.position = Vector3.Lerp( mStartPosition, mEndPosition, _pos );

        this.transform.rotation = Quaternion.Slerp(mStartRotation, mEndRotation, _rotationRate);
        //this.transform.rotation = Quaternion.Euler( new Vector3(0.0f, 0.0f, 120.0f));
    }


    /// <summary>
    /// 入れ物を初期状態に戻す
    /// </summary>
    public void Init()
    {
        mIsDiceRoll = false;
        mIsShake    = false;

        this.transform.position = mStartPosition;
        this.transform.rotation = mStartRotation;
    }


    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if( !UnityEditor.EditorApplication.isPlaying || enabled == false)
        {
            //this.transform.position = mStartPosition;
        }

        UnityEditor.Handles.Label( mEndPosition, mEndPosition.ToString() );
        UnityEditor.Handles.Label( mStartPosition, mStartPosition.ToString() );
#endif

        Gizmos.DrawSphere( mEndPosition, 0.1f );
        Gizmos.DrawSphere( mStartPosition, 0.1f );

        Gizmos.DrawLine( mStartPosition, mEndPosition );
    }
}
