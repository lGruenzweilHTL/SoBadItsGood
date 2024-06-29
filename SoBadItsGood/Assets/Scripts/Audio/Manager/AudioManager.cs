using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
public class AudioManager : MonoBehaviour {
    [FormerlySerializedAs("defaultAmount")]
    [SerializeField] 
    [Tooltip("The amount of notes in the song")]
    private int noteAmount;
    
    [SerializeField] 
    [Tooltip("The default note to assign to each note in the song")]
    private Note defaultNote;
    
    [SerializeField] 
    [Tooltip("The default octave to assign to each note in the song")]
    private int defaultOctave;
    
    [SerializeField] 
    [Tooltip("The default note length multiplier to assign to each note in the song")]
    private int defaultNoteLengthMultiplier;
    
    [SerializeField] 
    [Tooltip("The Length each note should be played for in seconds")]
    private float beatLength;

    [SerializeField] 
    [Tooltip("Reference to a NotePlayer script")]
    private NotePlayer notePlayer;
    
    
    private WaitForSeconds _wait;


    private SongNote[] _notes = Array.Empty<SongNote>();

    private void Initialize() {
        _wait = new WaitForSeconds(beatLength / defaultNoteLengthMultiplier);
        
        Array.Resize(ref _notes, noteAmount);
        
        for (var i = 0; i < _notes.Length; i++) {
            _notes[i] = new SongNote {
                Note = defaultNote,
                Octave = defaultOctave,
                NoteLengthMultiplier = defaultNoteLengthMultiplier
            };
        }
    }

    public ref SongNote GetNoteAsReference(int index) {
        return ref _notes[index];
    }

    [ContextMenu("Play Current Song")]
    private void PlayCurrentSong() => PlaySong();
    private async void PlaySong() {
        foreach (var songNote in _notes) {
            notePlayer.PlaySound(songNote.Note, songNote.Octave);
            
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(beatLength / songNote.NoteLengthMultiplier));
        }
    }
    
    private void Awake() {
        Initialize();
    }
}
