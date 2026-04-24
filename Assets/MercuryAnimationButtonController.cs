using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MercuryAnimationButtonController : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button playButton;

    [Header("수성 부모 오브젝트")]
    [SerializeField] private GameObject mercuryParentObject;

    [Header("수성 애니메이션 오브젝트")]
    [SerializeField] private GameObject mercuryAnimationObject;

    [Header("애니메이션 유지 시간")]
    [SerializeField] private float activeTime = 2f;

    private SpriteRenderer mercurySpriteRenderer;
    private Coroutine playCoroutine;

    private void Awake()
    {
        if (mercuryParentObject != null)
        {
            mercurySpriteRenderer = mercuryParentObject.GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayMercuryAnimation);
        }

        ResetMercuryState();
    }

    private void OnDestroy()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveListener(PlayMercuryAnimation);
        }
    }

    private void PlayMercuryAnimation()
    {
        if (mercurySpriteRenderer == null || mercuryAnimationObject == null)
        {
            Debug.LogWarning("수성 SpriteRenderer 또는 수성 애니메이션 오브젝트가 연결되지 않았습니다.");
            return;
        }

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }

        ResetMercuryState();

        playCoroutine = StartCoroutine(PlayMercuryAnimationRoutine());
    }

    private IEnumerator PlayMercuryAnimationRoutine()
    {
        mercurySpriteRenderer.enabled = false;
        mercuryAnimationObject.SetActive(true);

        yield return new WaitForSeconds(activeTime);

        mercuryAnimationObject.SetActive(false);
        mercurySpriteRenderer.enabled = true;

        playCoroutine = null;
    }

    private void ResetMercuryState()
    {
        if (mercurySpriteRenderer != null)
        {
            mercurySpriteRenderer.enabled = true;
        }

        if (mercuryAnimationObject != null)
        {
            mercuryAnimationObject.SetActive(false);
        }
    }
}