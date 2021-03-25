using System.Collections;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// 障害物本体
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        #region *[publicメンバ変数]
        // GameObject
        public GameObject _ParticleObj; // 破壊時に生成するパーティクル
        public GameObject _CoinObj; // 破壊時に生成するコイン
        public GameObject _StarObj; // 破壊時に生成する無敵アイテム
        // enum
        public MOVETYPE _MoveType; // 行動パターン
        // float
        public float _MoveLength = 1; // 移動距離
        public float _MoveSpeed = 1; // 移動速度
        public float _Offset; // 初期値をずらす
        // int
        [Range(0, 2)]
        public int _ColorNum; // 色の番号
        #endregion

        #region *[privateメンバ変数]
        // Class
        Player mPlayerCs;
        // Vector3
        Vector3 mCenterPos; // 中心位置
        Vector3 mCenterRot; // 中心回転
        // Color
        Color mColor;
        // float
        float mPlayTime; // ステージ開始からの経過時間
        bool mIsPlayingStage; // ステージプレイ中の時にtrueを返す
        // bool
        bool mHasBreaked; // 破壊時にtrueを返す
        #endregion

        //-----------------------------------------------------------------------------------------
        void Start()
        {
            // Class
            mPlayerCs = GameObject.Find("Player").GetComponent<Player>();

            // Vector3
            mCenterPos = this.transform.position;
            mCenterRot = this.transform.eulerAngles;

            // Color
            mColor = GameManager.instance.GetColors()[_ColorNum];

            // bool
            mPlayTime = _Offset;

            // 色変更
            GetComponent<Renderer>().material.color = mColor;
        }

        //-----------------------------------------------------------------------------------------
        void Update()
        {
            switch (_MoveType) {
                case MOVETYPE.左右x:
                    transform.position =
                        new Vector3(
                            mCenterPos.x + Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.y,
                            mCenterPos.z
                            );
                    break;
                case MOVETYPE.上下z:
                    transform.position =
                        new Vector3(
                            mCenterPos.x,
                            mCenterPos.y,
                            mCenterPos.z - Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength
                            );
                    break;
                case MOVETYPE.上下y:
                    transform.position =
                        new Vector3(
                            mCenterPos.x,
                            mCenterPos.y + Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.z
                            );
                    break;
                case MOVETYPE.斜めxz:
                    transform.position = (_MoveLength > 0) ?
                        new Vector3(
                            mCenterPos.x - Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.y,
                            mCenterPos.z - Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength
                            ) :
                        new Vector3(
                            mCenterPos.x - Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.y,
                            mCenterPos.z + Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength
                            );
                    break;
                case MOVETYPE.円運動xz:
                    transform.position =
                        new Vector3(
                            mCenterPos.x + Mathf.Cos(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.y,
                            mCenterPos.z + Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength
                            );
                    break;
                case MOVETYPE.円運動xy:
                    transform.position =
                        new Vector3(
                            mCenterPos.x + Mathf.Cos(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.y + Mathf.Sin(mPlayTime * _MoveSpeed) * _MoveLength,
                            mCenterPos.z
                            );
                    break;
                case MOVETYPE.回転x:
                    transform.eulerAngles =
                        new Vector3(
                            mCenterRot.x + mPlayTime * _MoveSpeed * 30,
                            mCenterRot.y,
                            mCenterRot.z
                            );
                    break;
                case MOVETYPE.回転y:
                    transform.eulerAngles =
                        new Vector3(
                            mCenterRot.x,
                            mCenterRot.y + mPlayTime * _MoveSpeed * 30,
                            mCenterRot.z
                            );
                    break;
                case MOVETYPE.回転z:
                    transform.eulerAngles =
                        new Vector3(
                            mCenterRot.x,
                            mCenterRot.y,
                            mCenterRot.z + mPlayTime * _MoveSpeed * 30
                            );
                    break;
            }
            if (mIsPlayingStage) {
                mPlayTime += Time.deltaTime;
            }

        }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Player") {
                // 色比較
                if (mPlayerCs.GetColorNum() == _ColorNum || mPlayerCs.GetIsMuteki()) {
                    // 破壊
                    if (!mHasBreaked) {
                        mHasBreaked = true;
                        StartCoroutine("DestroyEvent");
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// プレイヤーが同じ色で触れた時に呼び出される処理
        /// </summary>
        /// :
        /// 破壊演出、コンボ数加算、コインを飛ばす
        IEnumerator DestroyEvent()
        {
            // SE再生
            int combo = GameManager.instance.GetComboCount() - 2;
            if(combo < 0) {
                combo = 0;
            }
            if(combo > 7) {
                combo = 7;
            }
            AudioManager.instance.SoundPlayClipAtPoint(SETYPE.ブロック破壊 + combo, this.transform.position);
            AudioManager.instance.SoundPlayClipAtPoint(SETYPE.ブロック破壊 + combo, this.transform.position);
            AudioManager.instance.SoundPlayClipAtPoint(SETYPE.ブロック破壊 + combo, this.transform.position);

            // スケールを保持
            Vector3 tmpScale = this.transform.localScale;

            // 膨れる
            float tmpTime = 0.05f;
            while (tmpTime != 0) {
                this.transform.localScale += new Vector3(10, 10, 10) * Time.deltaTime;
                tmpTime = Mathf.MoveTowards(tmpTime, 0, Time.deltaTime);
                yield return null;
            }

            // 弾けるパーティクル生成（色も変える
            Instantiate(_ParticleObj, this.transform.position, Quaternion.identity).
                GetComponent<Renderer>().material.color = mColor;

            // コンボ数加算
            GameManager gm = GameManager.instance;
            gm.AddComboCount();

            // コイン生成
            combo = gm.GetComboCount();
            int cnt = 0;
            for(int i = 0; i <= combo; i++) {
                if (i % 15 == 0 && i > 0) {
                    cnt++;
                }
            }
            for (int i = 0; i < cnt; i++) {
                Instantiate(_CoinObj, this.transform.position, _CoinObj.transform.rotation);
            }

            // 無敵アイテム生成
            /*
            if (combo % 10 == 0 && combo > 0 && !mPlayerCs.GetIsMuteki()) {
                Instantiate(_StarObj, this.transform.position, _StarObj.transform.rotation);
            }
            */

            this.gameObject.SetActive(false);
            this.transform.localScale = tmpScale;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// _ColorNumberの値を返す
        /// </summary>
        public int GetColorNum()
        {
            return _ColorNum;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージ開始時に呼び出される処理
        /// </summary>
        public void Initialize()
        {
            mPlayTime = _Offset;
            mIsPlayingStage = true;
            mHasBreaked = false;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージの状態をリセットする時に呼び出される処理
        /// </summary>
        public void ResetStage()
        {
            mPlayTime = _Offset;
            mIsPlayingStage = false;
            mHasBreaked = false;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// デバッグ用のギズモ
        /// </summary>
        void OnDrawGizmos()
        {
            // 色の設定
            switch (_ColorNum) {
                case 0:
                    Gizmos.color = Color.cyan;
                    break;
                case 1:
                    Gizmos.color = Color.magenta;
                    break;
                case 2:
                    Gizmos.color = Color.yellow;
                    break;
            }

            // 絶対値Scale
            Vector3 scale = this.transform.localScale;
            scale.x = Mathf.Abs(scale.x) + 0.01f;
            scale.y = Mathf.Abs(scale.y) + 0.01f;
            scale.z = Mathf.Abs(scale.z) + 0.01f;

            // 移動範囲Vector
            Vector3 vector = Vector3.zero;
            float length = Mathf.Abs(_MoveLength);

            switch (_MoveType) {
                case MOVETYPE.左右x:
                    vector.x = length * 2;
                    break;
                case MOVETYPE.上下z:
                    vector.z = length * 2;
                    break;
                case MOVETYPE.上下y:
                    vector.y = length * 2;
                    break;
                case MOVETYPE.斜めxz:
                    vector.x = length;
                    vector.z = length;
                    break;
                case MOVETYPE.円運動xz:
                case MOVETYPE.円運動xy:
                    vector.x = length;
                    vector.y = length;
                    vector.z = length;
                    break;
                case MOVETYPE.回転x:
                    if(scale.y > scale.z) {
                        vector.y = scale.y / 2;
                        vector.z = scale.y / 2;
                    } else {
                        vector.y = scale.z / 2;
                        vector.z = scale.z / 2;
                    }
                    break;
                case MOVETYPE.回転y:
                    if(scale.x > scale.z) {
                        vector.x = scale.x / 2;
                        vector.z = scale.x / 2;
                    } else {
                        vector.x = scale.z / 2;
                        vector.z = scale.z / 2;
                    }
                    break;
                case MOVETYPE.回転z:
                    if(scale.x > scale.y) {
                        vector.x = scale.x / 2;
                        vector.y = scale.x / 2;
                    } else {
                        vector.x = scale.y / 2;
                        vector.y = scale.y / 2;
                    }
                    break;
            }

            // ワイヤー表示
            Vector3 pos = (mCenterPos == Vector3.zero) ? this.transform.position : mCenterPos;
            bool offsetDraw = (mCenterPos == Vector3.zero && _Offset != 0) ? true : false;
            switch (_MoveType) {
                case MOVETYPE.静止:
                    Gizmos.DrawWireCube(pos, scale);
                    break;
                case MOVETYPE.左右x:
                case MOVETYPE.上下z:
                case MOVETYPE.上下y:
                    Gizmos.DrawWireCube(pos, scale + vector);
                    if(offsetDraw) {
                        switch (_MoveType) {
                            case MOVETYPE.左右x:
                                pos.x += Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                                break;
                            case MOVETYPE.上下z:
                                pos.z -= Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                                break;
                            case MOVETYPE.上下y:
                                pos.y += Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                                break;
                        }
                        Gizmos.DrawCube(pos, scale);
                    }
                    break;
                case MOVETYPE.斜めxz:
                    if (_MoveLength < 0) {
                        vector.z *= -1;
                    }
                    Gizmos.DrawWireCube(pos + vector, scale);
                    Gizmos.DrawWireCube(pos - vector, scale);
                    Gizmos.DrawLine(pos + vector, pos - vector);
                    if (offsetDraw) {
                        if (_MoveLength > 0) {
                            pos.x -= Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                            pos.z -= Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                        } else {
                            pos.x -= Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                            pos.z += Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                        }
                        Gizmos.DrawCube(pos, scale);
                    }
                    break;
                case MOVETYPE.円運動xz:
                case MOVETYPE.円運動xy:
                case MOVETYPE.回転x:
                case MOVETYPE.回転y:
                case MOVETYPE.回転z:
                    Vector3 prevPos = Vector3.zero;
                    Vector3 nextPos = Vector3.zero;

                    int vertexCount = 64;

                    float step = 2 * Mathf.PI / vertexCount;
                    float x, y, z;
                    x = y = z = 0;

                    // 線を描く
                    for (int i = 0; i <= vertexCount; i++) {
                        float theta = step * i;

                        switch (_MoveType) {
                            case MOVETYPE.円運動xz:
                            case MOVETYPE.回転y:
                                x = vector.x * Mathf.Cos(theta);
                                z = vector.z * Mathf.Sin(theta);
                                nextPos.x = pos.x + x;
                                nextPos.y = pos.y;
                                nextPos.z = pos.z + z;
                                break;
                            case MOVETYPE.円運動xy:
                            case MOVETYPE.回転z:
                                x = vector.x * Mathf.Cos(theta);
                                y = vector.y * Mathf.Sin(theta);
                                nextPos.x = pos.x + x;
                                nextPos.y = pos.y + y;
                                nextPos.z = pos.z;
                                break;
                            case MOVETYPE.回転x:
                                y = vector.y * Mathf.Cos(theta);
                                z = vector.z * Mathf.Sin(theta);
                                nextPos.x = pos.x;
                                nextPos.y = pos.y + y;
                                nextPos.z = pos.z + z;
                                break;
                        }

                        if (i == 0) {
                            prevPos = nextPos;
                        } else {
                            Gizmos.DrawLine(prevPos, nextPos);
                        }
                        prevPos = nextPos;
                    }

                    if (offsetDraw) {
                        switch (_MoveType) {
                            case MOVETYPE.円運動xz:
                                pos.x += Mathf.Cos(_Offset * _MoveSpeed) * _MoveLength;
                                pos.z += Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                                Gizmos.DrawCube(pos, scale);
                                break;
                            case MOVETYPE.円運動xy:
                                pos.x += Mathf.Cos(_Offset * _MoveSpeed) * _MoveLength;
                                pos.y += Mathf.Sin(_Offset * _MoveSpeed) * _MoveLength;
                                Gizmos.DrawCube(pos, scale);
                                break;
                        }
                    }

                    break;
            }
        }
    }

} // namespace
