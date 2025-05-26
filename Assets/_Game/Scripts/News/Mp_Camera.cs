using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mp_Camera : MonoBehaviour
{
	public Transform target;
	public float followSpeed;

	public float directionOffset;

	[Header("Limits")]
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		if (target)
		{
			Vector3 position = this.target.transform.position;
			position.z = -10;

			position.x = position.x + HudManager.instance.localPlayerReference.xAttackDirection * directionOffset;
			position.y = position.y + HudManager.instance.localPlayerReference.yAttackDirection * directionOffset;


			if (position.x < xMin)
			{
				position.x = xMin;
			}
			if (position.x > xMax)
			{
				position.x = xMax;
			}
			if (position.y < yMin)
			{
				position.y = yMin;
			}
			if (position.y > yMax)
			{
				position.y = yMax;
			}

			base.transform.position = Vector3.Lerp(base.transform.position, position, this.followSpeed * Time.deltaTime);
		}
		
	}
}
