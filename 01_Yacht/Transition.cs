/*
    内容：トランジッションを処理を行う。
          シングルトンで管理する。
    予定：
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public delegate float GetFadeAlpha( float currentTime, float time );


public class Transition : MonoBehaviour
{
    public static Transition mInstance; // シングルトン用のインスタンス

    [SerializeField] private Material mTransitionIn;
    private GetFadeAlpha[] mGetFadeAlpha = {    // フェードインとフェードアウト関数をまとめて関数ポインタっぽくしてみる
        FadeInCalcAlpha,
        FadeOutCalcAlpha
    };
    private string mChangeScene = null;


    private void Awake()
    {
        // シングルトン作成
        if(mInstance == null)
        {
            mInstance = this;
            this.gameObject.SetActive(false); // メモ：「SetActive」は重いらしいので、可能な限りenabledで対応したほうがいいらしい。
            DontDestroyOnLoad(gameObject);    // シーンが移行しても削除させない
        }
    }


    /// <summary>
    /// トランジッション終了後シーンの変更を行う
    /// </summary>
    /// <param name="sceneName">移行するシーン名</param>
    /// <param name="fadeTime">トランジッション時間（秒）</param>
    /// <param name="transitionType">0=フェードイン／1=フェードアウト。デフォルト=0</param>
    public void TransSceneChange(string sceneName, float fadeTime = 1.0f, int transitionType = 0)
    {
        mChangeScene = sceneName;

        // コールルーチン
        StartCoroutine( BeginTransition( fadeTime, transitionType ) );
    }


    public IEnumerator BeginTransition( float fadeTime=1.0f, int transitionType=0 )
    {
        yield return Animation(mTransitionIn, 1, transitionType);

        if ( mChangeScene != null )
        {
            mChangeScene = null;
            //  SceneManager.LoadScene("SceneGame");    // 即時切替
            SceneManager.LoadSceneAsync("SceneGame");   // 非同期切り替え：シーンの読み込みが終了したらシーンの切り替えを行う
            //SceneManager.LoadSceneAsync("SceneGame", LoadSceneMode.Additive);   // 第２引数に「LoadSceneMode.Additive」を指定すると前のシーンを残して次のシーンをロードできる
            //SceneManager.UnloadSceneAsync("SceneTitle");      // 「SceneManager.UnloadSceneAsync」でシーンの削除が行える
                                                                // リソースは削除されないので、必要に応じてResources.UnloadUnusedAssetsで削除する
        }
    }


    /// <summary>
    /// time秒かけてトランジッションを行う
    /// </summary>
    /// <param name="material">トランジッション実行対象</param>
    /// <param name="time">トランジッション時間（秒）</param>
    /// <param name="transitionType">0=フェードイン／1=フェードアウト。デフォルト=0</param>
    /// <returns></returns>
    IEnumerator Animation( Material material, float time, int transitionType=0 )
    {
        GetComponent<Image>().material = material;
        float _current = 0;

        
        while(_current < time)
        {
            material.SetFloat( "_Alpha", mGetFadeAlpha[transitionType](_current, time) );
            yield return new WaitForEndOfFrame();
            _current += Time.deltaTime;
        }

        float _endApha = 1.0f;
        switch( transitionType )
        {
            case 0: _endApha = 1.0f; break; // フェードインの場合
            case 1: _endApha = 0.0f; break; // フェードアウトの場合
        }

        material.SetFloat( "_Alpha", _endApha);
    }


    /// <param name="elapsedTime">経過時間（秒）</param>
    /// <param name="transTime">トランジッションする時間（秒）</param>
    private static float FadeInCalcAlpha( float elapsedTime, float transTime) { return elapsedTime / transTime; }
    /// <param name="elapsedTime">経過時間（秒）</param>
    /// <param name="transTime">トランジッションする時間（秒）</param>
    private static float FadeOutCalcAlpha(float elapsedTime, float transTime) { return transTime - elapsedTime; }
}
