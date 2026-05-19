using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private Animator cinematicAnimator;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            cinematicAnimator.speed = 10f;
        }
    }

    public void PlayLaughEvent()
    {
        AudioManager.Instance.PlayKidLaughingSFX();
    }

    public void PlayFallEvent()
    {
        AudioManager.Instance.PlayKidFallSFX();
    }

    public void CinematicHasEnded()
    {
        StartCoroutine(CinematicHasEndedCoroutine());
        GameManager.Instance.SetState(GameState.Tutorial);
    }

    private IEnumerator CinematicHasEndedCoroutine()
    {
        SceneTransitionUIManager.Instance.StartTransition();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}