using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyOrbit : MonoBehaviour {
	
	public Vector3 m_force = Vector3.zero;
	public int m_limit = 10;
	
	private Material m_material;
	private List<GameObject> m_positions = new List<GameObject>();
	private bool isStarted = false;
	
	// Use this for initialization
	void Start () {
		m_material = Resources.Load("TimePositionMat") as Material;
		GetComponent<Rigidbody>().useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isStarted) {
			return;
		}
		
		Clear();
		
		for (int i = 0; i < m_limit; i++) {
			Vector3 pos = CalcPosition(0.1f * i);
			GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sp.transform.position = pos;
			sp.transform.parent = transform.parent;
			sp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			Destroy(sp.GetComponent<SphereCollider>());
			sp.GetComponent<Renderer>().material = m_material;
			
			m_positions.Add(sp);
		}
		
		if (Input.GetMouseButton(0)) {
			isStarted = true;
			//			Clear();
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
			rigidbody.AddForce(m_force);
		}
	}
	
	void Clear() {
		foreach (GameObject obj in m_positions) {
			Destroy(obj);
		}
		m_positions.Clear();
	}
	
	Vector3 CalcPosition(float time) {
		Vector3 start = transform.position;
		Vector3 gravity = Physics.gravity;
		float mass = GetComponent<Rigidbody>().mass;
		
		Vector3 speed = (m_force / mass) * Time.fixedDeltaTime;
		Vector3 gravitySpeed = gravity * 0.5f * Mathf.Pow(time, 2);
		
		return start + (speed * time) + gravitySpeed;
	}
}
