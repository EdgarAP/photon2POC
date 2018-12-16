using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    private PlayerManager target;

    [Tooltip("Pixel offset from the player target")]
    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    private float characterControllerHeight = 0f;
    private Transform targetTransform;
    private Vector3 targetPosition;

    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.Health;
        }
    }

    void LateUpdate()
    {
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y = characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerManager target for PlayerUI.SetTarget");
            return;
        }

        // Cache references for efficiency
        target = _target;

        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }

        CharacterController characterController = _target.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterControllerHeight = characterController.height;
        }
    }
}
