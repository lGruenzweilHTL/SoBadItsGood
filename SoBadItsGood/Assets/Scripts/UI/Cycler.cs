using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Custom Unity Tools/UI/Cycler")]
public class Cycler : MonoBehaviour {
    public List<string> Items {
        get => items;
        set => items = value;
    }

    [SerializeField] private List<string> items;

    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    [SerializeField, Tooltip("If you hold the button it will continue to go in the held direction")]
    private bool holdable;

    [SerializeField] private TMPro.TMP_Text displayText;

    [SerializeField] private UnityEvent<int> OnValueChanged;

    int currentItem;

    private void Update() {
        if (Input.GetKeyDown(right)) {
            OnInputRight();

            if (holdable) {
                StartCoroutine(WaitHold(true, true));
            }
        }

        if (Input.GetKeyDown(left)) {
            OnInputLeft();

            if (holdable) {
                StartCoroutine(WaitHold(false, true));
            }
        }

        displayText.text = items[currentItem];
    }

    private IEnumerator WaitHold(bool right, bool firstCall) {
        if (firstCall) yield return new WaitForSeconds(0.75f);

        if (Input.GetKey(right ? this.right : left)) {
            if (right) OnInputRight();
            else OnInputLeft();

            yield return new WaitForSeconds(0.1f);

            StartCoroutine(WaitHold(right, false));
        }
    }

    public void OnInputLeft() {
        if (currentItem == 0) currentItem = items.Count - 1;
        else currentItem--;

        OnValueChanged?.Invoke(currentItem);
    }

    public void OnInputRight() {
        if (currentItem == items.Count - 1) currentItem = 0;
        else currentItem++;

        OnValueChanged?.Invoke(currentItem);
    }
}