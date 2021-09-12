using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _levelCompletedMenu;
    [SerializeField] private CanvasGroup _failMenu;

    public void OpenLevelCompletedMenu() => _levelCompletedMenu.gameObject.SetActive(true);

    public void CloseLevelCompletedMenu() => _levelCompletedMenu.gameObject.SetActive(false);

    public void OpenRetryLevelMenu() => _failMenu.gameObject.SetActive(true);

    public void CloseRetryLevelMenu() => _failMenu.gameObject.SetActive(false);
}
