[System.Serializable]
public struct SongNote
{
    public Note note;
    public int octave;
    [UnityEngine.Tooltip("Example: A quarter note has 4")] public int noteLengthMultiplier;
}