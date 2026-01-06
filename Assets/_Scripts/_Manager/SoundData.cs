using UnityEngine;
public enum SoundId
{
    BGM=0,
    ScrewClick=1,
    ScrewRelease=2,
    MagnetUse=3,
    BoxFull=4,
    Lose=5,
    LevelClear=6,
    PlayingBGM=7,
    ButtonClick=8,
    Noti=9,
    Broom=10,
    Drill=11,
    Hammer=12,
    Magnet=13
}

[System.Serializable]
public class SoundItem
{
    public SoundId id;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

[CreateAssetMenu(menuName = "Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    public SoundItem[] sounds;
}