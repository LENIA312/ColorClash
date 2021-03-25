using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
namespace ugai
{

    public class AudioTest : MonoBehaviour
    {
        public enum SETYPE
        {
            ボタン,
            コイン,
            色変え,
            ダメージ,
            パワーアップ,
            ブロック破壊
        }

        public AudioClip _Sound1;//ボタン
        public AudioClip _Sound2;//コイン
        public AudioClip _Sound3;//色変え
        public AudioClip _Sound4;//ダメージ
        public AudioClip _Sound5;//パワーアップ
        public AudioClip _Sound6;//ブロック破壊
        AudioSource audioSource;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// 関数名     : Sound
        /// 処理の概要 : 効果音を鳴らす
        /// 引数       : SETYPE:鳴らしたい音
        /// 　　　　　　 ボタン、コイン、色変え、ダメージ、パワーアップ、ブロック破壊
        /// 返却値     : 
        /// 備考       : 
        /// </summary>
        /// <param name="anum"></param>
        public void Sound(SETYPE anum)
        {
            //Componentを取得
            audioSource = GetComponent<AudioSource>();

            if (anum == SETYPE.ボタン){
                //音(sound1)を鳴らす
                audioSource.PlayOneShot(_Sound1);
            }
            if (anum == SETYPE.コイン){
                //音(sound2)を鳴らす
                audioSource.PlayOneShot(_Sound2);
            }
            if (anum == SETYPE.色変え){
                //音(sound3)を鳴らす
                audioSource.PlayOneShot(_Sound3);
            }
            if (anum == SETYPE.ダメージ){
                //音(sound4)を鳴らす
                audioSource.PlayOneShot(_Sound4);
            }
            if (anum == SETYPE.パワーアップ){
                //音(sound5)を鳴らす
                audioSource.PlayOneShot(_Sound5);
            }
            if (anum == SETYPE.ブロック破壊){
                //音(sound6)を鳴らす
                audioSource.PlayOneShot(_Sound6);
            }
        }
       
    }
}//namespace
