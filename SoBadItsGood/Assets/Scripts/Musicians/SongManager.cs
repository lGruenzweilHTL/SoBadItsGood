using System.Threading;
using UnityEngine;

public class SongManager : MonoBehaviour {
    private CancellationTokenSource _cancellationTokenSource;

    public void StartPlaying() {
        _cancellationTokenSource = new CancellationTokenSource();
        PlayAllSongs(_cancellationTokenSource.Token);
    }

    public void StopPlaying() => _cancellationTokenSource.Cancel();

    private static void PlayAllSongs(CancellationToken token) {
        SongEditor[] songPlayers = FindObjectsOfType<SongEditor>();
        foreach (SongEditor songPlayer in songPlayers) {
            songPlayer.PlaySong(token);
        }
    }
}