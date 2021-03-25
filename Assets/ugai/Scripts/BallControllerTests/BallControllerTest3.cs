using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ugai
{

    public class BallControllerTest3 : MonoBehaviour
    {
        private Vector3 position_name_Upd;
        private Vector3 position_name_Axis;

        private Vector3 world_position_name_Upd;
        private Vector3 world_position_name_Axis;
        private Vector3 world_position_name_dif;

        bool flg = false;

        // 座標を取得
        Vector3 pos;

        void Start()
        {
            //pos.x = this.transform.localPosition.x;

            //pos.z = this.transform.localPosition.z;
        }

        void Update()
        {
            if (Input.touchCount > 1) {
                flg = true;
            }

            if (Input.GetMouseButtonUp(0)) {
                Debug.Log("離した瞬間");

                pos.x = this.transform.localPosition.x;
                world_position_name_dif.x = 0;

                pos.z = this.transform.localPosition.z;
                world_position_name_dif.y = 0;
                //flg = true;
            }

            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("押した瞬間");
                if (Input.touchCount == 1) {
                    pos.x = this.transform.localPosition.x;
                    pos.z = this.transform.localPosition.z;

                    // print("いま左ボタンが押された");
                    position_name_Axis = Input.mousePosition;
                    //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                    position_name_Axis.z = 10f;
                    //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                    world_position_name_Axis = Camera.main.ScreenToWorldPoint(position_name_Axis);
                }
            }

            if (Input.GetMouseButton(0)) {
                Debug.Log("押しっぱなし");
                if (Input.touchCount == 1) {

                    if (flg == true) {
                        pos.x = this.transform.localPosition.x;
                        pos.z = this.transform.localPosition.z;

                        position_name_Axis = Input.mousePosition;

                        //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                        position_name_Axis.z = 10f;
                        //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                        world_position_name_Axis = Camera.main.ScreenToWorldPoint(position_name_Axis);
                        world_position_name_Upd = Camera.main.ScreenToWorldPoint(position_name_Axis);
                        flg = false;
                    }
                    //マウス位置取得(X,Y,Z)
                    position_name_Upd = Input.mousePosition;
                    //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                    position_name_Upd.z = 10f;
                    //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                    world_position_name_Upd = Camera.main.ScreenToWorldPoint(position_name_Upd);

                    world_position_name_dif = world_position_name_Upd - world_position_name_Axis;

                    world_position_name_dif.x *= 3f;
                    world_position_name_dif.y *= 3f;
                }

            }


            /*if (Input.touchCount > 0)
            {
                // タッチ情報の取得
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("押した瞬間");
                    if (Input.touchCount == 1)
                    {
                        pos.x = this.transform.localPosition.x;
                        pos.z = this.transform.localPosition.z;

                        // print("いま左ボタンが押された");
                        position_name_Axis = Input.mousePosition;
                        //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                        position_name_Axis.z = 10f;
                        //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                        world_position_name_Axis = Camera.main.ScreenToWorldPoint(position_name_Axis);
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log("離した瞬間");

                    pos.x = this.transform.localPosition.x;
                    world_position_name_dif.x = 0;

                    pos.z = this.transform.localPosition.z;
                    world_position_name_dif.y = 0;
                    //flg = true;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Debug.Log("押しっぱなし");
                    if (Input.touchCount == 1)
                    {

                        if (flg == true)
                        {
                            position_name_Axis = Input.mousePosition;

                            //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                            position_name_Axis.z = 10f;
                            //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                            world_position_name_Axis = Camera.main.ScreenToWorldPoint(position_name_Axis);
                            flg = false;
                        }
                        //マウス位置取得(X,Y,Z)
                        position_name_Upd = Input.mousePosition;
                        //Z軸反映(マウス位置は2Dのため、カメラポジションのZ軸の10を入れる)
                        position_name_Upd.z = 10f;
                        //マウス位置をカメラに反映、ワールド位置取得(X,Y,Z)
                        world_position_name_Upd = Camera.main.ScreenToWorldPoint(position_name_Upd);

                        world_position_name_dif = world_position_name_Upd - world_position_name_Axis;

                        world_position_name_dif.x *= 3f;
                        world_position_name_dif.y *= 3f;
                    }
                }
            }*/

            this.transform.localPosition = new Vector3(pos.x + world_position_name_dif.x, this.transform.localPosition.y, pos.z + world_position_name_dif.y);

            if (this.transform.localPosition.x >= 9.5) {
                this.transform.localPosition = new Vector3(9.5f, this.transform.localPosition.y, this.transform.localPosition.z);
            } else if (this.transform.localPosition.x <= -9.5) {
                this.transform.localPosition = new Vector3(-9.5f, this.transform.localPosition.y, this.transform.localPosition.z);
            }
            if (this.transform.localPosition.z <= -13) {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -13f);
            }

        }
        void OnGUI()
        {
            string /*s, ss,*/ sss, s4/*, s5, s6*/;
            /* s = pos.ToString();
             GUI.Label(new Rect(0, 20, 150, 50), s);
             ss = this.transform.localPosition.ToString();
             GUI.Label(new Rect(0, 40, 150, 50), ss);*/

            sss = world_position_name_dif.ToString();
            GUI.Label(new Rect(0, 80, 150, 50), sss);

            s4 = world_position_name_Axis.ToString();
            GUI.Label(new Rect(0, 100, 150, 50), s4);

            /*s5 = world_position_name_Upd.ToString();
            GUI.Label(new Rect(0, 120, 150, 50), s5);
            s6 = position_name_Axis.ToString();
            GUI.Label(new Rect(0, 140, 150, 50), s6);*/
        }
    }

}