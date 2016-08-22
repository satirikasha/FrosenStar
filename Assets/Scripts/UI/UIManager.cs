using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

    private static UIManager Instance;

    public float TransitionTime = 1.25f;

    public CanvasGroup DefaultPanel;
    public List<CanvasGroup> Panels;

    public CanvasGroup CurrentPanel { get; private set; }
    
    // Use this for initialization
    void Start() {
        Instance = this;

        Panels.ForEach(_ => SetPanelVisibility(_, false));

        SetPanelVisibility(DefaultPanel, true);
        CurrentPanel = DefaultPanel;
    }
    
    private void SetPanelVisibility(CanvasGroup panel, bool value) {
        panel.gameObject.SetActive(value);
        panel.blocksRaycasts = value;
        panel.alpha = value ? 1 : 0;
        panel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void SetCurrentPanel(CanvasGroup newPanel, CanvasGroup requiredPanel = null, Action<float> customAction = null) {
        StartCoroutine(SetCurrentPanelTask(newPanel, requiredPanel, customAction));
    }

    private IEnumerator SetCurrentPanelTask(CanvasGroup newPanel, CanvasGroup requiredPanel, Action<float> customAction) {
        if (requiredPanel != null)
            yield return new WaitUntil(() => CurrentPanel == requiredPanel);

        var oldPanel = CurrentPanel;
        CurrentPanel = null;
        

        newPanel.gameObject.SetActive(true);
        newPanel.blocksRaycasts = false;

        oldPanel.blocksRaycasts = false;

        var delay = TransitionTime;
        while (delay >= 0) {
            var t = 1 - delay / TransitionTime;
            t = t * t * (3f - 2f * t);

            newPanel.alpha = Mathf.Lerp(0, 1, t);
            oldPanel.alpha = Mathf.Lerp(1, 0, t);

            if (customAction != null)
                customAction(t);

            delay -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        SetPanelVisibility(newPanel, true);
        SetPanelVisibility(oldPanel, false);
        if (customAction != null)
            customAction(1);

        CurrentPanel = newPanel;
    }
}
