using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreField : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    
	void Start () {
        
	}
	
    public void ShowScore(int score) {
        scoreText.SetText(score.ToString());
    }
}
