using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{

	public int highScore;
	public Text highScoreText;
	public Text scoreText;
	public GameObject startScreen;
	public GameObject restartButton;
	public float timeScale = 1;
	public Vector2 deadPos;
	public Transform parent;
	public int fieldw;
	public int fieldh;
	public int dead;
	public float speed;
	public GameObject piecePref;
	public GameObject apple;
	public List<SnakePiece> pieces;
	public bool isDead;//use this for dead stuff

	public int dir = 1;
	public float countdownTillMove = 0;

	private int pdir;
	private int startingLength;
    // Start is called before the first frame update
    void Start()
    {
		Time.timeScale = 0;
		startingLength = pieces.Count;
		highScore = PlayerPrefs.GetInt("HS", 0);
		highScoreText.text = "High Score: " + highScore;
		scoreText.text = "Score: 0";

	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void NewApple()
	{
		int x = Random.Range(1 - fieldw/2, fieldw/2);
		int y = Random.Range(1 - fieldh/2, fieldh/2);
		Instantiate(apple, new Vector3(x, y, 0), Quaternion.identity);
		Grow();
	}

	public void Grow()
	{
		foreach (SnakePiece p in pieces) p.index++;
		pieces.Insert(0, Instantiate(piecePref, new Vector3(-100000, 0, 0), Quaternion.identity, parent).GetComponent<SnakePiece>());
		pieces[0].snake = this;
		int tempScore = pieces.Count - startingLength;
		scoreText.text = "Score: " + tempScore;
		if(tempScore > highScore)
		{
			PlayerPrefs.SetInt("HS", tempScore);
			highScore = tempScore;
		}
		highScoreText.text = "High Score: " + highScore;
	}
	
	public void MoveSnake()
	{
		//make sure that for loop procedes from first (closest to end of snake).
		for(int i = 0;i < pieces.Count - 1; i++)
		{
			pieces[i].MoveNext();
		}
		SnakePiece temp = pieces[pieces.Count - 1];
		if (dir == 0) temp.y++;
		if (dir == 1) temp.x++;
		if (dir == 2) temp.y--;
		if (dir == 3) temp.x--;
		pdir = dir;
		pieces[pieces.Count - 1].transform.GetChild(0).eulerAngles = new Vector3(0, 0, -dir * 90);
		dead = 0;
	}

	// Update is called once per frame
	void Update()
    {
		
		if (isDead)
		{
			return;
		}
		if (dead > 3)
		{
			isDead = true;
			restartButton.SetActive(true);
			//spawn death thing
		}
		if(Time.timeScale > 0)
		{
			startScreen.SetActive(false);
		}
		if (pdir != 2 && Input.GetKeyDown(KeyCode.UpArrow)		|| Input.GetKeyDown(KeyCode.W)) {	dir = 0; countdownTillMove = 0; Time.timeScale = timeScale; }
		if (pdir != 3 && Input.GetKeyDown(KeyCode.RightArrow)	|| Input.GetKeyDown(KeyCode.D)) {	dir = 1; countdownTillMove = 0; Time.timeScale = timeScale; }
		if (pdir != 0 && Input.GetKeyDown(KeyCode.DownArrow)	|| Input.GetKeyDown(KeyCode.S)) {	dir = 2; countdownTillMove = 0; Time.timeScale = timeScale; }
		if (pdir != 1 && Input.GetKeyDown(KeyCode.LeftArrow)	|| Input.GetKeyDown(KeyCode.A)) {	dir = 3; countdownTillMove = 0; Time.timeScale = timeScale; }
		countdownTillMove -= Time.deltaTime;
		if(countdownTillMove < 0)
		{
			countdownTillMove = 1 / speed;
			MoveSnake();
		}
		if(dead > 4)
		{
			speed = 0;
		}
	}
}
