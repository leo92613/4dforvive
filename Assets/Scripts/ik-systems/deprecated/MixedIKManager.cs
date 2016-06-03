using UnityEngine;
using System.Collections;

public class MixedIKManager : MonoBehaviour {
	
	public bool snapHips = false;
	public bool renderLines = true;
	
	private static int _MixedIKID_ = 1;
	public int _id_ = 0;
	public bool showGizmos = true;
	public Material LimbsMaterial;
	public float LimbWidth = 0.05f;
	public float LowerLimbWidth = 0.03f;
	
	public Color lineColor = new Color(.5f, .5f, .5f);
	
	public Transform mocapHeadset;
	public Transform mocapLeftWrist = null;
	public Transform mocapRightWrist = null;
	public Transform mocapLeftAnkle = null;
	public Transform mocapRightAnkle = null;
	
	private Transform _headJoint;
	private Transform _hipJoint;
	private Transform _leftHandJoint;
	private Transform _rightHandJoint;
	private Transform _leftFootJoint;
	private Transform _rightFootJoint;
	private Transform[] _spines = new Transform[3];
	private Transform[] _shoulders = new Transform[2];
	private Transform[] _elbows = new Transform[2];
	private Transform[] _uplegs = new Transform[2];
	private Vector3[] _knees = new Vector3[2];
	private float _legDist;
	private float _armDist;
	
	public Vector3 headOffset;
	public Vector3 handOffset;
	public Vector3 footOffset;
	public Vector3 SpineOffset;
	private Vector3 _baseSpineOrigOffset;
	
	public float kneeScale = 0.1f;
	public float elbowScale = 0.1f;
	public float minimumMidOffset = 0.05f;
	public float maximumMidOffset = 0.5f;
	public Vector3 elbowDirection;
	
	public int lineDetail = 10;
	
	private int _numClicks = 0;
	private float _numClicksTimer = 0f;
	private float _numClicksTimeFrame = 2f;
	
	private Vector3 _offset;
	private Vector3 _originalScale;
	private float _origYOffset;
	private float _origXOffset;
	
	private Vector3 _headToHips;
	private Vector3 _hipsPosition = Vector3.zero;
	
	private Vector3 _nb;
	
	public float upperArmLength = 0.5f;
	public float lowerArmLength = 0.5f;
	public Vector3 hint = new Vector3(0, 0, 1);

	
	private float _averageHeadHeight = 1;
	private float[] _headHeights;
	
	// Use this for initialization
	void Start () {
		
		this._id_ = _MixedIKID_++;
		
		Debug.Log("Starting UnityIK for Avatar ID#" + this._id_);
		
		//animator = this.GetComponent<Animator>();
		
		this.FindSkeletalParts();
		
		_offset = _headJoint.transform.position.y * Vector3.up;
		_headToHips = _headJoint.transform.position - _hipJoint.transform.position;
		_originalScale = this.transform.localScale;
		_origYOffset = _headJoint.transform.position.y;
		_baseSpineOrigOffset = _spines[0].position - _hipJoint.position;
		_legDist = Vector3.Distance(_uplegs[0].position, _leftFootJoint.position);
		_armDist = Vector3.Distance(_shoulders[0].position, _leftHandJoint.position);
		
		GameObject shoulderChild = new GameObject ();
		shoulderChild.transform.parent = _shoulders [0];
		GameObject shoulderChild2 = new GameObject ();
		shoulderChild2.transform.parent = _shoulders [1];
		//_ik = new IK();
		
		
		Debug.Log("Start successful for Avatar ID#" + this._id_);
	}
	
