using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Musician : MonoBehaviour {
    [SerializeField] private SongEditor editor;
    
    public static bool IsHoveringOverAny;
    private void OnMouseDown() {
        print($"Opening song editor for musician {name}");
        editor.OpenEditor();
    }

    private void OnMouseEnter() => IsHoveringOverAny = true;
    private void OnMouseExit() => IsHoveringOverAny = false;
}