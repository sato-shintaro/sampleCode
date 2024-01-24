using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMain : MonoBehaviour
{
    [Header("フェード")] public FadeManager fade;

    private int  pressButton = -1;
    private bool sceneChange = false;


    // Update is called once per frame
    void Update()
    {
        if( !sceneChange && fade.IsFadeOutComplete() )
        {
            // ゲームシーンへ
            SceneManager.LoadScene("GameScene");
            sceneChange = true;
        }
    }


    public void PressStartButton()
    {
        Debug.Log( "Press Start" );
        if( pressButton == -1 )
        {
            Debug.Log( "Go Next Scene" );
            fade.StartFadeOut();
            pressButton = 1; // ※※※シーンごとにEnumとかで番号を作る
        }
    }
}