	void FindSkeletalParts()
	{
		Transform[] children = this.GetComponentsInChildren<Transform>();
		for (int i = 0; i < children.Length; i++)
		{
			switch (children[i].tag)
			{
			case "HEAD":
				_headJoint = children[i];
				break;
			case "HIPS":
				_hipJoint = children[i];
				break;
			case "LEFTHAND":
				_leftHandJoint = children[i];
				break;
			case "RIGHTHAND":
				_rightHandJoint = children[i];
				break;
			case "LEFTFOOT":
				_leftFootJoint = children[i];
				break;
			case "RIGHTFOOT":
				_rightFootJoint = children[i];
				break;
			case "SPINE":
				_spines[0] = children[i];
				break;
			case "SPINE1":
				_spines[1] = children[i];
				break;
			case "SPINE2":
				_spines[2] = children[i];
				break;
			case "LEFTSHOULDER":
				_shoulders[0] = children[i];
				break;
			case "RIGHTSHOULDER":
				_shoulders[1] = children[i];
				break;
			case "LEFTUPLEG":
				_uplegs[0] = children[i];
				break;
			case "RIGHTUPLEG":
				_uplegs[1] = children[i];
				break;
			case "LEFTELBOW":
				_elbows[0] = children[i];
				break;
			case "RIGHTELBOW":
				_elbows[1] = children[i];
				break;
				
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.PositionBody();
		this.BindAvatarToMocap();
		this.CheckForResize();
		this.SolveHipsAndIK();
		this.SolveSpineAndOthers();
		this.averageHeadHeight ();
	}
	
	void PositionBody()
	{
		//this.transform.position = mocapHeadset.position - _offset + (mocapHeadset.rotation * headOffset);
		
		float la = mocapLeftAnkle.rotation.eulerAngles.y;
		float ra = mocapRightAnkle.rotation.eulerAngles.y;
		float ny = ((Mathf.Abs(la - ra) > 180) ? (la + ra + 360) : (la + ra)) / 2f;
		ny = ny % 360;
		
		transform.rotation = Quaternion.Euler(0, ny, 0);
		//Debug.Log(ny);
		
		_nb = new Vector3((mocapLeftAnkle.position.x + mocapRightAnkle.position.x) / 2f, 0f, (mocapLeftAnkle.position.z + mocapRightAnkle.position.z) / 2f);
		this.transform.position = _nb;
	}
	
	void BindAvatarToMocap()
	{
		if (mocapHeadset)    Bind(mocapHeadset, _headJoint, headOffset);
		if (mocapLeftWrist)  Bind(mocapLeftWrist, _leftHandJoint, handOffset);
		if (mocapRightWrist) Bind(mocapRightWrist, _rightHandJoint, handOffset);
		if (mocapLeftAnkle)  Bind(mocapLeftAnkle, _leftFootJoint, footOffset);
		if (mocapRightAnkle) Bind(mocapRightAnkle, _rightFootJoint, footOffset);
	}
	
	void Bind(Transform mocap, Transform bone, Vector3 offset)
	{
		if (bone.tag.Equals("LEFTHAND") || bone.tag.Equals("LEFTFOOT"))
		{
			offset.x *= -1;
		}
		
		bone.position = mocap.position + (mocap.rotation * offset);
		bone.rotation = mocap.rotation;
	}
	
	void CheckForResize()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_numClicks += 1;
			
			if (_numClicks == 1)
			{
				_numClicksTimer = Time.time;
			}
		}
		
		if (_numClicks >= 4)
		{
			this.ResizeAvatar();
			_numClicks = 0;
		}
		
		if (Time.time > _numClicksTimer + _numClicksTimeFrame)
		{
			_numClicks = 0;
		}
	}
	
	public static void ResizeAvatarWithID(int id)
	{
		MixedIKManager[] managers = GameObject.FindObjectsOfType<MixedIKManager>();
		
		for (int i = 0; i < managers.Length; i++)
		{
			if (managers[i]._id_ == id)
			{
				managers[i].ResizeAvatar();
			}
		}
	}
	
	void ResizeAvatar()
	{
		Debug.Log("Resize Activated for Avatar ID#" + this._id_);
		Vector3 ny = _originalScale;
		float scale = (mocapHeadset.position.y / _origYOffset);
		ny *= scale;
		_baseSpineOrigOffset *= scale;
		SpineOffset *= scale;
		_offset = _headJoint.transform.position.y * Vector3.up;
		_headToHips = _headJoint.transform.position - _hipJoint.transform.position;
		this.transform.localScale = ny;
	}
	
