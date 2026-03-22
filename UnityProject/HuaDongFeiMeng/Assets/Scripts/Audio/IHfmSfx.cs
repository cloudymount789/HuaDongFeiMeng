/// <summary>
/// 全局音效入口（预留扩展：可换 FMOD/Wwise/Addressables 等实现）。
/// </summary>
public interface IHfmSfx
{
    /// <summary>播放预定义事件；无剪辑或未注册时安全 no-op。</summary>
    void Play(HfmSfxEvent sfxEvent);

    /// <summary>按资源直接播放（关卡一次性音效、策划临时指定等）。</summary>
    void PlayOneShot(UnityEngine.AudioClip clip, float volumeScale = 1f);
}
