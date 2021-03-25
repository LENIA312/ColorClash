using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace kai
{

    public class AudioManager : MonoBehaviour
    {
        // Singleton
        public static AudioManager instance = null;

        #region *[public変数]
        public AudioClip[] _SE;
        #endregion

        #region *[private変数]
        AudioSource audioSource;
        #endregion

        private void Awake()
        {
            // Singleton
            if (instance == null) {
                instance = this;
            } else {
                Destroy(this.gameObject);
            }
        }
        //-----------------------------------------------------------------------------------------
        private void Start()
        {
            // Componentを取得
            audioSource = GetComponent<AudioSource>();
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 関数名     : Sound
        /// 処理の概要 : 効果音を鳴らす
        /// 備考       : 
        /// </summary>
        /// <param name="aNum">
        /// SETYPE:鳴らしたい音
        /// ボタン、コイン、色変え、ダメージ、パワーアップ、ブロック破壊
        /// </param>
        public void SoundPlayOneShot(SETYPE aSETYPE, float aVolume = 1)
        {
            audioSource.PlayOneShot(_SE[(int)aSETYPE], aVolume);
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 指定したAudioClipを再生する
        /// </summary>
        /// <param name="aNum"></param>
        /// <returns></returns>
        public void SoundPlayClipAtPoint(SETYPE aSETYPE, Vector3 aPos, float aVolume = 1)
        {
            AudioSource.PlayClipAtPoint(_SE[(int)aSETYPE], aPos, aVolume);
        }
       
    }
} // namespace
