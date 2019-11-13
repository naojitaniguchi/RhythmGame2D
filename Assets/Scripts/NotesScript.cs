using System.Collections;
using UnityEngine;

public class NotesScript : MonoBehaviour {

	public int lineNum;
	private GameController _gameManager;
	public bool isInLine = false;
	private KeyCode _lineKey;
    public int noteCount;
    public float yMin = -5.0f;
    public float speed = 10.0f;

	void Start () {
		_gameManager = GameObject.Find ("GameManager").GetComponent<GameController> ();
		_lineKey = GameUtil.GetKeyCodeByLineNum(lineNum);
	}

	void Update () {
		this.transform.position += Vector3.down * speed * Time.deltaTime;

		if (this.transform.position.y < yMin) {
			// Debug.Log ("false");
            _gameManager.GetComponent<GameController>().SetNullToNoteInstance(noteCount);

            Destroy (this.gameObject);
		}

		if(isInLine){
			CheckInput(_lineKey);
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        isInLine = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        isInLine = false;
    }

	void CheckInput (KeyCode key) {

		if (Input.GetKeyDown (key)) {
			_gameManager.GoodTimingFunc (lineNum);
            _gameManager.GetComponent<GameController>().SetNullToNoteInstance(noteCount);
            Destroy (this.gameObject);
		}
	}
}