using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PowerupUIManager : MonoBehaviour
{
    [SerializeField] private Transform _skillParent;
    private Vector3 _skillParentInitialLocation;
    [SerializeField] private CanvasGroup _openTabCanvasGroup;

    [SerializeField] private List<Transform> _postClickPositionData;
    [SerializeField] private List<Transform> _preClickPositionData;
    [SerializeField] private List<PowerupUI> _powerupUIs;

    [SerializeField] private Button _outsideOverlayButton;

    [SerializeField] private TMP_Text _powerupNameText;

    public enum ClickStage { PreClick, PostClick }
    public ClickStage clickStage = ClickStage.PreClick;

    private int _selectedPowerupID = -1;

    Sequence overlaySequence;

    private void Awake()
    {
        for (int i = 0; i < _powerupUIs.Count; i++)
        {
            int x = i;
            _powerupUIs[x].powerupIcon.GetComponent<Button>().onClick.AddListener(() => PostClickReset(x));
        }
        _outsideOverlayButton.onClick.AddListener(() => PreClickReset());
        _skillParentInitialLocation = _skillParent.position;
    }

    private void Start()
    {
        for (int i = 0; i < _powerupUIs.Count; i++)
        {
            _powerupUIs[i].Rotate(_preClickPositionData[i].position, false, true);
        }
        _selectedPowerupID = -1;
        overlaySequence = DOTween.Sequence();
    }

    private void PostClickReset(int powerupID)
    {
        if (_selectedPowerupID == powerupID)
        {
            return;
        }

        _skillParent.DOMove(new Vector3(Screen.width * 3 / 4, Screen.height / 2, 0f), 1f, false);
        #region Rotation
        bool clockwise;
        if (Mathf.Abs(_selectedPowerupID - powerupID) > (float)_powerupUIs.Count / 2 && _selectedPowerupID - powerupID > 0 ||
            Mathf.Abs(_selectedPowerupID - powerupID) < (float)_powerupUIs.Count / 2 && _selectedPowerupID - powerupID < 0)
        {
            clockwise = true;
        }
        else
        {
            clockwise = false;
        }

        for (int i = 0; i < _powerupUIs.Count; i++)
        {
            _powerupUIs[i].Rotate(_postClickPositionData[(-i + 12 + powerupID) % 5].position, false, clockwise);
        }
        #endregion

        #region Scale
        _powerupUIs[powerupID].ScaleWhenSelected(true);
        if (_selectedPowerupID != -1)
        {
            _powerupUIs[_selectedPowerupID].ScaleWhenSelected(false);
        }
        #endregion

        _openTabCanvasGroup.DOFade(1f, 1f).OnUpdate(delegate
        {
            _openTabCanvasGroup.gameObject.SetActive(true);
            _outsideOverlayButton.gameObject.SetActive(true);
        });
        _selectedPowerupID = powerupID;
        _powerupNameText.text = _powerupUIs[powerupID].powerupName;
        clickStage = ClickStage.PostClick;
    }

    void PreClickReset()
    {
        if (_selectedPowerupID == -1)
        {
            return;
        }
        for (int i = 0; i < _powerupUIs.Count; i++)
        {
            _powerupUIs[i].Rotate(_preClickPositionData[i].position, false, true);
        }
        _skillParent.DOMove(_skillParentInitialLocation, 1f, false);
        _powerupUIs[_selectedPowerupID].ScaleWhenSelected(false);
        _openTabCanvasGroup.DOFade(0f, 1f).OnComplete(delegate
        {
            _openTabCanvasGroup.gameObject.SetActive(false);
            _outsideOverlayButton.gameObject.SetActive(false);
        });
        _selectedPowerupID = -1;
        clickStage = ClickStage.PreClick;
    }
}
