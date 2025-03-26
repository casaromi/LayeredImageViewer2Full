using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWebCellButtons : MonoBehaviour
{
    // Web Cell slider buttons
    public GameObject heightSliderButton;
    public GameObject transparencySliderButton;
    public GameObject cutoffSliderButton;

    // Web Cell slider content
    public GameObject heightLabel;
    public GameObject heightSlider;
    public GameObject transparencyLabel;
    public GameObject transparencySlider;
    public GameObject cutoffLabel;
    public GameObject cutoffSlider;

    // Point Cloud slider content
    public GameObject SizeSlider;
    public GameObject ColorSlider;

    // View selectors and back
    public GameObject WebCellViewButton;
    public GameObject PointCloudViewButton;
    public GameObject backButton;

    private enum ViewMode { None, WebCell, PointCloud }
    private ViewMode currentView = ViewMode.None;

    // ===============================
    // View Entry Points
    // ===============================

    public void ShowWebCellSliders()
    {
        currentView = ViewMode.WebCell;

        heightSliderButton.SetActive(true);
        transparencySliderButton.SetActive(true);
        cutoffSliderButton.SetActive(true);

        SizeSlider.SetActive(false);
        ColorSlider.SetActive(false);

        StartCoroutine(DisableAfterClick(WebCellViewButton));
        StartCoroutine(DisableAfterClick(PointCloudViewButton));

        backButton.SetActive(true);
    }

    public void ShowPointCloudSliders()
    {
        currentView = ViewMode.PointCloud;

        SizeSlider.SetActive(true);
        ColorSlider.SetActive(true);

        heightSliderButton.SetActive(false);
        transparencySliderButton.SetActive(false);
        cutoffSliderButton.SetActive(false);
        heightLabel.SetActive(false);
        heightSlider.SetActive(false);
        transparencyLabel.SetActive(false);
        transparencySlider.SetActive(false);
        cutoffLabel.SetActive(false);
        cutoffSlider.SetActive(false);

        StartCoroutine(DisableAfterClick(WebCellViewButton));
        StartCoroutine(DisableAfterClick(PointCloudViewButton));

        backButton.SetActive(true);
    }

    // ===============================
    // Web Cell Sub-buttons
    // ===============================

    public void ToggleHeightSlider()
    {
        bool isActive = heightLabel.activeSelf;

        heightLabel.SetActive(!isActive);
        heightSlider.SetActive(!isActive);

        if (!isActive)
        {
            heightSliderButton.SetActive(false);
            transparencySliderButton.SetActive(false);
            cutoffSliderButton.SetActive(false);
        }
    }

    public void ToggleTransparencySlider()
    {
        bool isActive = transparencyLabel.activeSelf;

        transparencyLabel.SetActive(!isActive);
        transparencySlider.SetActive(!isActive);

        if (!isActive)
        {
            heightSliderButton.SetActive(false);
            transparencySliderButton.SetActive(false);
            cutoffSliderButton.SetActive(false);
        }
    }

    public void ToggleCutoffSlider()
    {
        bool isActive = cutoffLabel.activeSelf;

        cutoffLabel.SetActive(!isActive);
        cutoffSlider.SetActive(!isActive);

        if (!isActive)
        {
            heightSliderButton.SetActive(false);
            transparencySliderButton.SetActive(false);
            cutoffSliderButton.SetActive(false);
        }
    }

    // ===============================
    // Back button resets everything
    // ===============================

    public void HideAllSliders()
    {
        Debug.Log("Back button pressed — current view: " + currentView);

        // Hide Web Cell UI
        heightSliderButton.SetActive(false);
        transparencySliderButton.SetActive(false);
        cutoffSliderButton.SetActive(false);
        heightLabel.SetActive(false);
        heightSlider.SetActive(false);
        transparencyLabel.SetActive(false);
        transparencySlider.SetActive(false);
        cutoffLabel.SetActive(false);
        cutoffSlider.SetActive(false);

        // Hide Point Cloud UI
        SizeSlider.SetActive(false);
        ColorSlider.SetActive(false);

        // Show main view selector buttons
        WebCellViewButton.SetActive(true);
        PointCloudViewButton.SetActive(true);
        backButton.SetActive(false);

        currentView = ViewMode.None;
    }

    private IEnumerator DisableAfterClick(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        obj.SetActive(false);
    }
}