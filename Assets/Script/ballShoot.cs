using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballShoot : MonoBehaviour
{

	Vector2 startPos;
	Vector2 endPos;
	Vector2 direction;

	float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

	[SerializeField]
	float throwForceInXandY = 1f;

	[SerializeField]
	float throwForceInZ = 50f;

	Rigidbody rb;

	[SerializeField]
	Camera selectedCamera;
	bool CanWarp = false;

	[Range(-360, 360)]
	public float x_range = 50;
	[Range(0, 300)]
	public float y_range = 50;
	[Range(-360, 360)]
	public float z_range = 300;


    void Start()
	{
		rb = GetComponent<Rigidbody>();

	}

	// Update is called once per frame
	void Update()
	{
        //if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)
        //      {

        //      }

        //if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)
        //      {
        //	if (!Manager.manager.shootcheck)
        //	{
        //		Manager.manager.shootcheck = true;
        //		rb.isKinematic = false;
        //		rb.AddForce(0, 300, 300);
        //	}
        //}


        if (Input.GetMouseButtonUp(0))
        {
            addforce();
        }

        if (Manager.manager.shootcheck)
        {
			CheckCam();
        }
        else
        {
			return;
        }		
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.layer)
		{
			case 0:
				break;
			case 6:
				//Manager.manager.LogText.text = "ball_hit";
				Manager.manager.goalCount++;
				Manager.manager.goal_text(Manager.manager.goalCount);
				rb.velocity = Vector3.zero;				
				rb.isKinematic = true;
				gameObject.SetActive(false);
				Manager.manager.returnball();
				break;
		}
	}

	
	public bool CheckCam()
	{
		Vector3 screenPoint = selectedCamera.WorldToViewportPoint(gameObject.transform.position);
		bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

		if (OnScreen == false)
		{
			rb.isKinematic = true;
			Manager.manager.returnball();
		}
		else
		{
			CanWarp = false;
		}

		return CanWarp;
	}

	public void addforce()
    {
		if (!Manager.manager.shootcheck)
		{
			Manager.manager.shootcheck = true;
			rb.isKinematic = false;

			

			//rb.AddForce(transform.forward * 500);
			rb.AddRelativeForce(new Vector3(0, y_range, z_range));
			
		}
	}


}
