using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// Star
    /// </summary>
    public class Star : MonoBehaviour
    {
        #region *[publicメンバ変数]
        // GameObject
        public GameObject _ParticleObj;
        #endregion

        #region *[privateメンバ変数]
        // GameObject
        GameObject mCameraControllerObj;
        // Compornent
        Rigidbody mRigidBody;
        #endregion

        //-----------------------------------------------------------------------------------------
        void Start()
        {
            mCameraControllerObj = GameObject.Find("CameraController");

            mRigidBody = GetComponent<Rigidbody>();

            StartCoroutine("Scatter");
        }

        //-----------------------------------------------------------------------------------------
        void Update()
        {
            transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
            // 画面外消去
            if (transform.position.z < mCameraControllerObj.transform.position.z - 12 &&
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
            float rX = Random.Range(-2f, 2f);
            float rY = 10;
            float rZ = 10;

            mRigidBody.AddForce(new Vector3(rX, rY, rZ), ForceMode.Impulse);

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
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Obstcale") {
                mRigidBody.AddForce(Vector3.up * 4, ForceMode.Impulse);
            }
            if (collision.gameObject.tag == "Player") {
                Instantiate(_ParticleObj, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }

} // namespace
