using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ugai
{

    public class Text : MonoBehaviour
    {
        //文字を書く場所を指定
        public Rect zahyou_mouse = new Rect(0, 0, 150, 50);
        private Vector3 position;                       // 位置座標
        private Vector3 screenToWorldPointPosition;     // スクリーン座標をワールド座標に変換した位置座標
                                                        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            position = Input.mousePosition;             // Vector3でマウス位置座標を取得する
            position.z = 10f;   //←不明
                                // マウス位置座標をスクリーン座標からワールド座標に変換する
            screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
        }
        //GUI更新イベントが有ると勝手に呼ばれる
        void OnGUI()
        {
            string s, ss;
            s = screenToWorldPointPosition.ToString();
            GUI.Label(zahyou_mouse, s);
            ss = Input.touchCount.ToString();
            GUI.Label(new Rect(0, 60, 150, 50), ss);
        }
    }

}