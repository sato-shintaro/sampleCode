using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPinBall : MonoBehaviour
{
    [SerializeField] private float limitSpeed = 10.0f;
    [SerializeField] private Transform dirLine = null;
    private int  strikeCnt = 0;  // �s���{�[����e������

    private Rigidbody myRigidbody = null;

    private Vector3 defaultPosition = new Vector3();  // �����ʒu�L�^�p
    private Vector3 mousePosition   = new Vector3();  // �s���{�[����e���������v�Z���邽�߂̃}�E�X�ʒu�L�^�p


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
        myRigidbody.sleepThreshold = 3;  // default�F0.005

        defaultPosition   = this.transform.localPosition;

        dirLine.gameObject.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Debug.Log("S�F" + myRigidbody.IsSleeping() + "�^" + "V�F" + myRigidbody.velocity + "�^" + "R�F" + myRigidbody.velocity.magnitude);
        //UnityEditor.EditorApplication.isPaused = true;
#endif

        // �s���{�[�����~�܂��Ă���Ȃ�s���{�[����e��������s����
        if( myRigidbody.IsSleeping() )
        {
            myRigidbody.isKinematic = false;
            PlayBall();
        }
    }


    /// <summary>
    /// �s���{�[����e�����߂̃}�E�X����
    /// </summary>
    private void PlayBall()
    {
        // ���N���b�N�J�n��
        if( Input.GetMouseButtonDown( 0 ) )
        {
            mousePosition = Input.mousePosition; // �N���b�N�J�n�ʒu��ۊ�
            dirLine.gameObject.SetActive( true );  // ��������\��
            return;
        }

        // ���N���b�N��
        if( Input.GetMouseButton( 0 ) )
        {
            Vector3 _nowMousePosition = Input.mousePosition; // �}�E�X�̌��݈ʒu�𐏎��ۊ�

            // �p�x���Z�o
            Vector3 _def = ( mousePosition - _nowMousePosition ).normalized;

            float _rad = Mathf.Atan2( _def.x, _def.y );
            float _angle = _rad * Mathf.Rad2Deg;
            Vector3 _rot = new Vector3( 0, _angle, 0 );
            Quaternion _qua = Quaternion.Euler( _rot );

            // �������̈ʒu�p�x�𒲐�
            dirLine.localRotation = _qua;
        }

        // ���N���b�N�I����
        if( Input.GetMouseButtonUp( 0 ) )
        {
            Vector3 _upMousePosition = Input.mousePosition;  // �}�E�X�̌��݈ʒu�ۊ�

            // �J�n�ʒu�ƏI���ʒu�̃x�N�g���v�Z����ł��o���������Z�o
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
        //myRigidbody.velocity = Vector3.zero;        // �ꉞ�ړ����x��0��
        //myRigidbody.angularVelocity = Vector3.zero; // �ꉞ��]���x��0��
        //myRigidbody.ResetInertiaTensor();
        myRigidbody.isKinematic = true;
    }


    public void Reset()
    {
        Stop();
        this.transform.localPosition = defaultPosition;  // �����ʒu�Ɉړ�
        strikeCnt = 0;
        myRigidbody.isKinematic = false;
    }
}
