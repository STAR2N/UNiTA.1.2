namespace Game.CMF {
	using global::CMF;
	using UnityEngine;

	//This script rotates a gameobject based on user input.
	//Rotation around the x-axis (vertical) can be clamped/limited by setting 'upperVerticalLimit' and 'lowerVerticalLimit'.
	public class CameraController : MonoBehaviour {
      	const int ROTATE_MOUSE_BUTTON = 0;
        bool enableRotation = false;
        public bool EnableRotation
        {
            get => enableRotation;
            set
            {
                enableRotation = value;
                // Cursor.lockState = enableRotation ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }

		[Header("Components")]
		public Transform TargetTransform;

		[SerializeField] Camera m_Camera;
		public Camera Camera => m_Camera;
		[SerializeField] CameraInput m_Input;

		[Header("Keys")]
		public KeyCode MouseLock = KeyCode.X;

		[Header("Options")]
		//Upper and lower limits (in degrees) for vertical rotation (along the local x-axis of the gameobject);
		[Range(0f, 90f)] public float m_UpperVerticalLimit = 60f;
		[Range(0f, 90f)] public float m_LowerVerticalLimit = 60f;

		//Camera turning speed; 
		public float CameraSpeed = 200f;

		//Whether camera rotation values will be smoothed;
		public bool SmoothCameraRotation = false;

		//This value controls how smoothly the old camera rotation angles will be interpolated toward the new camera rotation angles;
		//Setting this value to '50f' (or above) will result in no smoothing at all;
		//Setting this value to '1f' (or below) will result in very noticable smoothing;
		//For most situations, a value of '25f' is recommended;
		[Range(1f, 50f)]
		public float CameraSmoothingFactor = 25f;

        #region Runtime variables
        //Current rotation values (in degrees);
        Vector2 m_CurrentAngle = Vector2.zero;

		//Variables to store old rotation values for interpolation purposes;
		float oldHorizontalInput = 0f;
		float oldVerticalInput = 0f;

		//Variables for storing current facing direction and upwards direction;
		Vector3 facingDirection;
		Vector3 upwardsDirection;
        #endregion

        //Setup references.
        void Awake() {
			if (m_Input == null)
				Debug.LogWarning("No camera input script has been attached to this gameobject", this.gameObject);

			//If no camera component has been attached to this gameobject, search the transform's children;
			if (Camera == null)
				m_Camera = GetComponentInChildren<Camera>();

			if (TargetTransform) {
				//Set angle variables to current rotation angles of this transform;
				m_CurrentAngle.x = TargetTransform.localRotation.eulerAngles.x;
				m_CurrentAngle.y = TargetTransform.localRotation.eulerAngles.y;
			}

			//Execute camera rotation code once to calculate facing and upwards direction;
			RotateCamera(0f, 0f);

// #if !(UNITY_ANDROID || UNITY_IOS)
// 			if (isActiveAndEnabled)
// 	            Cursor.lockState = CursorLockMode.Locked;
// #endif

			Setup();
		}

		//This function is called right after Awake(); It can be overridden by inheriting scripts;
		protected virtual void Setup() {

		}

		void Update() {
			HandleCameraRotation();
		}

        private void OnDestroy() {
			if (Cursor.lockState == CursorLockMode.Locked)
				Cursor.lockState = CursorLockMode.None;
		}

        //Get user input and handle camera rotation;
        //This method can be overridden in classes derived from this base class to modify camera behaviour;
        protected virtual void HandleCameraRotation() {
// 			if (UnityEngine.Input.GetKeyDown(MouseLock)) {
// 				Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
// 			}


// 			if (m_Input == null
// #if !(UNITY_ANDROID || UNITY_IOS)
// 				|| Cursor.lockState != CursorLockMode.Locked
// #endif
// 				)
// 				return;

			if(TutorialManager.Instance) {
				if(!TutorialManager.Instance.isDone) {
					return;
				}
			}

			if(LetterMaster.Instance) {
            	if(LetterMaster.Instance.IsWriting) {
                	return ;
				}
			}

			var pressing = UnityEngine.Input.GetMouseButton(ROTATE_MOUSE_BUTTON);
			if (pressing != EnableRotation)
			{
				EnableRotation = pressing;
			}

			//Get input values;
			float _inputHorizontal = m_Input.GetHorizontalCameraInput();
			float _inputVertical = m_Input.GetVerticalCameraInput();

			RotateCamera(_inputHorizontal, _inputVertical);
		}

		//Rotate camera; 
		protected void RotateCamera(float _newHorizontalInput, float _newVerticalInput) {
			if(!EnableRotation)
				return;
			
			if (SmoothCameraRotation) {
				//Lerp input;
				oldHorizontalInput = Mathf.Lerp(oldHorizontalInput, _newHorizontalInput, Time.deltaTime * CameraSmoothingFactor);
				oldVerticalInput = Mathf.Lerp(oldVerticalInput, _newVerticalInput, Time.deltaTime * CameraSmoothingFactor);
			} else {
				//Replace old input directly;
				oldHorizontalInput = _newHorizontalInput;
				oldVerticalInput = _newVerticalInput;
			}

			//Add input to camera angles;
			m_CurrentAngle.x += oldVerticalInput * CameraSpeed * Time.deltaTime;
			m_CurrentAngle.y += oldHorizontalInput * CameraSpeed * Time.deltaTime;

			//Clamp vertical rotation;
			m_CurrentAngle.x = Mathf.Clamp(m_CurrentAngle.x, -m_UpperVerticalLimit, m_LowerVerticalLimit);

			UpdateRotation();
		}

		//Update camera rotation based on x and y angles;
		protected void UpdateRotation() {
			if (TargetTransform == null)
				return;

			TargetTransform.localRotation = Quaternion.Euler(new Vector3(0, m_CurrentAngle.y, 0));

			//Save 'facingDirection' and 'upwardsDirection' for later;
			facingDirection = TargetTransform.forward;
			upwardsDirection = TargetTransform.up;

			TargetTransform.localRotation = Quaternion.Euler(new Vector3(m_CurrentAngle.x, m_CurrentAngle.y, 0));
		}

		//Set the camera's field-of-view (FOV);
		public void SetFOV(float _fov) {
			if (Camera)
				Camera.fieldOfView = _fov;
		}

		//Set x and y angle directly;
		public void SetRotationAngles(float _xAngle, float _yAngle) {
			m_CurrentAngle.x = _xAngle;
			m_CurrentAngle.y = _yAngle;

			UpdateRotation();
		}

		//Rotate the camera toward a rotation that points at a world position in the scene;
		public void RotateTowardPosition(Vector3 _position, float _lookSpeed) {
			//Calculate target look vector;
			Vector3 _direction = (_position - TargetTransform.position);

			RotateTowardDirection(_direction, _lookSpeed);
		}

		//Rotate the camera toward a look vector in the scene;
		public void RotateTowardDirection(Vector3 _direction, float _lookSpeed) {
			//Normalize direction;
			_direction.Normalize();

			//Transform target look vector to this transform's local space;
			_direction = TargetTransform.parent.InverseTransformDirection(_direction);

			//Calculate (local) current look vector; 
			Vector3 _currentLookVector = GetAimingDirection();
			_currentLookVector = TargetTransform.parent.InverseTransformDirection(_currentLookVector);

			//Calculate x angle difference;
			float _xAngleDifference = VectorMath.GetAngle(new Vector3(0f, _currentLookVector.y, 1f), new Vector3(0f, _direction.y, 1f), Vector3.right);

			//Calculate y angle difference;
			_currentLookVector.y = 0f;
			_direction.y = 0f;
			float _yAngleDifference = VectorMath.GetAngle(_currentLookVector, _direction, Vector3.up);

			//Turn angle values into Vector2 variables for better clamping;
			Vector2 _currentAngles = new Vector2(m_CurrentAngle.x, m_CurrentAngle.y);
			Vector2 _angleDifference = new Vector2(_xAngleDifference, _yAngleDifference);

			//Calculate normalized direction;
			float _angleDifferenceMagnitude = _angleDifference.magnitude;
			if (_angleDifferenceMagnitude == 0f)
				return;
			Vector2 _angleDifferenceDirection = _angleDifference/_angleDifferenceMagnitude;

			//Check for overshooting;
			if (_lookSpeed * Time.deltaTime > _angleDifferenceMagnitude) {
				_currentAngles += _angleDifferenceDirection * _angleDifferenceMagnitude;
			} else
				_currentAngles += _angleDifferenceDirection * _lookSpeed * Time.deltaTime;

			//Set new angles;
			m_CurrentAngle.y = _currentAngles.y;
			//Clamp vertical rotation;
			m_CurrentAngle.x = Mathf.Clamp(_currentAngles.x, -m_UpperVerticalLimit, m_LowerVerticalLimit);

			UpdateRotation();
		}

		public float GetCurrentXAngle() {
			return m_CurrentAngle.x;
		}

		public float GetCurrentYAngle() {
			return m_CurrentAngle.y;
		}

		//Returns the direction the camera is facing, without any vertical rotation;
		//This vector should be used for movement-related purposes (e.g., moving forward);
		public Vector3 GetFacingDirection() {
			return facingDirection;
		}

		//Returns the 'forward' vector of this gameobject;
		//This vector points in the direction the camera is "aiming" and could be used for instantiating projectiles or raycasts.
		public Vector3 GetAimingDirection() {
			return TargetTransform.forward;
		}

		// Returns the 'right' vector of this gameobject;
		public Vector3 GetStrafeDirection() {
			return TargetTransform.right;
		}

		// Returns the 'up' vector of this gameobject;
		public Vector3 GetUpDirection() {
			return upwardsDirection;
		}

		public void comm_SetCursorVisible() {
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
