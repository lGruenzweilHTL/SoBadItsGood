[System.Serializable]
public struct SongNote
{
    public Note Note;
    public int Octave;
    [UnityEngine.Tooltip("Example: A quarter note has 4")] public int NoteLengthMultiplier;
}