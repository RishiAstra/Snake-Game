using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePiece : MonoBehaviour
{
	public int index;
	public int x;
	public int y;

	public bool movedYet;
	public Snake snake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(x, y, 0);
    }

	public void MoveNext()
	{
		x = snake.pieces[index + 1].x;
		y = snake.pieces[index + 1].y;
		if(x > -1-snake.fieldw && x < snake.fieldw + 2)	movedYet = true;
	}

	public void OnTriggerStay2D(Collider2D col)
	{
		if (!movedYet) return;
		GameObject g = col.gameObject;
		if (g.tag == "Apple")
		{
			snake.NewApple();
			g.tag = "Untagged";
			Destroy(g);
		}
		if (g.tag == "Snake")
		{
			snake.dead++;
			snake.deadPos = col.ClosestPoint(transform.position);
		}
	}
}
