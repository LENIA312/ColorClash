using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ugai
{

    public class BallControllerTest2 : MonoBehaviour
    {
        public float movementSpeed;

        private Vector3 position_name_Upd;
        private Vector3 position_name_Axis;

        private Vector3 world_position_name_Upd;
        private Vector3 world_position_name_Axis;
        private Vector3 world_position_name_dif;

        // 座標を取得
        Vector3 pos;

        void Start()
        {
            pos.x = this.transform.localPosition.x;

            pos.z = this.transform.localPosition.z;
        }

        void Update()
        {
            //this.gameObject.transform.Translate(0, 0, movementSpeed);

            if (Input.GetMouseButtonUp(0)) {
                pos.x = this.transform.localPosition.x;
                world_position_name_dif.x = 0;

                pos.z = this.transform.localPosition.z;
                world_position_name_dif.y = 0;
            }

            if (Input.GetMouseButtonDown(0)) {
                // print("いま左ボタンが押された");
                position_name_Axis = Input.mousePosition;
                //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                position_name_Axis.z = 10f;
                //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                world_position_name_Axis = Camera.main.ScreenToWorldPoint(position_name_Axis);
            }

            if (Input.GetMouseButton(0)) {
                //マウス位置取得(X,Y,Z)
                position_name_Upd = Input.mousePosition;
                //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                position_name_Upd.z = 10f;
                //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                world_position_name_Upd = Camera.main.ScreenToWorldPoint(position_name_Upd);

                world_position_name_dif = world_position_name_Axis - world_position_name_Upd;

                world_position_name_dif.x *= 3f;
                world_position_name_dif.y *= 3f;

            }

            //Debug.Log("A" + world_position_name_Axis);
            //Debug.Log("U" + world_position_name_Upd);
            //Debug.Log("D" + world_position_name_dif);

            //ワールド位置反映(X,Y,Z)
            /*this.transform.position = new Vector3(pos.x - world_position_name_dif.x, this.transform.position.y, pos.z - world_position_name_dif.z);

            if (this.transform.position.x >= 9.5)
            {
                this.transform.position = new Vector3(9.5f, this.transform.position.y, this.transform.position.z);
            }
            else if (this.transform.position.x <= -9.5)
            {
                this.transform.position = new Vector3(-9.5f, this.transform.position.y, this.transform.position.z);
            }*/
            this.transform.localPosition = new Vector3(pos.x - world_position_name_dif.x, this.transform.localPosition.y, pos.z - world_position_name_dif.y);

            if (this.transform.localPosition.x >= 9.5) {
                this.transform.localPosition = new Vector3(9.5f, this.transform.localPosition.y, this.transform.localPosition.z);
            } else if (this.transform.localPosition.x <= -9.5) {
                this.transform.localPosition = new Vector3(-9.5f, this.transform.localPosition.y, this.transform.localPosition.z);
            }
            if (this.transform.localPosition.z <= -13) {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -13f);
            }
        }
    }

}