using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
	public static Scoreboard S;

	[Header("Set In inspector")]
	public GameObject prefabFloatingScore;

	[Header("Set Dynamically")]
	[SerializeField] private int _score = 0;
	[SerializeField] private string _scoreString;

	private Transform canvasTrans;

	public int score {
		get {
			return (_score);
		}
		set {
			_score = value;
			scoreString = _score.ToString("N0");
		}
	}

	public string scoreString {
		get {
			return (_scoreString);
		}
		set {
			_scoreString = value;
			GetComponent<Text>().text = _scoreString;
		}
	}


    void Awake()
    {
		if (S == null) {
			S = this;
		} else {
			Debug.LogError("singleton is already set (scoreboard)");
		}
		canvasTrans = transform.parent;
    }
	
	public void FSCallback(FloatingScore fs) {
		score += fs.score;
	}

	public FloatingScore CreateFloatingScore(int amt,List<Vector2> pts) {
		GameObject go = Instantiate<GameObject>(prefabFloatingScore);
		go.transform.SetParent(canvasTrans);
		FloatingScore fs = go.GetComponent<FloatingScore>();
		fs.score = amt;
		fs.reportFinishTo = this.gameObject;
		fs.Init(pts);
		return (fs);
	}
}
