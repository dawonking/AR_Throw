using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballShoot : MonoBehaviour
{
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

	//�浹�� ť�꿡 �浹�ϸ� ���� �߰� �� ���� �ڸ��� ���ƿ´�.
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

	//���� ī�޶� �þ߿��� �������� ���� üũ
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
						
			//y��� z�� �������θ� �����ش�.
			rb.AddRelativeForce(new Vector3(0, y_range, z_range));
			
		}
	}


}
