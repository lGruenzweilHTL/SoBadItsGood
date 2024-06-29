using System;
using UnityEngine;
using UnityEngine.UI;

public class SongEditor : MonoBehaviour {
    [Serializable]
    private struct NoteSlider {
        public Slider slider;
        public int rowIndex;
        public int noteIndex;
    }

    private const int NUM_ROWS = 2;
    private const int NOTES_PER_ROW = 5;
    
    private static readonly (Note note, int octave) LowestNote = (Note.C, -1);
    private static readonly (Note note, int octave) HighestNote = (Note.C, 2);

    [SerializeField] private AudioManager audioManager;
    [Space, SerializeField] private Slider noteSliderPrefab;
    [SerializeField] private Transform noteRowPrefab;
    [SerializeField] private Transform sliderParent;

    private SongNote[] _notes = new SongNote[NUM_ROWS * NOTES_PER_ROW];

    private void Start() => InitializeRows();

    private void InitializeRows() {
        // Instantiate all note rows and sliders for each row
        for (int i = 0; i < NUM_ROWS; i++) {
            // Instantiate a row
            Transform row = Instantiate(noteRowPrefab, sliderParent);
            // Instantiate sliders for each note in the row
            for (int j = 0; j < NOTES_PER_ROW; j++) {
                // Initialize the slider
                NoteSlider note = new() {
                    slider = Instantiate(noteSliderPrefab, row),
                    rowIndex = i,
                    noteIndex = j
                };
                note.slider.onValueChanged.AddListener(pitch => OnPitchChanged((int)pitch, note.rowIndex, note.noteIndex));
                InitializeSlider(note.slider);
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
    private void OnPitchChanged(int pitch, int rowIndex, int noteIndex) {
        // Pitch = octave * 12 + noteValue
        
        int index = rowIndex * NOTES_PER_ROW + noteIndex; // Calculate the index in the notes array
        Note noteValue = (Note)((pitch + 12) % 12); // Reverse-engineer the note value
        
        int octave = pitch / 12;
        if (pitch < 0 && noteValue != Note.C) octave--;
        
        // Get a reference to the note at the calculated index
        ref SongNote note = ref audioManager.GetNoteAsReference(index);
        note.Note = noteValue;
        note.Octave = octave;
        
        print($"Note at index {index} is now {noteValue} at octave {octave}");
    }
}