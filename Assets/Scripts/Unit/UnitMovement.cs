using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
	[SerializeField] LayerMask _ground;
	NavMeshAgent _agent;
	Camera _cam;

	private void Start()
	{
		_cam = Camera.main;
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
			{
				_agent.SetDestination(hit.point);
			}
		}
	}
}