	void SolveHipsAndIK()
	{
		_hipsPosition = SolveForHipsPosition();
		_hipJoint.transform.position = _hipsPosition;
		_hipJoint.transform.up = (_headJoint.transform.position - _hipJoint.transform.position).normalized;	
	}
	
	Vector3 SolveForHipsPosition()
	{
		Vector3 hit = Vector3.zero;
		
		if (SolveHipsIntersection(out hit) && snapHips)
		{
			return hit;
		}
		else
		{
			hit = _headJoint.transform.position - (_headJoint.transform.position - _nb).normalized * Vector3.Distance(_headToHips, Vector3.zero);
			Vector3 headxz = Vector3.Scale (_headJoint.transform.position, Vector3.right + Vector3.forward);
			Vector3 offsetxz = (headxz - _nb)* -0.5f;
			hit += offsetxz;
		}
		
		
		return hit;
	}
	
	bool SolveHipsIntersection(out Vector3 hitPoint)
	{
		
		float d, q, t, l2, r2, m2;
		hitPoint = Vector3.zero;
		
		Vector3 s2r = _headJoint.transform.position - _nb;
		l2 = s2r.sqrMagnitude;
		d = Vector3.Dot(s2r, Vector3.up);
		r2 = Mathf.Pow(Vector3.Distance(_headToHips, Vector3.zero), 2f);
		
		if (d < 0.0f && l2 > r2)
		{
			return false;
		}
		
		m2 = (l2 - (d * d));
		if (m2 > r2)
		{
			return false;
		}
		
		q = Mathf.Sqrt(r2 - m2);
		
		
		if (l2 > r2)
		{
			t = d - q;
		}
		else
		{
			t = d + q;
		}
		
		Vector3 v = Vector3.up * t;
		
		hitPoint = _nb + v;
		return true;
	}
	
