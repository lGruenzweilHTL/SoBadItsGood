using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MusicianPlacer : MonoBehaviour {
    [SerializeField] private Camera mainCam;
    [SerializeField] private Musician musicianPrefab;

    private bool _isEditorOpen = false;

    private void Start() {
        SongEditor.OnOpenedAnyEditor += () => _isEditorOpen = true;
        SongEditor.OnClosedAnyEditor += () => _isEditorOpen = false;
    }

    private void OnMouseUpAsButton() {
        if (_isEditorOpen || Musician.IsHoveringOverAny) return;
        
        Vector2 mousePosWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(musicianPrefab, mousePosWorld, Quaternion.identity, transform);
    }
}