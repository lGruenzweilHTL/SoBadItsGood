using System.Collections;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    [System.Serializable]
    public struct SongNote
    {
        public Note note;
        public int octave;
        [Tooltip("Example: A quarter note has 4")] public int noteLengthMultiplier;
    }
    [SerializeField] private NotePlayer notePlayer;
    [SerializeField] private float fullBeat;
    [SerializeField] private SongNote[] songNotes;

    private void Start()
    {
        StartCoroutine(PlaySong());
    }

    private IEnumerator PlaySong()
    {
        foreach (SongNote songNote in songNotes)
        {
            notePlayer.PlaySound(songNote.note, songNote.octave, fullBeat / songNote.noteLengthMultiplier);
            yield return new WaitForSeconds(fullBeat / songNote.noteLengthMultiplier);
        }
    }
}