	void SolveSpineAndOthers()
	{
		Vector3 dir = (_headJoint.position - _hipJoint.position).normalized;
		
		_hipJoint.up = dir;
		
		_spines[0].position = _hipJoint.position +  _hipJoint.rotation * _baseSpineOrigOffset;
		_spines[1].position = _spines[0].position + _hipJoint.rotation * SpineOffset;
		_spines[2].position = _spines[1].position + _hipJoint.rotation * SpineOffset;
		
		//SPINES[1] POSITION? EXTRA BEND IN HIPS?
		float la = _leftFootJoint.rotation.eulerAngles.y;
		float ra = _rightFootJoint.rotation.eulerAngles.y;
		float ny = ((Mathf.Abs(la - ra) > 180) ? (la + ra + 360) : (la + ra)) / 2f;
		ny = ny % 360;
		_hipJoint.Rotate(ny * Vector3.up);
		
		Vector3 o = -0.05f * ((_headJoint.eulerAngles.x > 180f ? _headJoint.eulerAngles.x -360f : _headJoint.eulerAngles.x) / 30f)  * this.transform.forward;
		
		_spines[1].position += o;
		_spines[2].position += o;
		_spines [2].rotation = _hipJoint.rotation;
		
		if (_rightFootJoint.position.y < 0.35) {
			if (_rightFootJoint.position.y < 0.15)
				_rightFootJoint.position = new Vector3(_rightFootJoint.position.x, 0.15f, _rightFootJoint.position.z);
			Vector3 fxz = Vector3.Scale (_rightFootJoint.transform.forward, Vector3.right + Vector3.forward).normalized;
			_rightFootJoint.transform.forward = Vector3.Lerp(fxz,_rightFootJoint.transform.forward,(_rightFootJoint.position.y-.15f)/0.2f);
		}
		if (_leftFootJoint.position.y < 0.35) {
			if (_leftFootJoint.position.y < 0.15)
				_leftFootJoint.position = new Vector3(_leftFootJoint.position.x, 0.15f, _leftFootJoint.position.z);
			Vector3 fxz = Vector3.Scale (_leftFootJoint.transform.forward, Vector3.right + Vector3.forward).normalized;
			_leftFootJoint.transform.forward = Vector3.Lerp(fxz,_leftFootJoint.transform.forward,(_leftFootJoint.position.y-.15f)/0.2f);
		}
		
		//SPINES[1] POSITION? EXTRA BEND IN HIPS?

		//float rx = _headJoint.eulerAngles.x;
		//_hipJoint.Rotate((rx < 180 ? rx : rx - 360) * Vector3.right * -0.5f);
		
		float d, nks;
		
		//ELBOWS AND KNEES
		//d = Vector3.Distance(_shoulders[0].position, _leftHandJoint.position);
		//nks = Mathf.Clamp((_armDist / d) * elbowScale, minimumMidOffset,  maximumMidOffset);
		//_elbows[0] = ((_shoulders[0].position + _leftHandJoint.position) / 2f) + (nks * elbowDirection);
		
		//d = Vector3.Distance(_shoulders[1].position, _rightHandJoint.position);
		//nks = Mathf.Clamp((_armDist / d) * elbowScale, minimumMidOffset, maximumMidOffset); 
		//_elbows[1] = ((_shoulders[1].position + _rightHandJoint.position) / 2f) + (nks * elbowDirection);
		
		d = Vector3.Distance(_uplegs[0].position,_leftFootJoint.position);
		nks = Mathf.Clamp((_legDist / d) * kneeScale, minimumMidOffset, maximumMidOffset); 
		_knees[0] = ((_uplegs[0].position + _leftFootJoint.position) / 2f) + (nks * _leftFootJoint.forward);
		
		d = Vector3.Distance(_uplegs[1].position, _rightFootJoint.position);
		nks = Mathf.Clamp((_legDist / d) * kneeScale, minimumMidOffset, maximumMidOffset); 
		_knees[1] = ((_uplegs[1].position + _rightFootJoint.position) / 2f) + (nks * _rightFootJoint.forward);
		
		
		//Lines!

	
		RebuildLine(_uplegs[0].gameObject, new Vector3[] { _uplegs[0].position, _knees[0], mocapLeftAnkle.position }, mocapLeftAnkle.gameObject);
		RebuildLine(_uplegs[1].gameObject, new Vector3[] { _uplegs[1].position, _knees[1], mocapRightAnkle.position }, mocapRightAnkle.gameObject);
		
		//Spine!
		//RebuildLine(_spines[0].gameObject, new Vector3[] { /*_spines[0].position, _spines[1].position, _spines[2].position,*/ _headJoint.position }, mocapHeadset.gameObject);
		IKArm(_shoulders[0], _elbows[0], _leftHandJoint,  upperArmLength*_averageHeadHeight, lowerArmLength*_averageHeadHeight,false);
		IKArm(_shoulders[1], _elbows[1], _rightHandJoint, upperArmLength*_averageHeadHeight, lowerArmLength*_averageHeadHeight,true);
		
		RebuildLine(_shoulders[0].gameObject, new Vector3[] { _shoulders[0].position, _elbows[0].position, mocapLeftWrist.position }, mocapLeftWrist.gameObject);
		RebuildLine(_shoulders[1].gameObject, new Vector3[] { _shoulders[1].position, _elbows[1].position, mocapRightWrist.position }, mocapRightWrist.gameObject);
	}
	
	
	
	
	void RebuildLine(GameObject g, Vector3[] points, GameObject e)
	{

		if (!renderLines)
			return;

		LineRenderer r = g.GetComponent<LineRenderer>();
		
		if (!r)
		{
			r = g.AddComponent<LineRenderer>();
			r.SetWidth(LimbWidth, LowerLimbWidth);
			r.material = LimbsMaterial;
			r.material.color = lineColor;
		}
		
		Vector3[] interpPoints = getPoints (points);
		
		
		int q = 0;
		for (int i = 0; i < interpPoints.Length; i++)
		{
			r.SetVertexCount(i + 1);
			r.SetPosition(i, interpPoints[i]);
			q = i;
		}
		//
		//		int q = 0;
		//		for (int i = 0; i < points.Length; i++)
		//		{
		//			r.SetVertexCount(i + 1);
		//			r.SetPosition(i, points[i]);
		//			q = i;
		//		}
		//		
		r.SetVertexCount(q + 1);
		r.SetPosition(q, e.transform.position);
	}
	
