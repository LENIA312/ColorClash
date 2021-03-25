using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://christinayan01.jp/architecture/archives/3145
/// 
/// ちゃんと理解してません
/// </summary>

public class SkyBox_Controller : MonoBehaviour
{
    public float anglePerFrame = 0.01f;    // 1フレームに何度回すか
    float rot = 0.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rot += anglePerFrame;
        if (rot >= 360.0f)
        {    // 0～360°の範囲におさめたい
            rot = 0.0f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", rot);    // 回す
    }
}
