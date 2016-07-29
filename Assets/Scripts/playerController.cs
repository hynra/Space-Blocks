using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public float xSpeed = 0.0f;         // x speed
    public float tilt = 0.0f;           // tilt speed
    public float xMin, xMax;            // x movement range
	public float moveSpeed = 0.0f;

    private float xInput = 0.0f;        // x input
    private float xMove = 0.0f;         // x movement (input * speed)
	public int score;
	public Transform spawnPlayer;
	private float tempSpeed;
	public spawnDanger spawnScript;
	private float tmpSpawnSeconds;
	private Vector3 tempSpawnPos;
	public GameObject mainCamera;
	private UnityStandardAssets.ImageEffects.Vortex vortex;
	private UnityStandardAssets.ImageEffects.Twirl twirl;
	public Text scoreText;
	public GameObject panelGameOver;
	public Text scoreOverText;
	public GameObject startingPanel;
	

	public void playGame(){
		score = 0;
		scoreText.text = "Score : " + score;
		Time.timeScale = 1;
		startingPanel.SetActive (false);
	}

	public void playAgain(){
		Time.timeScale = 1;
		score = 0;
		scoreText.text = "Score : " + score;
		panelGameOver.SetActive (false);
		scoreText.gameObject.SetActive (true);
	}

	public void exit(){
		panelGameOver.SetActive (false);
		startingPanel.SetActive (true);
		scoreText.gameObject.SetActive (true);
		Time.timeScale = 0;
	}

	void Start(){
		Time.timeScale = 0;

		tempSpeed = moveSpeed;
		tmpSpawnSeconds = spawnScript.spawnSeconds;
		tempSpawnPos = spawnScript.gameObject.transform.position;
		vortex = mainCamera.GetComponent<UnityStandardAssets.ImageEffects.Vortex> ();
		twirl = mainCamera.GetComponent<UnityStandardAssets.ImageEffects.Twirl> ();
	}

	public void addScore(){
		score += 1;
		scoreText.text = "Score : " + score;
		Debug.Log ("Score : "+score);
		if (score == 200) {
			vortex.enabled = true;
		} else if (score == 400) {
			twirl.enabled = true;
			vortex.enabled = false;
		}else if(score == 800){
			vortex.enabled = true;
		}
	}
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        xMove = xInput * xSpeed;

		//-------- touch ---------//
		int k = 0;
		while (k <Input.touchCount) {
			if(Input.GetTouch(k).position.x < Screen.width/2){
				if(Input.touchCount > 0){
					// left
					xMove -= 1 * xSpeed;
				}
			}

			if(Input.GetTouch(k).position.x > Screen.width/2){
				if(Input.touchCount > 0){
					// right
					xMove += 1 * xSpeed;
				}
			}
			k++;
		}
    }


    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(xMove, 0.0f, moveSpeed * Time.smoothDeltaTime);
        float playerTilt = GetComponent<Rigidbody>().velocity.x * tilt;
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, playerTilt);
        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, xMin, xMax),
            GetComponent<Rigidbody>().position.y,
			GetComponent<Rigidbody>().position.z
        );
    }
	
    void OnTriggerEnter(Collider collisions)
    {
        Debug.Log("Player hit by: " + collisions.gameObject.tag);
		gameOver (score.ToString());
    }

	public void gameOver(string _score){
		this.transform.position = spawnPlayer.position;
		GameObject[] floors = GameObject.FindGameObjectsWithTag ("floor");
		foreach (GameObject floor in floors) {
			Destroy(floor);
		}
		GameObject[] walls = GameObject.FindGameObjectsWithTag ("dangerCube");
		foreach (GameObject wall in walls) {
			wall.GetComponent<dangerController>().stopCountingScore();
			Destroy(wall);
		}
		GameObject[] particles = GameObject.FindGameObjectsWithTag ("particle");
		foreach (GameObject particle in particles) {
			Destroy(particle);
		}

		scoreText.gameObject.SetActive (false);
		moveSpeed = tempSpeed;
		spawnScript.spawnSeconds = tmpSpawnSeconds;
		spawnScript.gameObject.transform.position = tempSpawnPos;
		vortex.enabled = false;
		twirl.enabled = false;
		panelGameOver.SetActive (true);
		scoreOverText.text = "Score : " + _score;
		Time.timeScale = 0;
	}
}