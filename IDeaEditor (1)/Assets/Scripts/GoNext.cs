using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoNext : MonoBehaviour
{

	public Canvas ProjectCanvas;

	public void NextButton()
	{
		ProjectCanvas.enabled = false;
	}
}