	Vector3 interp(Vector3[] P, float t){
		return Vector3.Lerp (Vector3.Lerp (P [0], P [1], t), Vector3.Lerp (P [1], P [2], t), t);
	}
	
	
	Vector3[] getPoints(Vector3[] points){
		Vector3[] returnPoints = new Vector3[lineDetail];
		for (int i = 0; i < lineDetail; i++) {
			if(points.Length<=3)
				returnPoints[i] = interp(points,(float)i/(lineDetail-1));
			else
				returnPoints[i] = interp(points,(float)i/(lineDetail-1));
		}
		return returnPoints;
	}
	
	Vector3 C, D;
	float cc, x, y;
	
	/*
	void IKArm(Transform startJoint, Transform midJoint, Transform endJoint, float a, float b, bool rightArm)
	{
		
		C = endJoint.position - startJoint.position;
		D = findHint(C);
		
		if (!rightArm)
		{
			D = new Vector3(D.x, D.y, D.z * -1f);
		}
		
		//smoothing
		float t = Mathf.Sqrt(Vector3.Dot(C,C)) / (a + b) - .2f;
		t = Mathf.Max(0, Mathf.Min(1, t * t * (3 - t - t)));
		t = .9f + .2f * Mathf.Sqrt(t);
		a *= t;
		b *= t;
		//smoothing
		
		cc = Vector3.Dot(C, C);
		x = (1 + (a * a - b * b) / cc) / 2;
		y = Vector3.Dot(C, D) / cc;
		
		D -= y * C; 
		
		y = Mathf.Sqrt(Mathf.Max(0, a * a - cc * x * x) / Vector3.Dot(D, D));
		
		D = x * C + y * D;
		
		midJoint.position = D + startJoint.position;
	}
	*/
	
	void IKArm(Transform startJoint, Transform midJoint, Transform endJoint, float a, float b, bool rightArm)
	{
		
		GameObject tempEnd = startJoint.GetChild (1).gameObject;
		tempEnd.transform.position = endJoint.transform.position;
		Debug.Log(tempEnd.gameObject.name);
		C = tempEnd.transform.localPosition;//endJoint.position - startJoint.position;
		D = findHint(C,true);
		
		if (rightArm)
		{
			D = findHint(C,false);
			//			D = new Vector3(D.x, D.y, D.z * -1f);
		}
		
		//smoothing
		float t = Mathf.Sqrt(Vector3.Dot(C,C)) / (a + b) - .2f;
		t = Mathf.Max(0, Mathf.Min(1, t * t * (3 - t - t)));
		t = .9f + .2f * Mathf.Sqrt(t);
		a *= t;
		b *= t;
		//smoothing
		
		cc = Vector3.Dot(C, C);
		x = (1 + (a * a - b * b) / cc) / 2;
		y = Vector3.Dot(C, D) / cc;
		
		D -= y * C; 
		
		y = Mathf.Sqrt(Mathf.Max(0, a * a - cc * x * x) / Vector3.Dot(D, D));
		
		D = x * C + y * D;
		
		midJoint.localPosition = D;// + startJoint.position;
	}
	
