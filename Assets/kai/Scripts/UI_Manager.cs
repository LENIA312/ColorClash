using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------------------------
namespace kai
{

    public class UI_Manager
    {        
        GameManager mGameManager;

        #region #privateメンバ変数
        //{
        int mCoin;
        int mComboCount;
        float mComboTimer;

        GameObject[] mUI_Objs = null;
        GameObject[] mComboUI_Objs = new GameObject[2];

        Image mBlackScreenImage;

        Text mCoinUI_Text;
        Text[] mComboUI_Texts = new Text[2];

        Color[] mColors = new Color[2];

        Animator mComboUI_Animator;
        //}
        #endregion

        //----------------------------------------------------------------------
        public UI_Manager()
        {
            mGameManager = GameManager.instance;

            mCoin = mGameManager.GetCoin();
            mComboCount = mGameManager.GetComboCount();

            // Obj
            mUI_Objs = mGameManager.GetUI_Obj();
            mComboUI_Objs[0] = GameObject.Find("ComboUI");
            mComboUI_Objs[1] = GameObject.Find("ComboCount");

            // Image
            mBlackScreenImage = GameObject.Find("BlackScreen").GetComponent<Image>();

            // Text
            mCoinUI_Text = GameObject.Find("CoinCount").GetComponent<Text>();
            mCoinUI_Text.text = "" + mCoin;
            mComboUI_Texts[0] = mComboUI_Objs[0].GetComponent<Text>();
            mComboUI_Texts[0].text = "";
            mComboUI_Texts[1] = mComboUI_Objs[1].GetComponent<Text>();
            mComboUI_Texts[1].text = "";            

            // Color
            mColors[0] = mComboUI_Texts[0].color;
            mColors[1] = new Color(0, 0, 0, 0);

            // Animator
            mComboUI_Animator = mComboUI_Objs[1].GetComponent<Animator>();
        }

        //----------------------------------------------------------------------
        public void TitleUI_Update()
        {
            if (!mUI_Objs[(int)GAMESTATE.TITLE].activeSelf) {
                // UI表示
                foreach(GameObject uiObjs in mUI_Objs) {
                    uiObjs.SetActive(false);
                }
                mUI_Objs[(int)GAMESTATE.TITLE].SetActive(true);
                // コンボ非表示
                mComboUI_Texts[0].color = mColors[1];
                mComboUI_Texts[1].color = mColors[1];
                // 黒画面非表示
                mBlackScreenImage.color = mColors[1];
            }
        }

        //----------------------------------------------------------------------
        public void PlayingUI_Update()
        {
            if (!mUI_Objs[(int)GAMESTATE.PLAYING].activeSelf) {
                // UI表示
                foreach (GameObject uiObjs in mUI_Objs) {
                    uiObjs.SetActive(false);
                }
                mUI_Objs[(int)GAMESTATE.PLAYING].SetActive(true);
            }
        }

        //----------------------------------------------------------------------
        public void GameOverUI_Update()
        {
            if (!mUI_Objs[(int)GAMESTATE.GAMEOVER].activeSelf) {
                // UI表示
                foreach (GameObject uiObjs in mUI_Objs) {
                    uiObjs.SetActive(false);
                }
                mUI_Objs[(int)GAMESTATE.GAMEOVER].SetActive(true);
                // コンボ非表示
                mComboUI_Texts[0].color = mColors[1];
                mComboUI_Texts[1].color = mColors[1];
                // 黒画面非表示
                mBlackScreenImage.color = mColors[1];
            }
            // だんだん暗く
            if(mBlackScreenImage.color.a < 0.8) {
                mBlackScreenImage.color =
                    Color.Lerp(
                        mBlackScreenImage.color,
                        new Color(0, 0, 0, 0.8f),
                        Time.deltaTime * 2
                        );
            }
        }

        //----------------------------------------------------------------------
        public void StageClearUI_Update()
        {
            if (!mUI_Objs[(int)GAMESTATE.STAGECLEAR].activeSelf) {
                // UI表示
                foreach (GameObject uiObjs in mUI_Objs) {
                    uiObjs.SetActive(false);
                }
                mUI_Objs[(int)GAMESTATE.STAGECLEAR].SetActive(true);
                // コンボ非表示
                mComboUI_Texts[0].color = mColors[1];
                mComboUI_Texts[1].color = mColors[1];
                // 黒画面非表示
                mBlackScreenImage.color = mColors[1];
            }
            // だんだん暗く
            if (mBlackScreenImage.color.a < 0.8) {
                mBlackScreenImage.color =
                    Color.Lerp(
                        mBlackScreenImage.color,
                        new Color(0, 0, 0, 0.8f),
                        Time.deltaTime
                        );
            }
        }

        //----------------------------------------------------------------------
        public void CoinUI_Update()
        {
            if (mCoin != mGameManager.GetCoin()) {
                mCoin = mGameManager.GetCoin();
                mCoinUI_Text.text = "" + mCoin;
            }
        }

        //----------------------------------------------------------------------
        public void ComboUI_Update()
        {
            // コンボ数更新
            if(mComboCount != mGameManager.GetComboCount()) {
                mComboCount = mGameManager.GetComboCount();
                if(mComboCount > 0) {
                    // １コンボ以上ならタイマー起動
                    mComboTimer = mGameManager._ComboTime;
                    if (mComboCount >= 5) {
                        // 5コンボ以上ならコンボ数表示
                        mComboUI_Texts[0].text = "COMBO!";
                        mComboUI_Texts[0].color = mColors[0];
                        mComboUI_Texts[1].text = "" + mComboCount;
                        mComboUI_Texts[1].color = mColors[0];
                        mComboUI_Animator.Play("Combo");
                    }
                } else {
                    // コンボ表示を消す
                    mComboUI_Texts[0].text = "";
                    mComboUI_Texts[1].text = "";
                }
            }
            // コンボタイマー
            if(mComboTimer > 0) {
                // 徐々に薄くする
                mComboUI_Texts[0].color = Color.Lerp(mComboUI_Texts[0].color, mColors[1], Time.deltaTime / mComboTimer);
                mComboUI_Texts[1].color = Color.Lerp(mComboUI_Texts[1].color, mColors[1], Time.deltaTime / mComboTimer);
                // タイマーを減算
                mComboTimer = Mathf.MoveTowards(mComboTimer, 0, Time.deltaTime);
                if (mComboTimer <= 0) {
                    mGameManager.ResetComboCount();
                }
            }
        }
    }

} // namespace
// EOF
