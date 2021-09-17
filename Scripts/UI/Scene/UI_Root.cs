using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Root : MonoBehaviour
{
	private void Start() {
		DontDestroyOnLoad(gameObject);
	}
}