	public Vector3 findHint(Vector3 endEffectorPosition,bool right)
	{
		
		float r = Mathf.Sqrt(0.5f);
		Vector3 c = endEffectorPosition;
		
		float[] C = { c.x, c.y, c.z };
		
		float[, ,] map = new float[,,]{
			{{  1,  0,  0 }  ,  { 0, -r, -r }},
			{{ -1,  0,  0 }  ,  { 0,  0,  1 }},
			{{  0,  1,  0 }  ,  { 1,  0,  0 }},
			{{  0, -1,  0 }  ,  { r,  0, -r }},
			{{  0,  0,  1 }  ,  { r, -r,  0 }},
			{{  r,  0,  r }  ,  { r,  0, -r }},
			{{ -r,  0,  r }  ,  { r,  0,  r }},
		};
		
		if(right){
			map = new float[,,]{
				{{  -1,  0,  0 }  ,  { 0, -r, -r }},
				{{ 1,  0,  0 }  ,  { 0,  0,  1 }},
				{{  0,  1,  0 }  ,  { -1,  0,  0 }},
				{{  0, -1,  0 }  ,  { -r,  0, -r }},
				{{  0,  0,  1 }  ,  { -r, -r,  0 }},
				{{  -r,  0,  r }  ,  { -r,  0, -r }},
				{{ r,  0,  r }  ,  { -r,  0,  r }},
			};
		}
		
		float[] D = { 0, 0, 0 };
		
		for (int n = 0; n < 7; n++)
		{
			
			float[] thisMap = { map[n, 0, 0], map[n, 0, 1], map[n, 0, 2] };
			float d = dot(thisMap, C);
			if (d > 0)
				for (int j = 0; j < 3; j++)
					D[j] += d * map[n, 1, j];
			
		}
		
		normalize(D);
		return new Vector3(D[0], D[1], D[2]);
	}
	
	float dot(float[] a, float[] b)
	{
		return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
	}
	
	float[] normalize(float[] a)
	{
		float length = Mathf.Sqrt((a[0] * a[0]) + (a[1] * a[1]) + (a[2] * a[2]));
		float[] r = { a[0] / length, a[1] / length, a[2] / length };
		return (r);
	}
	
	
	void OnDrawGizmos()
	{
		if (showGizmos)
		{
			
			Gizmos.color = Color.yellow;
			
			Gizmos.DrawWireSphere(_nb, 0.05f);
			
			//Gizmos.DrawWireSphere(_elbows[0].position, 0.05f);
			//Gizmos.DrawWireSphere(_elbows[1].position, 0.05f);
			
			Gizmos.DrawWireSphere(_knees[0], 0.05f);
			Gizmos.DrawWireSphere(_knees[1], 0.05f);
		}
	}
	
	void averageHeadHeight(){
		
		if (_headHeights == null) {
			_headHeights = new float[1000];
			for (int i = 0; i < _headHeights.Length-1; i++) {
				_headHeights[i] = _headJoint.transform.position.y;
			}
		}
		
		for (int i = 0; i < _headHeights.Length-1; i++) {
			_headHeights[i] = _headHeights[i+1];
		}
		_headHeights [_headHeights.Length - 1] = _headJoint.transform.position.y;
		
		_averageHeadHeight = 0;
		
		for (int i = 0; i < _headHeights.Length; i++) {
			_averageHeadHeight += _headHeights[i];
		}
		
		_averageHeadHeight /= _headHeights.Length;
		
	}
	
	Vector3 interpb3(Vector3[] points, float t){
		
		Vector3 vector = new Vector3();
		
		vector.x = b3( t, points[0].x, points[1].x, points[2].x, points[3].x );
		vector.y = b3( t, points[0].y, points[1].y, points[2].y, points[3].y );
		vector.z = b3( t, points[0].z, points[1].z, points[2].z, points[3].z );
		
		return vector;
		
	}
	
	// Cubic Bezier Functions
	
	float b3p0 ( float t, float p ) {
		
		float k = 1 - t;
		return k * k * k * p;
		
	}
	
	float b3p1 ( float t, float p ) {
		
		float k = 1 - t;
		return 3 * k * k * t * p;
		
	}
	
	float b3p2 ( float t,float  p ) {
		
		float k = 1 - t;
		return 3 * k * t * t * p;
		
	}
	
	float b3p3 ( float t, float p ) {
		
		return t * t * t * p;
		
	}
	
	float b3 ( float t, float p0, float p1, float p2, float p3 ) {
		return b3p0( t, p0 ) + b3p1( t, p1 ) + b3p2( t, p2 ) +  b3p3( t, p3 );
	}
}
