using UnityEngine;

/// <summary>
/// 默认音效实现：单 AudioSource + PlayOneShot。场景中挂一个即可；其他脚本通过接口或本组件引用调用。
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class HfmSfxPlayer : MonoBehaviour, IHfmSfx
{
    [Header("Mixer (optional)")]
    [Range(0f, 1f)] public float masterVolume = 1f;

    [Header("Puzzle")]
    public AudioClip puzzlePiecePickup;
    public AudioClip puzzlePieceSnapSuccess;
    public AudioClip puzzlePieceSnapReject;
    public AudioClip puzzleLevelComplete;

    [Header("UI / Lore (optional)")]
    public AudioClip uiClick;
    public AudioClip uiOpen;
    public AudioClip loreLineAdvance;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
        HfmSfxHub.SetPlayer(this);
    }

    public void Play(HfmSfxEvent sfxEvent)
    {
        var clip = Resolve(sfxEvent);
        PlayOneShot(clip, 1f);
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null || _source == null) return;
        _source.PlayOneShot(clip, masterVolume * Mathf.Clamp01(volumeScale));
    }

    private AudioClip Resolve(HfmSfxEvent e)
    {
        switch (e)
        {
            case HfmSfxEvent.PuzzlePiecePickup: return puzzlePiecePickup;
            case HfmSfxEvent.PuzzlePieceSnapSuccess: return puzzlePieceSnapSuccess;
            case HfmSfxEvent.PuzzlePieceSnapReject: return puzzlePieceSnapReject;
            case HfmSfxEvent.PuzzleLevelComplete: return puzzleLevelComplete;
            case HfmSfxEvent.UIClick: return uiClick;
            case HfmSfxEvent.UIOpen: return uiOpen;
            case HfmSfxEvent.LoreLineAdvance: return loreLineAdvance;
            default: return null;
        }
    }
}
