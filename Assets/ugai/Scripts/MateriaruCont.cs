using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ugai
{

    public class MateriaruCont : MonoBehaviour
    {

        public Material[] _material;           // 割り当てるマテリアル.
        private byte i;

        private Vector3 position_name_Upd;
        private Vector3 position_name_Axis;

        private Vector3 world_position_name_Upd;
        private Vector3 world_position_name_Axis;
        private Vector3 world_position_name_dif;

        private float seconds;

        // Use this for initialization
        void Start()
        {
            i = 0;
            seconds = 0f;
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetMouseButtonDown(0)) {
                // print("いま左ボタンが押された");
                position_name_Axis = Input.mousePosition;
                //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                position_name_Axis.z = 10f;
                //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                world_position_name_Axis = Camera.main.ScreenToWorldPoint(position_name_Axis);

                seconds = 0f;
            }

            if (Input.GetMouseButton(0)) {
                //マウス位置取得(X,Y,Z)
                position_name_Upd = Input.mousePosition;
                //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                position_name_Upd.z = 10f;
                //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                world_position_name_Upd = Camera.main.ScreenToWorldPoint(position_name_Upd);

                world_position_name_dif = world_position_name_Axis - world_position_name_Upd;

                seconds += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0)) {
                if (world_position_name_dif.x <= 0.1 && world_position_name_dif.x >= -0.1 &&
                    world_position_name_dif.y <= 0.1 && world_position_name_dif.y >= -0.1 &&
                    seconds < 0.15f) {
                    i++;
                    if (i == 3) {
                        i = 0;
                    }

                    this.GetComponent<Renderer>().material = _material[i];
                }
            }

        }
    }

}