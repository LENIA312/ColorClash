using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

namespace matsushima
{

    public class UI_Maneger : MonoBehaviour
    {
        /**[ 変数宣言 ]***********************************************************************/


        int state;                                      //現在のステータス [ TITLE , CLEAR , GAMEOVER , GAMENOW ]

        byte nowArea;                                   //現在のエリア

        bool[] displaybool = new bool[5];               //表示非表示
        bool vibration;                                 //バイブフラグ

        [Range(0, 1000000)]
        public int gotCoin;                             //入手したコインの枚数

        public static class define
        {
            public const int TITLE = 0;
            public const int CLEAR = 1;
            public const int GAMEOVER = 2;
            public const int OPTION = 3;
            public const int GAMENOW = 3;
            public const int GAME = 4;
        }                   //define
        public GameObject[] panels = new GameObject[5]; //UI画面配列
        public GameObject areaObject = null;            //現在のエリアを表示するテキスト 
        public GameObject coinObject = null;            //現在のコインを表示するテキスト 

        //キャンバスの座標
        Vector3 canvas = new Vector3(142.5f, 253, 0);

        /**[ StartとUpdate ]***********************************************************************/

        // Start is called before the first frame update
        void Start()
        {
            /*string filePath = Application.dataPath + @"\Resources\state.txt";
            string[] allText1 = File.ReadAllLines(filePath);
            gotCoin = int.Parse("allText1");*/

            init(); //初期化

        }

        // Update is called once per frame
        void Update()
        {


            //Debug.Log(displaybool[define.TITLE]);

            //現在のエリアを表示させる
            Text areaText = areaObject.GetComponent<Text>();
            areaText.text = "LEVEL : " + nowArea;

            //現在のコインを表示させる
            Text coinText = coinObject.GetComponent<Text>();
            if (gotCoin > 999999) {
                coinText.text = "COIN : 999999+";

            } else {
                coinText.text = "COIN : " + gotCoin;
            }


            /***[ Debug ]****/

            if (Input.GetKeyDown(KeyCode.A)) {
                state++;
                if (state == 4) state = 0;
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                clearPanelSwitch();
            }

            //Debug.Log(state);

            /**[ ステータス変更 ]***********************************************************************/

            //タイトル
            if (state == define.TITLE) {
                displaybool[define.TITLE] = true;
                displaybool[define.CLEAR] = false;
                displaybool[define.GAMEOVER] = false;
                displaybool[define.GAME] = false;
            }

            //ゲームクリア
            if (state == define.CLEAR) {
                displaybool[define.TITLE] = false;
                displaybool[define.CLEAR] = true;
                displaybool[define.GAMEOVER] = false;
                displaybool[define.GAME] = false;
            }

            //ゲームオーバー
            if (state == define.GAMEOVER) {
                displaybool[define.TITLE] = false;
                displaybool[define.CLEAR] = false;
                displaybool[define.GAMEOVER] = true;
                displaybool[define.GAME] = false;
            }

            //ゲーム中
            if (state == define.GAMENOW) {
                displaybool[define.TITLE] = false;
                displaybool[define.CLEAR] = false;
                displaybool[define.GAMEOVER] = false;
                displaybool[define.GAME] = true;
            }

            panels[define.TITLE].SetActive(displaybool[define.TITLE]);
            panels[define.CLEAR].SetActive(displaybool[define.CLEAR]);
            panels[define.GAMEOVER].SetActive(displaybool[define.GAMEOVER]);
            panels[define.OPTION].SetActive(displaybool[define.OPTION]);
            panels[define.GAME].SetActive(displaybool[define.GAME]);
        }

        /***[ UIの表示非表示切り替え関数 ]**********************************************************************/

        //タイトルの表示非表示
        public void titlePanelSwitch()
        {

            displaybool[define.TITLE] = displaybool[define.TITLE] ? false : true;

            panels[define.TITLE].SetActive(displaybool[define.TITLE]);

        }

        //クリア画面の表示非表示
        public void clearPanelSwitch()
        {

            displaybool[define.CLEAR] = displaybool[define.CLEAR] ? false : true;

            panels[define.CLEAR].SetActive(displaybool[define.CLEAR]);

        }

        //ゲームオーバー画面の表示非表示
        public void gameoverPanelSwitch()
        {

            displaybool[define.GAMEOVER] = displaybool[define.GAMEOVER] ? false : true;

            panels[define.GAMEOVER].SetActive(displaybool[define.GAMEOVER]);

        }

        //オプション画面の表示非表示
        public void optionPanelSwitch()
        {

            displaybool[define.OPTION] = displaybool[define.OPTION] ? false : true;
            panels[define.OPTION].SetActive(displaybool[define.OPTION]);


            //x : -78
            //x : -212から


        }

        /**[ UIButtonのみで使う関数 ]***********************************************************************/

        //振動のオンオフ切り替え
        public void vibrationSwitch()
        {
            vibration = vibration ? false : true;
            //if (vibration) Handheld.Vibrate();  //振動がオンになったときバイブレーションをかける
        }

        //タイトルへ
        public void backButton()
        {
            state = define.TITLE;
        }

        //リトライ
        public void retryButton()
        {
            state = define.GAMENOW;
        }

        /**[ シーン遷移関数 ]***********************************************************************/

        //ショップシーンに移動
        public void shopButtonDown()
        {
            SceneManager.LoadScene("shopScene");
        }



        /**[ システム?関数 ]************************************************************************/


        /**************************************************************************/
        void init()
        {
            //ステータスの初期化
            state = define.TITLE;    //最初はタイトル

            //表示非表示の初期化
            displaybool[define.TITLE] = true;
            displaybool[define.CLEAR] = false;
            displaybool[define.GAMEOVER] = false;
            displaybool[define.OPTION] = false;
            displaybool[define.GAME] = false;


            //ゲーム開始時のパネル表示
            panels[define.TITLE].SetActive(displaybool[define.TITLE]);
            panels[define.CLEAR].SetActive(displaybool[define.CLEAR]);
            panels[define.GAMEOVER].SetActive(displaybool[define.GAMEOVER]);
            panels[define.OPTION].SetActive(displaybool[define.OPTION]);
            panels[define.GAME].SetActive(displaybool[define.GAME]);

            //現在のエリア(仮)
            nowArea = 105;

            //入手したコインの枚数(仮)
            gotCoin = 0;

            //バイブレーション
            vibration = false;
        } //初期化
    }

}
