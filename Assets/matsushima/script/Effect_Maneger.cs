using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace matsushima
{

    public class Effect_Maneger : MonoBehaviour
    {
        GameObject playerObj;
        GameObject goalLineObj;

        public GameObject hanabiEffect;
        public GameObject confettiEffect;

        //private int cnt_tt;

        private int[] cnt_tt = new int[2];

        // Start is called before the first frame update
        void Start()
        {
            playerObj = GameObject.Find("Player");
            goalLineObj = GameObject.Find("GoalLine");

            //タイマー配列の中身を初期化
            for(int i = 0; i < cnt_tt.Length; i++)
            {
                cnt_tt[i] = 0;
            }

        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Hello Unko");

            //ゴールしたら ( Player.csと同じ処理のためまとめたほうがよいかもしれないかももち米 )
            if (playerObj.transform.position.z > goalLineObj.transform.position.z)
            {
                //Debug.Log("Sucsess");

                //花火を一定時間置きに
                cnt_tt[0]++; cnt_tt[1]++;
                if (cnt_tt[0] == 31)
                {
                    HanabiGene();

                    cnt_tt[0] = 1;
                }

                //花吹雪を一度だけ
                if (cnt_tt[1] == 1)
                {
                    ConfettiGene();
                }
            }
        }


        /// <summary>
        /// 花火を生成
        /// </summary>
        void HanabiGene()
        {
            Instantiate(hanabiEffect, new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(165f,190f)), Quaternion.Euler(-90f, 0, 0));
        }

        /// <summary>
        /// ゴールの紙吹雪生成
        /// </summary>
        void ConfettiGene()
        {
            Instantiate(confettiEffect, new Vector3(10f, 1f, 163f), Quaternion.Euler(-30, -90, 0));
            Instantiate(confettiEffect, new Vector3(-10f, 1f, 163f), Quaternion.Euler(-30, 90, 0));
        }

    }
}
