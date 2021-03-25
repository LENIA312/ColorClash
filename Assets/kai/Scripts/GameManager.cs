using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//-------------------------------------------------------------------------------------------------
namespace kai
{
    
    /// <summary>
    /// ゲーム全体を管理するスクリプト
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // Singleton
        public static GameManager instance = null;

        #region *[publicメンバ変数]
        // GameObject
        public GameObject[] _UI_Objs;
        // Color
        public Color[] _Colors = new Color[3];
        // float
        public float _ComboTime = 2;
        // bool
        public bool _TestPlay; // テストプレイフラグ
        #endregion

        #region *[privateメンバ変数]
        // GameObject
        GameObject mCameraControllerObj;
        GameObject mPlayerObj;
        GameObject[] mObstacleObjs;
        GameObject mCanvas;
        GameObject mFlashEfectObj;
        GameObject mStarLineObj;

        // Compornent
        TextMesh mLevelNumTextMesh;

        // Vector3
        Vector3 mGoalLinePos;
        Vector3 mInitialStartLinePos;

        // Class
        //SaveData mSaveData = null;
        CameraController mCameraControllerCs;
        Player mPlayerCs;
        UI_Manager mUI_ManagerCs = null;
        AudioManager mAudioManager;

        // enum
        GAMESTATE mGameState;

        // int
        int mCoin = 0; // コイン
        int mLevel = 0; // 現在のレベル
        int mComboCount; // コンボ数
        #endregion

