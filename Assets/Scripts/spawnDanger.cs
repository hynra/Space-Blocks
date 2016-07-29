using UnityEngine;
using System.Collections;

public class spawnDanger : MonoBehaviour
{
    public GameObject spawnPrefab;         // prefab yang akan di spawn -> wall
	public GameObject floorPrefab;
	public GameObject rainPrefab;
    public float spawnSeconds;             // spawn setiap ? detik
    public float xMin, xMax;               // x position range -> utk random position
    public float yMin, yMax;               // y position range -> utk random position

    private float timeElapsed = 0.0f;      // time elapsed
    private float prevTime = 0.0f;         // previous time
	private playerController player;
	private Vector3 posZ;

	void Awake(){
		player = GameObject.Find ("Player").GetComponent<playerController> ();
	}

    // Update is called once per frame
    void Update()
    {
		// panggi fungsi Spawn dalam update (realtime)
		// dengan parameter waktu untuk menentukan interval kapan wall spawn
        Spawn(spawnSeconds);
		//transform.position += Vector3.forward * 3 *Time.deltaTime;
    }

    // fungsi untuk menetukan random position dari wall
    void RandomPosition(float _xMin, float _xMax, float _yMin, float _yMax)
    {
        // posisi x utk wall baru dari range xmin & xMax
        float xMove = Random.Range(_xMin, _xMax);
		// posisi y utk wall baru dari range ymin & yMax
        float yMove = Random.Range(_yMin, _yMax);
        // update posisi spawner yang kemudian akan dipake wall baru yg telah di spawner
        transform.position = new Vector3(xMove, yMove, transform.position.z);
    }

    // this function handles the object spawning, is time controlled
    void Spawn(float _spawnSeconds)
    {
        // create elapsed time by adding delta time
        timeElapsed += Time.deltaTime;

        // if elapsed time minus previous time is greater than spawn seconds, do something...
        if (timeElapsed - prevTime > _spawnSeconds)
        {
			// randomly move our spawner on the x
			RandomPosition(xMin, xMax, yMin, yMax);

            // spawn the prefab we referenced in this object's inspector
            // set the prefab's position & rotation to match this spawner
//			spawnPrefab.GetComponent<Renderer>().material.color = Color.blue;
			Instantiate(spawnPrefab, new Vector3(transform.position.x, 90, transform.position.z), new Quaternion(0, 0, 0,0));
			Instantiate(rainPrefab, new Vector3(transform.position.x, 90, transform.position.z), new Quaternion(0, 0, 0,0));
			Instantiate(floorPrefab, new Vector3(0, -10, transform.position.z+25), new Quaternion(0, 0, 0,0));

            // reset our timer by syncing elapsed time with previous time
			player.moveSpeed += 5;
			posZ = transform.position;
			posZ.z += 0.5f;
			this.transform.position = posZ;
			//spawnSeconds -= 0.0001f;
            prevTime = timeElapsed;
			StartCoroutine(updateSpawnseconds());
        }
    }

	IEnumerator updateSpawnseconds(){
		yield return new WaitForSeconds(1.5f);
		spawnSeconds -= 0.0003f;
		StopAllCoroutines ();
	}
}
