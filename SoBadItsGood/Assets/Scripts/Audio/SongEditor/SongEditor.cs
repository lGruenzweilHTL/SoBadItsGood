using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongEditor : MonoBehaviour {
    [Serializable]
    private struct NoteSliderObject {
        public NoteSlider noteSlider;
        public int rowIndex;
        public int noteIndex;
    }

    [SerializeField] private int numRows = 2;
    [SerializeField] private int notesPerRow = 5;
    [Space, SerializeField] private float beatLength = 0.5f;

    private static readonly (Note note, int octave) LowestNote = (Note.C, -1);
    private static readonly (Note note, int octave) HighestNote = (Note.C, 2);

    [Space, SerializeField] private NotePlayer notePlayer;
    [Space, SerializeField] private NoteSlider noteSliderPrefab;
    [SerializeField] private Transform noteRowPrefab;
    [SerializeField] private Transform sliderParent;

    private SongNote[] _notes = { };

    public static Action OnOpenedAnyEditor;
    public static Action OnClosedAnyEditor;

    private void Start() {
        Debug.Log("Initializing...", gameObject);
        _notes = new SongNote[numRows * notesPerRow];
        InitializeRows();
    }

    public void OpenEditor() {
        sliderParent.gameObject.SetActive(true);
        OnOpenedAnyEditor?.Invoke();
    }

    private void CloseEditor() {
        sliderParent.gameObject.SetActive(false);
        OnClosedAnyEditor?.Invoke();
    }

    private void Update() {
        if (sliderParent.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) CloseEditor();
    }

    [ContextMenu("Regenerate Rows")]
    private void InitializeRows() {
        // Instantiate all note rows and sliders for each row
        for (int i = 0; i < numRows; i++) {
            // Instantiate a row
            Transform row = Instantiate(noteRowPrefab, sliderParent);
            // Instantiate sliders for each note in the row
            for (int j = 0; j < notesPerRow; j++) {
                // Initialize the slider
                NoteSliderObject note = new() {
                    noteSlider = Instantiate(noteSliderPrefab, row),
                    rowIndex = i,
                    noteIndex = j
                };
                note.noteSlider.slider.onValueChanged.AddListener(pitch => OnPitchChanged((int)pitch, note.rowIndex, note.noteIndex, note.noteSlider.noteText));
                InitializeSlider(note.noteSlider.slider);
            }
        }
    }

    private void InitializeSlider(Slider slider) {
        int lowestNoteSemitones = LowestNote.octave * 12 + (int)LowestNote.note;
        int highestNoteSemitones = HighestNote.octave * 12 + (int)HighestNote.note;

        slider.minValue = lowestNoteSemitones;
        slider.maxValue = highestNoteSemitones;
        slider.wholeNumbers = true;
        slider.value = (highestNoteSemitones + lowestNoteSemitones) / 2f; // Set the slider to the middle note
    }

    /// <summary>
    /// The callback method for when a slider's value changes
    /// </summary>
    /// <param name="pitch">The new pitch of the note</param>
    /// <param name="rowIndex">The row the slider is on</param>
    /// <param name="noteIndex">The slider's index inside the row</param>
    /// <param name="noteText">The text to display the current note</param>
    private void OnPitchChanged(int pitch, int rowIndex, int noteIndex, TMP_Text noteText) {
        // Pitch = octave * 12 + noteValue

        int index = rowIndex * notesPerRow + noteIndex; // Calculate the index in the notes array
        Note noteValue = (Note)((pitch + 12) % 12); // Reverse-engineer the note value

        int octave = pitch / 12;
        if (pitch < 0 && noteValue != Note.C) octave--;

        // Get a reference to the note at the calculated index
        ref SongNote note = ref _notes[index];
        note.Note = noteValue;
        note.Octave = octave;

        string noteString = noteValue.ToString().Replace("Sharp", "#");
        noteText.text = $"{noteString}<size=50%>{octave + 4}";

        print($"Note at index {index} is now {noteValue} at octave {octave}");
    }
    
    public async void PlaySong(CancellationToken token) {
        foreach (SongNote songNote in _notes) {
            notePlayer.PlaySound(songNote.Note, songNote.Octave);
            
            // ReSharper disable once MethodSupportsCancellation
            // Reason to not pass CancellationToken: If passed and cancelled, the method will throw an OperationCancelledException
            await Task.Delay(TimeSpan.FromSeconds(beatLength));
            if (token.IsCancellationRequested) break;
        }
    }
}