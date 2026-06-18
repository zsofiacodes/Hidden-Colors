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
            if (cinematicAnimator != null) cinematicAnimator.speed = 10f;
        }
    }

    public void PlayLaughEvent() => AudioManager.Instance.PlayKidLaughingSFX();
    public void PlayFallEvent() => AudioManager.Instance.PlayKidFallSFX();

    public void CinematicHasEnded()
    {
        StartCoroutine(CinematicHasEndedCoroutine());
    }

    private IEnumerator CinematicHasEndedCoroutine()
    {
        SceneTransitionUIManager.Instance.StartTransition();
        yield return new WaitForSeconds(1f);

        // This checks if we are in the Game scene to go to Outro
        if (SceneManager.GetActiveScene().name == "Game")
        {
            SceneManager.LoadScene("OutroCinematic");
        }
        // This handles your existing flow for the Intro
        else if (SceneManager.GetActiveScene().name == "IntroCinematic")
        {
            GameManager.Instance.SetState(GameState.Tutorial);
            SceneManager.LoadScene("Game");
        }
    }
}