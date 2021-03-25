using System.Collections;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// プレイヤーの操作
    /// </summary>
    public class Player : MonoBehaviour
    {
        GameManager mGameManager;
        AudioManager mAudioManager;

        #region *[public変数]
        // GameObject
        public GameObject _ColorChangePtcObj; // 色変え時に生成するパーティクル        
        public GameObject _MutekiPtcObj; // 無敵時に生成するパーティクル
        public GameObject _SpeedLinePtcObj; // 無敵時に生成する集中線パーティクル        
        public GameObject _PlayerDebrisObj; // ミス時に生成する破片

        // Material
        public Material[] _Materials = new Material[2]; // 使用するマテリアル

        // float
        public float _MoveSpeed = 1; // 移動の感度
        #endregion

        #region *[privateメンバ変数]
        // GameObject
        GameObject mGoalLineObj;
        GameObject mCameraControllerObj;

        // Component
        Renderer mRenderer;
        Rigidbody mRigidbody;
        Light mLight;
        Camera mTouchCamera;

        // Vector3        
        Vector3 mInitialPos; // 初期位置
        Vector3 mInitalScale; // 初期サイズ
        Vector3 mLastPos; // 最後にタップを離した位置
        Vector3 mDragDistance; // startPosとnowPosの距離

        // Vector2
        Vector3 mStartPos; // タップした瞬間の指の位置
        Vector3 mNowPos; // 現在タップしている指の位置

        // Color
        Color[] mColors;

        // Script
        CameraController mCameraControllerCs;

        // float
        float mTappingTimer; // タップし続けている時の経過時間

        // int        
        int mColorNum; // 現在の色        
        int mTapCount; // タップ数

        // bool
        bool mIsPlayingStage; // ステージプレイ中にtrueを返す
        bool mIsMuteki; // 無敵中にtrueを返す
        #endregion

        #region *[UnityEngine関数]
        //-----------------------------------------------------------------------------------------
        void Start()
        {
            mGameManager = GameManager.instance;
            mAudioManager = AudioManager.instance;

            mColorNum = 0;

            // Vector
            mInitialPos = transform.localPosition;
            mInitalScale = transform.localScale;
            mLastPos = mInitialPos;

            // Color
            mColors = mGameManager.GetColors();

            // GameObject
            mGoalLineObj = GameObject.Find("GoalLine");
            mCameraControllerObj = GameObject.Find("CameraController");

             // Script
             mCameraControllerCs = mCameraControllerObj.GetComponent<CameraController>();

            // Compornent
            mRenderer = GetComponent<Renderer>();
            mRigidbody = GetComponent<Rigidbody>();
            mLight = GetComponent<Light>();
            mTouchCamera = GameObject.Find("TouchCamera").GetComponent<Camera>();

            mRenderer.material = _Materials[0];
            mRenderer.material.color = mColors[mColorNum];
        }

        //-----------------------------------------------------------------------------------------
        void Update()
        {
            if (mIsPlayingStage) {
                ChangeColorTap();
                MoveDrag();
                if(this.transform.position.z > mGoalLineObj.transform.position.z) {
                    // ステージクリア
                    mIsMuteki = false;
                    mIsPlayingStage = false;
                    mGameManager.StageClearEvent();
                    mRigidbody.AddForce(0, 10, 20, ForceMode.Impulse);
                    Instantiate(_MutekiPtcObj, this.transform.position, Quaternion.identity);
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        void OnCollisionEnter(Collision collision)
        {
            if (mIsPlayingStage) {
                // 障害物
                if (collision.gameObject.tag == "Obstacle") {
                    Obstacle obstacleCs = collision.gameObject.GetComponent<Obstacle>();
                    // 色の比較
                    if (obstacleCs.GetColorNum() != mColorNum && !mIsMuteki) {
                        // ゲームオーバー
                        GameOverEvent();
                    } else {
                        // カメラを揺らす
                        mCameraControllerCs.Shake(1, 0.25f, 10);
                        // 10コンボなら無敵
                        if (!mIsMuteki && mGameManager.GetComboCount() % 10 == 0 && mGameManager.GetComboCount() > 0) {
                            StartCoroutine("Muteki");
                        }
                    }
                }
                // スター
                if (collision.gameObject.tag == "Star") {
                    if (!mIsMuteki) {
                        // 無敵
                        StartCoroutine("Muteki");
                    } else {
                        // 10コイン
                        mAudioManager.SoundPlayClipAtPoint(SETYPE.コイン, this.transform.position);
                        mGameManager.AddCoin(10);
                    }
                }
            }
        }
        #endregion

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ドラッグで移動
        /// </summary>
        void MoveDrag()
        {
            // タップした瞬間
            if (Input.GetMouseButtonDown(0) && Input.touchCount <= 1) {
                mStartPos = GetMousePosition();
            }
            // タップ中
            else if (Input.GetMouseButton(0) && Input.touchCount <= 1 && mTapCount > 0) {
                mNowPos = GetMousePosition();
                mDragDistance.x = mNowPos.x - mStartPos.x;
                mDragDistance.z = mNowPos.y - mStartPos.y;
            }
            // タップを離した瞬間
            else if (Input.GetMouseButtonUp(0)) {
                mLastPos = transform.localPosition;
                mDragDistance.x = 0;
                mDragDistance.z = 0;
            }

            // ドラッグした距離だけ移動
            transform.localPosition = mLastPos - (mDragDistance * _MoveSpeed);

            // 移動制限
            Vector3 playerPos = transform.localPosition;
            playerPos.x = Mathf.Clamp(playerPos.x, -5.25f, 5.25f);
            playerPos.y = 0;
            playerPos.z = Mathf.Clamp(playerPos.z, -10, 14);
            transform.localPosition = playerPos;

            // デバッグ
            //Debug.Log(mStartPos + "/" + mNowPos);
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// マウス座標をワールド座標に変換して取得
        /// </summary>
        /// <returns>マウスのワールド座標</returns>
        Vector3 GetMousePosition()
        {
            // マウスから取得できないz座標を補完する
            Vector3 position = Input.mousePosition;
            position.z = mTouchCamera.transform.position.z;
            position = mTouchCamera.ScreenToWorldPoint(position);
            position.z = 0;
            return position;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// タップで色を変える
        /// </summary>
        void ChangeColorTap()
        {
            // タップ中
            if (Input.GetMouseButton(0)) {
                mTappingTimer += Time.deltaTime;
            }
            // 指を離した瞬間
            else if (Input.GetMouseButtonUp(0)) {
                mTapCount++;
                if (mTappingTimer <= 0.15 && mDragDistance.magnitude <= 20 && mTapCount > 1 && !mIsMuteki) {
                    StartCoroutine("ChangeColor");
                }
                mTappingTimer = 0;
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージ開始時に呼び出される処理
        /// </summary>
        public void Initialize()
        {
            mIsPlayingStage = true;

            mTapCount = 0;
            transform.GetChild(0).GetComponent<TrailRenderer>().time = 0.75f;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// リセット
        /// </summary>
        public void ResetStage()
        {
            mIsPlayingStage = false;
            mIsMuteki = false;

            mTapCount = 0;
            mColorNum = 0;

            mStartPos = Vector2.zero;
            mNowPos = Vector2.zero;
            mLastPos = mInitialPos;
            mDragDistance = Vector3.zero;

            mRenderer.material = _Materials[0];
            mRenderer.material.color = mColors[mColorNum];

            transform.GetChild(0).GetComponent<TrailRenderer>().time = 0;
            transform.localScale = mInitalScale;
            transform.localPosition = mInitialPos;

            mLight.enabled = false;

            GameObject gameObject = GameObject.Find("SpeedLineParticle(Clone)");
            Destroy(gameObject);
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ゲームオーバーになった瞬間に呼び出される処理
        /// </summary>
        void GameOverEvent()
        {
            // SE再生
            mAudioManager.SoundPlayClipAtPoint(SETYPE.ダメージ, this.transform.position);

            // ステージプレイ中フラグをオフに
            mIsPlayingStage = false;

            // スケールをリセット
            this.transform.localScale = mInitalScale;

            // 破片を生成
            Transform debrisChildren =
                Instantiate(_PlayerDebrisObj, this.transform.position, Quaternion.identity).transform;
            foreach (Transform debries in debrisChildren) {
                debries.GetComponent<MeshRenderer>().material.color = mColors[mColorNum];
            }

            // 座標のリセット
            mLastPos = new Vector3(0, 0, -7);
            mDragDistance = new Vector3(0, 0, 0);

            // 軌跡を消す
            transform.GetChild(0).GetComponent<TrailRenderer>().time = 0;

            // GameManagerのGameOverイベントを実行
            GameManager.instance.GameOverEvent();

            // 非アクティブに
            this.gameObject.SetActive(false);
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 色を変える瞬間に呼び出される処理
        /// </summary>
        IEnumerator ChangeColor()
        {
            // SE再生
            mAudioManager.SoundPlayClipAtPoint(SETYPE.色変え, this.transform.position);

            // 色の変更
            mColorNum = (mColorNum < 2) ? mColorNum + 1 : 0;
            mRenderer.material.color = mColors[mColorNum];

            // パーティクル
            ParticleSystem.MainModule main =
                Instantiate(_ColorChangePtcObj, this.transform.position, Quaternion.identity).
                GetComponent<ParticleSystem>().main;
            // main.startColor = mColors[mColorNum];

            // 一瞬だけ膨らませる
            Vector3 tmpScale = this.transform.localScale;
            float hukuramu = 0.5f;
            while (hukuramu > 0) {
                this.transform.localScale =
                    new Vector3(
                        tmpScale.x + hukuramu,
                        tmpScale.y + hukuramu,
                        tmpScale.z + hukuramu
                        );
                hukuramu = Mathf.MoveTowards(hukuramu, 0, Time.deltaTime * 10);
                yield return null;
            }
            // 戻す
            this.transform.localScale = tmpScale;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 無敵になる瞬間に呼び出される処理
        /// </summary>
        IEnumerator Muteki()
        {
            // SE再生
            mAudioManager.SoundPlayClipAtPoint(SETYPE.パワーアップ, this.transform.position);

            // 色を保持
            int tmpColor = mColorNum;

            // bool
            mIsMuteki = true;
            mLight.enabled = true;

            // Material
            mRenderer.material = _Materials[1];

            // Particle
            Instantiate(_MutekiPtcObj, this.transform.position, Quaternion.identity);
            GameObject obj =
                Instantiate(_SpeedLinePtcObj, Vector3.zero, _SpeedLinePtcObj.transform.rotation);
            obj.transform.SetParent(mCameraControllerObj.transform, false);
            obj.transform.localPosition = new Vector3(0, 0, 50);

            // Camera
            mCameraControllerCs.StartCoroutine("MutekiStart");
            //mCameraControllerCs.Shake(1, 1, 20);

            // 待つ
            float[] timeBlinks =
                { 2f, 1.8f, 1.6f, 1.4f, 1.2f, 1.0f,
                0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f,
                0.3f, 0.2f, 0.1f, 0.05f };
            float wait = 5;
            int i = 0;
            while (wait > 0) {
                wait -= Time.deltaTime;
                // 点滅
                if (wait < timeBlinks[i]) {
                    mAudioManager.SoundPlayClipAtPoint(SETYPE.色変え, this.transform.position, 0.5f);
                    if (!mLight.enabled) {
                        mLight.enabled = true;
                        mRenderer.material = _Materials[1];
                    } else {
                        mLight.enabled = false;
                        mRenderer.material = _Materials[0];
                        mRenderer.material.color = mColors[mColorNum];
                    }
                    i++;
                }
                // ループ中断
                if (i >= timeBlinks.Length) {
                    break;
                }
                if (!mIsMuteki) {
                    break;
                }
                yield return null;
            }

            // 無敵解除
            {
                // SE再生
                mAudioManager.SoundPlayClipAtPoint(SETYPE.パワーアップ, this.transform.position, 0.1f);
                // 色戻す
                mColorNum = tmpColor;
                // bool
                mIsMuteki = false;
                mLight.enabled = false;
                // Renderer
                mRenderer.material = _Materials[0];
                // Particle
                Instantiate(_MutekiPtcObj, this.transform.position, Quaternion.identity);
                Destroy(obj);
                // Camera
                mCameraControllerCs.StartCoroutine("MutekiEnd");
                // 色の変更
                mRenderer.material.color = mColors[mColorNum];
            }
        }

#region *[Get関数]
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// _ColorNumberの値を返す
        /// </summary>
        public int GetColorNum()
        {
            return mColorNum;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// mIsMutekiを返す
        /// </summary>
        public bool GetIsMuteki()
        {
            return mIsMuteki;
        }
#endregion
    }

} // namespace
