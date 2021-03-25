using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// コイン
    /// </summary>
    public class Coin : MonoBehaviour
    {
        #region *[publicメンバ変数]
        // GameObjcet
        public GameObject _ParticleObj; // 消滅時に生成するパーティクル
        #endregion

        #region *[privateメンバ変数]
        // GameObjcet
        GameObject mCameraControllerObj;
        // Vector3
        Vector3 mRot;
        #endregion

        //-----------------------------------------------------------------------------------------
        void Start()
        {
            mRot = this.transform.eulerAngles;
            mCameraControllerObj = GameObject.Find("CameraController");
            StartCoroutine("Scatter");
        }

        //-----------------------------------------------------------------------------------------
        void Update()
        {
            //transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
            transform.eulerAngles = new Vector3(mRot.x, mRot.y, Time.time * -200);
            // 画面外消去
            if(transform.position.z < mCameraControllerObj.transform.position.z - 12 &&
                transform.position.y >= 0) {
                Destroy(this.gameObject);
            }
            // 奈落の底消去
            if (transform.position.y < -200) {
                Destroy(this.gameObject);
            }
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 散らばす処理
        /// </summary>
        IEnumerator Scatter()
        {
            // ランダムに飛ばす
            float rX = Random.Range(-5f, 5f);
            float rY = 5 - Random.Range(-2f, 2f);
            float rZ = 10 - Random.Range(-2f, 2f);

            this.gameObject.GetComponent<Rigidbody>().
                AddForce(new Vector3(rX, rY, rZ), ForceMode.Impulse);

            // 当たり判定を一瞬だけ消す
            CapsuleCollider collider = this.gameObject.GetComponent<CapsuleCollider>();
            collider.enabled = false;
            yield return new WaitForSeconds(0.5f);
            collider.enabled = true;
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 他のコライダーに触れた時に呼び出される処理
        /// </summary>
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player") {
                // コイン加算
                GameManager.instance.AddCoin();
                // SE再生
                AudioManager.instance.SoundPlayClipAtPoint(SETYPE.コイン, this.transform.position, 0.5f);
                // パーティクル生成
                Instantiate(_ParticleObj, this.transform.position, Quaternion.identity);
                // 消去
                Destroy(this.gameObject);
            }
        }
    }

} // namespace