        #region *[Awake / Start / Update 関数]
        //-----------------------------------------------------------------------------------------
        void Awake()
        {
            // Singleton
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this.gameObject);
            }

            // セーブデータをロード
            /*
            if (!_TestPlay) {
                mSaveData = new SaveData();
                mSaveData.LoadFile();

                mLevel = mSaveData.LEVEL;
                mCoin = mSaveData.COIN;
            }
            */
        }

        //-----------------------------------------------------------------------------------------
        // Start is called before the first frame update
        void Start()
        {
            // GAMESTATEをTITLE切り替える
            mGameState = GAMESTATE.TITLE;

            // GameObjcet取得
            mCanvas = GameObject.Find("Canvas");
            mFlashEfectObj = (GameObject)Resources.Load("Prefabs/FlashEfect");

            // イベントにイベントハンドラーを追加
            SceneManager.sceneLoaded += SceneLoaded;

            // テストプレイでないならばシーンを移動
            if (!_TestPlay) {
                string nextLevel = "level_" + mLevel;
                SceneManager.LoadScene(nextLevel);
            } else {
                GetStageObjects();
            }

            // UI_Manegerクラスの生成
            mUI_ManagerCs = new UI_Manager();

            // AudioManager
            mAudioManager = AudioManager.instance;
        }

        //-----------------------------------------------------------------------------------------
        // Update is called once per frame
        void Update()
        {
            // 常時
            mUI_ManagerCs.CoinUI_Update();

            // シーン毎
            switch (mGameState) {
                case GAMESTATE.TITLE:
                    mUI_ManagerCs.TitleUI_Update();
                    break;
                case GAMESTATE.PLAYING:
                    mUI_ManagerCs.PlayingUI_Update();
                    mUI_ManagerCs.ComboUI_Update();
                    break;
                case GAMESTATE.GAMEOVER:
                    mUI_ManagerCs.GameOverUI_Update();
                    break;
                case GAMESTATE.STAGECLEAR:
                    mUI_ManagerCs.StageClearUI_Update();
                    break;
            }
        }
        #endregion

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// シーンロード完了時に呼び出される処理
        /// </summary>
        void SceneLoaded(Scene aNextScene, LoadSceneMode aMode)
        {
            Debug.Log("【SCENE/LOADED】NEXT:" + aNextScene.name + "/MODE:" + aMode);
            // ステージオブジェクトを検索
            if (mGameState != GAMESTATE.PLAYING) {
                GetStageObjects();
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージのオブジェクトを検索する
        /// </summary>
        void GetStageObjects()
        {
            // GameObject
            mCameraControllerObj = GameObject.Find("CameraController");
            mPlayerObj = GameObject.Find("Player");
            mStarLineObj = GameObject.Find("StartLine");
            mObstacleObjs = GameObject.FindGameObjectsWithTag("Obstacle");

            // Vector
            mGoalLinePos = GameObject.Find("GoalLine").transform.position;
            mInitialStartLinePos = mStarLineObj.transform.position;

            // TextMesh
            mLevelNumTextMesh = GameObject.Find("LevelNumTextMesh").GetComponent<TextMesh>();
            if (!_TestPlay) {
                mLevelNumTextMesh.text = (mLevel > 0) ? "LEVEL : " + mLevel : "TUTORIAL";
            } else {
                mLevelNumTextMesh.text = "TEST PLAY";
            }

            // Script
            mCameraControllerCs = mCameraControllerObj.GetComponent<CameraController>();
            mPlayerCs = mPlayerObj.GetComponent<Player>();
        }

        #region *[ゲームオーバー処理]
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ゲームオーバー時に呼び出される処理
        /// </summary>
        public void GameOverEvent()
        {
            mGameState = GAMESTATE.GAMEOVER;

            // ゲームオーバー時の演出
            StartCoroutine("GameOverDirecting");
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ゲームオーバーの演出
        /// </summary>
        IEnumerator GameOverDirecting()
        {
            // フラッシュさせる
            Instantiate(mFlashEfectObj, Vector3.zero, Quaternion.identity).
                transform.SetParent(mCanvas.transform, false);

            // 振動させる
            //Handheld.Vibrate();

            // カメラ演出
            mCameraControllerCs.DecaySpeed(0, 0.25f);
            mCameraControllerCs.Shake(10, 1, 50);

            // 時を遅くして
            Time.timeScale = 0.25f;

            // 少し待って
            float waitTime = 0;
            while (waitTime < 0.1f) {
                waitTime += Time.deltaTime;
                yield return null;
            }

            // 段々戻る
            while (Time.timeScale != 1) {
                Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, Time.deltaTime * 2);
                yield return null;
            }
            Time.timeScale = 1;
        }
        #endregion

        #region *[ステージクリア処理]
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージクリア時に呼び出される処理
        /// </summary>
        public void StageClearEvent()
        {
            mGameState = GAMESTATE.STAGECLEAR;

            // SE再生
            AudioManager.instance.SoundPlayOneShot(SETYPE.ダメージ, 0.15f);

            // 演出
            StartCoroutine("StageClearDirecting");

            if (!_TestPlay) {
                // 次のレベルへ
                mLevel++;
                // データをセーブ
                /*
                mSaveData.LEVEL = mLevel;
                mSaveData.COIN = mCoin;
                mSaveData.SaveFile();
                */
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージクリアの演出
        /// </summary>
        IEnumerator StageClearDirecting()
        {
            // フラッシュさせる
            Instantiate(mFlashEfectObj, Vector3.zero, Quaternion.identity).
                transform.SetParent(mCanvas.transform, false);

            // 振動させる
            //Handheld.Vibrate();

            // カメラスクロール解除
            mCameraControllerCs.DecaySpeed(0, 0.1f);

            // 時を遅くして
            Time.timeScale = 0.25f;

            // 少し待って
            float waitTime = 0;
            while (waitTime < 0.1f) {
                waitTime += Time.deltaTime;
                yield return null;
            }

            // 段々戻る
            while (Time.timeScale != 1) {
                Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, Time.deltaTime * 2);
                yield return null;
            }
            Time.timeScale = 1;
        }
        #endregion

        #region *[UIボタン処理]
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// タイトル画面から、Tap to Startされた時に呼び出される処理
        /// </summary>
        public void Tap_to_StartButtonDown()
        {
            mGameState = GAMESTATE.PLAYING;

            // SE再生
            AudioManager.instance.SoundPlayOneShot(SETYPE.ボタン, 0.8f);

            // 数値を初期化
            Time.timeScale = 1;
            mComboCount = 0;

            // 各オブジェクトのInitialize関数を呼び出す
            mCameraControllerCs.Initialize();
            mPlayerCs.Initialize();
            foreach (GameObject obj in mObstacleObjs) {
                obj.GetComponent<Obstacle>().Initialize();
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ゲームオーバー画面から、
        /// タイトル画面に戻るボタンを押した時に呼び出される処理
        /// </summary>
        public void BackButtonDown()
        {
            mGameState = GAMESTATE.TITLE;

            // SE再生
            mAudioManager.SoundPlayOneShot(SETYPE.ボタン, 0.8f);

            // 数値初期化
            Time.timeScale = 1;
            mComboCount = 0;

            if (!_TestPlay) {
                /*
                mCoin = mSaveData.COIN;
                */
            } else {
                mCoin = 0;
            }

            // ステージの状態をリセット
            ResetStage();
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ゲームオーバー画面から、リトライボタンを押した時に呼び出される処理
        /// </summary>
        public void RetryButtonDown()
        {
            mGameState = GAMESTATE.TITLE;

            // SE再生
            mAudioManager.SoundPlayOneShot(SETYPE.ボタン, 0.8f);

            // 数値初期化
            Time.timeScale = 1;
            mComboCount = 0;

            // リトライ位置指定
            Vector3 retryPos = mCameraControllerObj.transform.position;
            if (mGoalLinePos.z / 2 > retryPos.z) {
                retryPos.z = mGoalLinePos.z / 2;
            }

            // ステージの状態をリセット
            ResetStage();

            // リトライ位置まで移動
            mCameraControllerObj.transform.position = retryPos;

            // リトライ位置より後ろの障害物を消す
            foreach (GameObject obj in mObstacleObjs) {
                if (retryPos.z + 5 > obj.transform.position.z) {
                    obj.SetActive(false);
                }
            }

            // スタートラインの位置を移動
            Vector3 retryStartLinePos = mInitialStartLinePos;
            retryStartLinePos.z = retryPos.z;
            mStarLineObj.transform.position = retryStartLinePos;

            // ステージ進行度の%を表示
            float percent = (retryPos.z / mGoalLinePos.z) * 100;
            mLevelNumTextMesh.text = (int)percent + "%";
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージクリア画面から、
        /// ネクストステージボタンを押した時に呼び出される処理
        /// </summary>
        public void NextLevelButtonDown()
        {
            mGameState = GAMESTATE.TITLE;

            // SE再生
            mAudioManager.SoundPlayOneShot(SETYPE.ボタン, 0.8f);

            // 数値初期化
            Time.timeScale = 1;
            mComboCount = 0;

            // ステージの状態をリセット
            ResetStage();

            if (!_TestPlay) {
                // 次のレベルのシーンへ移動
                string nextLevel = "level_" + mLevel;
                SceneManager.LoadScene(nextLevel);
            } else {
                mCoin = 0;
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// タイトル画面からステージセレクト画面に飛ぶボタン
        /// </summary>
        public void StageSelectButton()
        {
            mGameState = GAMESTATE.PLAYING;

            // SE再生
            mAudioManager.SoundPlayOneShot(SETYPE.ボタン, 0.8f);

            SceneManager.LoadScene("StageSelectScene");
        }
        #endregion

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージの状態をリセットする処理
        /// </summary>
        void ResetStage()
        {
            // 不要オブジェクトの消去
            Destroy(GameObject.Find("BallDebris(Clone)"));
            GameObject[] coinObjs = GameObject.FindGameObjectsWithTag("Coin");
            foreach (GameObject obj in coinObjs) {
                Destroy(obj);
            }
            GameObject[] starObjs = GameObject.FindGameObjectsWithTag("Star");
            foreach (GameObject obj in starObjs) {
                Destroy(obj);
            }

            // カメラコントローラーの状態をリセット
            mCameraControllerCs.ResetStage();

            // プレイヤーの状態をリセット
            mPlayerObj.SetActive(true);
            mPlayerObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mPlayerCs.ResetStage();

            // 障害物の状態をリセット
            if (mObstacleObjs != null) {
                foreach (GameObject obj in mObstacleObjs) {
                    obj.SetActive(true);
                    obj.GetComponent<Obstacle>().ResetStage();
                }
            }

            // スタートラインを初期化
            mStarLineObj.transform.position = mInitialStartLinePos;
            if (!_TestPlay) {
                mLevelNumTextMesh.text = (mLevel > 0) ? "LEVEL : " + mLevel : "TUTORIAL";
            } else {
                mLevelNumTextMesh.text = "TEST PLAY";
            }
        }

        #region *[Set関数]
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// mCoin加算
        /// </summary>
        public void AddCoin(int aCoin = 1)
        {
            mCoin += aCoin;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// コンボ数を加算
        /// </summary>
        public void AddComboCount()
        {
            mComboCount++;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// コンボ数をリセット
        /// </summary>
        public void ResetComboCount()
        {
            mComboCount = 0;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// レベルをセット
        /// </summary>
        public void SetLevel(int aLevel)
        {
            mLevel = aLevel;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// GameStateをセット
        /// </summary>
        public void SetGameState(GAMESTATE aGAMESTATE)
        {
            mGameState = aGAMESTATE;
        }
        #endregion

        #region *[Get関数]
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// mCoinを返す
        /// </summary>
        /// <returns></returns>
        public int GetCoin()
        {
            return mCoin;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 現在のステージ番号を返す
        /// </summary>
        /// <returns></returns>
        public int GetStageNum()
        {
            return mLevel;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// コンボ数を返す
        /// </summary>
        /// <returns></returns>
        public int GetComboCount()
        {
            return mComboCount;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 現在のシーンを返す
        /// </summary>
        /// <returns></returns>
        public GAMESTATE GetGameState()
        {
            return mGameState;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// UIを返す
        /// </summary>
        /// <returns></returns>
        public GameObject[] GetUI_Obj()
        {
            return _UI_Objs;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 色を返す
        /// </summary>
        /// <returns></returns>
        public Color[] GetColors()
        {
            return _Colors;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// コンボ持続時間を返す
        /// </summary>
        /// <returns></returns>
        public float GetComboTime()
        {
            return _ComboTime;
        }
        #endregion
    }

} // namespace
