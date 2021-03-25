using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using kai;

public class StageSelect_Maneger : MonoBehaviour
{
    public GameObject StageNumView = null; // Textオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトからTextコンポーネントを取得
        Text score_text = StageNumView.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text.text = this.transform.name.Substring(7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ステージ選択ボタン
    /// 概要 : スクリプトをアタッチしたボタンの名前に応じて遷移するシーンが変わる
    /// 　　　 ボタンの名前は「Select_番号」としてください
    /// </summary>
    public void StageSelectButton()
    {
        string StageNum = "level_" + transform.name.Substring(7); //ボタンの名前を参照し、番号のみを抽出
        SceneManager.LoadScene(StageNum);
        GameManager.instance.SetLevel(int.Parse(transform.name.Substring(7)));
        GameManager.instance.SetGameState(GAMESTATE.TITLE);
        AudioManager.instance.SoundPlayOneShot(SETYPE.ボタン, 0.8f);
    }
}
