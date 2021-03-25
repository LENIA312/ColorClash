using System.Collections;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// カメラ
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region *[publicメンバ変数]
        public float _ScrollSpeed = 6;
        #endregion

        #region *[privateメンバ変数]
        float mScrollSpeed;
        Vector3 mInitialPos;
        Vector3 mInitialRot;

        GameObject mMainCamera;
        #endregion

        //-----------------------------------------------------------------------------------------
        void Start()
        {
            mMainCamera = GameObject.Find("Main Camera");

            mInitialPos = transform.position;
            mInitialRot = mMainCamera.transform.eulerAngles;
        }

        //-----------------------------------------------------------------------------------------
        void Update()
        {
            transform.Translate(0, 0, mScrollSpeed * Time.deltaTime);
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ステージ開始時に呼び出される処理
        /// </summary>
        public void Initialize()
        {
            mScrollSpeed = _ScrollSpeed;
            mMainCamera.transform.localEulerAngles = mInitialRot;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// スピード減衰
        /// </summary>
        /// <param name="aTarget"> 減衰目標値 </param>
        /// <param name="aSpeed"> 減衰速度 </param>
        public void DecaySpeed(float aTarget, float aSpeed)
        {
            StartCoroutine(DoDecaySpeed(aTarget, aSpeed));
        }
        IEnumerator DoDecaySpeed(float aTarget, float aSpeed)
        {
            // スピード減衰
            float time = 0;
            while (mScrollSpeed > aTarget) {
                time += Time.deltaTime * aSpeed;
                mScrollSpeed -= Mathf.Pow(time, 2);
                yield return null;
            }
            mScrollSpeed = aTarget;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// カメラを揺らす
        /// </summary>
        /// <param name="aCount"> 振動回数 </param>
        /// <param name="aMagnitude"> 振動の強さ </param>
        /// <param name="aSpeed"> 振動のスピード </param>
        public void Shake(int aCount, float aMagnitude, float aSpeed)
        {
            StartCoroutine(DoShake(aCount, aMagnitude, aSpeed));
        }

        IEnumerator DoShake(int aCount, float aMagnitude, float aSpeed)
        {
            // 初めにどの方向に揺れるか乱数
            int dirX = Random.Range(0, 2);
            //int dirZ = Random.Range(0, 2);
            dirX = (dirX == 1) ? 1 : -1;
            //dirZ = (dirZ == 1) ? 1 : -1;

            // 揺らす
            Vector3 mainCameraPos = mMainCamera.transform.localPosition;
            Vector3 tmpPos = Vector3.zero;
            int i = 0;
            while (i < aCount) {
                // 揺らす
                while (Mathf.Abs(tmpPos.x) < aMagnitude) {
                    tmpPos.x += dirX * Time.deltaTime * aSpeed;
                    //tmpPos.z += dirZ * Time.deltaTime * aSpeed;
                    mMainCamera.transform.localPosition = mainCameraPos + tmpPos;
                    yield return null;
                }
                // 補正
                tmpPos.x = dirX * aMagnitude;
                //tmpPos.z = dirZ * aMagnitude;
                mMainCamera.transform.localPosition = mainCameraPos + tmpPos;
                // 戻す
                while (Mathf.Abs(tmpPos.x) > 0) {
                    tmpPos.x = Mathf.MoveTowards(tmpPos.x, 0, Time.deltaTime * aSpeed);
                    //tmpPos.z = Mathf.MoveTowards(tmpPos.z, 0, Time.deltaTime * aSpeed);
                    mMainCamera.transform.localPosition = mainCameraPos + tmpPos;
                    yield return null;
                }
                // 補正
                tmpPos.x = 0;
                //tmpPos.z = 0;
                mMainCamera.transform.localPosition = mainCameraPos + tmpPos;
                // 方向を変える
                dirX *= -1;
                //dirZ *= -1;
                // 振動の揺れ軽減
                aMagnitude /= 1.5f;
                // i++
                i++;

                //Debug.Log(aMagnitude);
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 無敵になった瞬間に呼び出される処理
        /// </summary>
        IEnumerator MutekiStart()
        {
            // スピード２倍
            float target = mScrollSpeed * 2;
            mScrollSpeed = target * 2;
            // スピード減衰
            DecaySpeed(target, 2);

            // アングル変更
            float time = 0;
            while (mMainCamera.transform.localEulerAngles.x > 60) {
                time += Time.deltaTime * 2;
                mMainCamera.transform.Rotate(Vector3.left * time);
                if(mScrollSpeed <= _ScrollSpeed) {
                    StartCoroutine("MutekiEnd");
                    yield break;
                }
                yield return null;
            }
            // アングル補正
            Vector3 tmpAngle = mMainCamera.transform.localEulerAngles;
            tmpAngle.x = 60;
            mMainCamera.transform.localEulerAngles = tmpAngle;
            yield return null;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 無敵が終わった瞬間に呼び出される処理
        /// </summary>
        IEnumerator MutekiEnd()
        {
            mScrollSpeed = _ScrollSpeed;

            float nowX = mMainCamera.transform.eulerAngles.x;
            float targetX = mInitialRot.x;
            while (targetX != nowX) {
                mMainCamera.transform.eulerAngles =
                    new Vector3(
                        nowX,
                        mMainCamera.transform.eulerAngles.y,
                        mMainCamera.transform.eulerAngles.z
                        );
                nowX = Mathf.MoveTowards(nowX, targetX, Time.deltaTime * 500);
                if (mScrollSpeed == 0) {
                    mMainCamera.transform.localEulerAngles = mInitialRot;
                    yield break;
                }
                yield return null;
            }
            mMainCamera.transform.localEulerAngles = mInitialRot;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// リセット
        /// </summary>
        public void ResetStage()
        {
            mScrollSpeed = 0;
            this.transform.position = mInitialPos;
            mMainCamera.transform.localEulerAngles = mInitialRot;
        }
    }

} // namespace
