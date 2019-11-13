using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject[] notes;
    public float[] noteX;
	private float[] _timing;
	private int[] _lineNum;
    private int[] _type;
    private GameObject[] _noteInstance;

    public string filePass;
	private int _notesCount = 0;

	private AudioSource _audioSource;
	private float _startTime = 0;

	public float timeOffset = -1;

	private bool _isPlaying = false;
	public GameObject startButton;

	public Text scoreText;
	private int _score = 0;
    public int maxNotes = 1024;
    private int score;
    public float yPos = 10.0f ;

	void Start(){
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_timing = new float[maxNotes];
		_lineNum = new int[maxNotes];
        _type = new int[maxNotes];
        _noteInstance = new GameObject[maxNotes];
        score = 0;

        LoadCSV ();
	}

	void Update () {
		if (_isPlaying) {
			CheckNextNotes ();
			// scoreText.text = _score.ToString ();
		}
			
	}

	public void StartGame(){
		startButton.SetActive (false);
		_startTime = Time.time;
		_audioSource.Play ();
		_isPlaying = true;
	}

	void CheckNextNotes(){
        if ( _notesCount > maxNotes)
        {
            return;
        }
		while (_timing [_notesCount] + timeOffset < GetMusicTime () && _timing [_notesCount] != 0) {
			GameObject spawnedNotes = SpawnNotes (_lineNum[_notesCount], _type[_notesCount]);
            spawnedNotes.GetComponent<NotesScript>().noteCount = _notesCount;
            spawnedNotes.GetComponent<NotesScript>().lineNum = _lineNum[_notesCount];
            _noteInstance[_notesCount] = spawnedNotes;


            _notesCount++;
		}
	}

	GameObject SpawnNotes(int lineNum, int typeNum){
        //Debug.Log(num);
        // 
        GameObject obj = Instantiate (notes[typeNum], 
			new Vector3 (noteX[lineNum], yPos, 0),
			Quaternion.identity);
        
        return obj;
	}

	void LoadCSV(){
		int i = 0, j;
		TextAsset csv = Resources.Load (filePass) as TextAsset;
		StringReader reader = new StringReader (csv.text);
		while (reader.Peek () > -1) {
			
			string line = reader.ReadLine ();
			string[] values = line.Split (',');
			for (j = 0; j < values.Length; j++) {
				_timing [i] = float.Parse( values [0] );
				_lineNum [i] = int.Parse( values [1] );
                _type[i] = int.Parse(values[2]);
            }
			i++;
		}
	}

	float GetMusicTime(){
		return Time.time - _startTime;
	}

	public void GoodTimingFunc(int num){
		Debug.Log ("Line:" + num + " good!");
		Debug.Log (GetMusicTime());
		// 追加
		EffectManager.Instance.PlayEffect(num);
		_score++;
	}

    public void SetNullToNoteInstance( int index)
    {
        _noteInstance[index] = null;
    }

    public void CheckNotes(int index)
    {
        // Debug.Log(index);

        for ( int i = 0; i < maxNotes; i ++)
        {
            if ( _noteInstance[i] != null)
            {
                GameObject note = _noteInstance[i];
                // Debug.Log(i);
                if (note.GetComponent<NotesScript>().isInLine)
                {
                    //Debug.Log("InLine");
                    //Debug.Log(note.GetComponent<NotesScript>().lineNum);
                    if (note.GetComponent<NotesScript>().lineNum == index)
                    {
                        //Debug.Log("Hit");
                        // 点数入れる
                        score++;
                        //Debug.Log(score);
                        //Debug.Log(score.ToString());
                        scoreText.GetComponent<Text>().text = "Score:" + score.ToString();

                        Destroy(_noteInstance[i]);
                        _noteInstance[i] = null;
                        
                    }
                }
            }
        }
    }

}
