using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// 画面をフラッシュさせる
    /// </summary>
    public class FlashEfect : MonoBehaviour
    {
        Image mImage;
        Color mTargetColor;

        //----------------------------------------------------------------------
        void Start()
        {
            mImage = GetComponent<Image>();
            mTargetColor = new Color(1, 1, 1, 0);
        }

        //----------------------------------------------------------------------
        void Update()
        {
            mImage.color = Color.Lerp(mImage.color, mTargetColor, Time.deltaTime * 10);
            if(mImage.color == mTargetColor) {
                Destroy(this.gameObject);
            }
        }
    }

} // namespace
// EOF
