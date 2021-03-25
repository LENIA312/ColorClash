//列挙型まとめ
//-------------------------------------------------------------------------------------------------
namespace kai
{

    /// <summary>
    /// 障害物の行動パターン列挙型
    /// </summary>
    public enum MOVETYPE : byte
    {
        静止,
        左右x,
        上下z,
        上下y,
        斜めxz,
        円運動xz,
        円運動xy,
        回転x,
        回転y,
        回転z,
    }

    /// <summary>
    /// ゲームの状態を示す列挙型
    /// </summary>
    public enum GAMESTATE
    {
        TITLE,
        PLAYING,
        GAMEOVER,
        STAGECLEAR,
    }

    /// <summary>
    /// SEを指定
    /// </summary>
    public enum SETYPE
    {
        ボタン,
        コイン,
        色変え,
        ダメージ,
        パワーアップ,
        ブロック破壊
    }

} // namespace
