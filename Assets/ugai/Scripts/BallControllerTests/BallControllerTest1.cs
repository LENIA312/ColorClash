using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ugai
{

    public class BallControllerTest1 : MonoBehaviour
    {
        public float movementSpeed;
        // private Vector3 position_name;
        // private Vector3 world_position_name;

        private Vector3 position_name_Upd;
        private Vector3 position_name_Axis;

        private Vector3 world_position_name_Upd;
        private Vector3 world_position_name_Axis;
        private Vector3 world_position_name_dif;

        // 座標を取得
        Vector3 pos;

        // Start is called before the first frame update
        void Start()
        {
            pos.x = this.transform.position.x;
        }

        // Update is called once per frame
        /*  void Update()
          {
              if (Input.GetKey(KeyCode.W))
              {
                  transform.position += transform.forward * movementSpeed;
              }
              if (Input.GetKey(KeyCode.S))
              {
                  transform.position -= transform.forward * movementSpeed;
              }

          }*/
        /* void Update()
         {
             this.gameObject.transform.Translate(0, 0, movementSpeed);

             if (Input.GetKey(KeyCode.D))
             {
                 this.gameObject.transform.Translate(movementSpeed, 0, 0);
             }
             if (Input.GetKey(KeyCode.A))
             {
                 this.gameObject.transform.Translate(movementSpeed * -1, 0, 0);
             }


             //マウス位置取得(X,Y,Z)
             position_name_Upd = Input.mousePosition;

             if (Input.GetMouseButtonDown(0))
             {
                 // print("いま左ボタンが押された");
                 position_name_Axis = Input.mousePosition;
             }

             if (Input.GetMouseButton(0))
             {
                 //print("左ボタンが押されている");
                 if(position_name_Upd.x < position_name_Axis.x)
                 {
                     this.gameObject.transform.Translate(movementSpeed * -1, 0, 0);
                 }
                 else if (position_name_Upd.x > position_name_Axis.x)
                 {
                     this.gameObject.transform.Translate(movementSpeed, 0, 0);
                 }


             }

         }*/

        //無限ループ
        /*void Update()
        {
            this.gameObject.transform.Translate(0, 0, movementSpeed);

            // transformを取得
            Transform myTransform = this.transform;
            // 座標を取得
            Vector3 pos = myTransform.position;

            if (Input.GetMouseButton(0))
            {
                //マウス位置取得(X,Y,Z)
                position_name = Input.mousePosition;
                //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                position_name.z = 10f;
                //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                world_position_name = Camera.main.ScreenToWorldPoint(position_name);

                world_position_name.x *= 3f;

                if (world_position_name.x >= 9.5)
                {
                    world_position_name.x = 9.5f;
                }else if (world_position_name.x <= -9.5)
                {
                    world_position_name.x = -9.5f;
                }
                    pos.x = world_position_name.x;
            }

            //ワールド位置反映(X,Y,Z)
            this.transform.position = pos;
        }*/

        void Update()
        {
            this.gameObject.transform.Translate(0, 0, movementSpeed);

            // transformを取得
            //Transform myTransform = this.transform;
            // 座標を取得
            // Vector3 pos = myTransform.position;

            if (Input.GetMouseButtonUp(0)) {
                pos.x = this.transform.position.x;
                world_position_name_dif.x = 0;
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

                /*if (world_position_name_dif.x >= 9.5)
                {
                    world_position_name_dif.x = 9.5f;
                }
                else if (world_position_name_dif.x <= -9.5)
                {
                    world_position_name_dif.x = -9.5f;
                }*/
                // pos.x = world_position_name_dif.x;
            }
            //pos.x = this.transform.position .x- world_position_name_dif.x;
            Debug.Log("A" + world_position_name_Axis);
            Debug.Log("U" + world_position_name_Upd);
            Debug.Log("D" + world_position_name_dif);



            //ワールド位置反映(X,Y,Z)
            this.transform.position = new Vector3(pos.x - world_position_name_dif.x, this.transform.position.y, this.transform.position.z);
            if (this.transform.position.x >= 9.5) {
                this.transform.position = new Vector3(9.5f, this.transform.position.y, this.transform.position.z);
            } else if (this.transform.position.x <= -9.5) {
                this.transform.position = new Vector3(-9.5f, this.transform.position.y, this.transform.position.z);
            }
        }
    }

}