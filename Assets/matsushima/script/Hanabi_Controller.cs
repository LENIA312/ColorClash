using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanabi_Controller : MonoBehaviour
{

 public static class define
    {
        public const int Red = 0;
        public const int Gleen = 1;
        public const int Blue = 2;
    }

    int[] randColor = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        //パーティクルオブジェクトを取得
        var particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;


            //RGBにランダムな色を与える
            for (int i = 0; i < 3; i++)
            {
                randColor[i] = Random.Range(0, 1 + 1);
            }

            if(randColor[define.Red] == 0 && randColor[define.Blue] == 0 && randColor[define.Gleen] == 0)
            {
            randColor[Random.Range(define.Red, define.Gleen)] = 1;
            }


            //花火の色を変更
            main.startColor = new Color(randColor[define.Red], randColor[define.Gleen], randColor[define.Blue]);
    }

    // Update is called once per frame
    void Update()
    {
        //消滅させる
        Destroy(this.gameObject, 5f);
    }
}
