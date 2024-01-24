/*
    内容：タイトル画面処理
    予定：元々 activeSerlf が false のものを他のファイルから true にできない？
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // 左クリックを押されたらゲーム画面へシーン遷移
        if(Input.GetMouseButtonDown( 0 ))   SceneChange();
    }


    // シーンの切り替え
    void SceneChange()
    {
        //GameObject _transition = GameObject.Find("ImageTransition");  // メモ：「GameObject.Find」はアクティブ状態のものしか検索できないことに注意
        GameObject _transition = Transition.mInstance.gameObject;
        
        _transition.SetActive( true );
        _transition.GetComponent<Transition>().TransSceneChange("SceneGame", 1.0f);
    }
}
