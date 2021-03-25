using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace matsushima
{

    public class SHOP_Maneger : MonoBehaviour
    {
        int gotCoin; //持っているコイン

        public GameObject coinObject = null;  //コインを表示させるテキスト

        // Start is called before the first frame update
        void Start()
        {
            gotCoin = 9999999;
        }

        // Update is called once per frame
        void Update()
        {
            //コイン枚数の表示
            Text coinText = coinObject.GetComponent<Text>();
            if (gotCoin > 999999) {
                coinText.text = "COIN : 999999+";

            } else {
                coinText.text = "COIN : " + gotCoin;
            }



        }

        public void backbuttonDown()
        {
            SceneManager.LoadScene("GameScene");
        }


    }

}