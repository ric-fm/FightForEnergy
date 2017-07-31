/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string gameScene = "game_scene";

	public GameObject buttonsPanel;

	public GameObject instructionsPanel;

	public GameObject creditsPanel;

	public void Play()
	{
		SceneManager.LoadScene(gameScene);
	}

	public void ShowInstructions()
	{
		buttonsPanel.SetActive(false);
		instructionsPanel.SetActive(true);
	}

	public void HideInstructions()
	{
		instructionsPanel.SetActive(false);
		buttonsPanel.SetActive(true);
	}

	public void ShowCredits()
	{
		buttonsPanel.SetActive(false);
		creditsPanel.SetActive(true);
	}

	public void HideCredits()
	{
		creditsPanel.SetActive(false);
		buttonsPanel.SetActive(true);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
