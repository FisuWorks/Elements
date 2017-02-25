using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElemCharacterController : MonoBehaviour {

	[SerializeField]
	private int walkSpeed = 5;

	/** It is necessary to comment this one, too. */
	private Animator animator;

	/** Character facing. */
	private Orientation _orientation;
	public Orientation orientation {
		get { return _orientation; }
		set {
			_orientation = value;
			setAnimatorOrientation();
		}
	}

	/** Character state of movement. */
	private MovementState _movementState;
	public MovementState movementState {
		get { return _movementState; }
		set {
			_movementState = value;
			animator.SetInteger("State", (int)value);
		}
	}

	/** Tell Mecanim our orientation in a way it can understand. */
	private void setAnimatorOrientation() {
		float moveX = 0;
		float moveY = 0;

		switch (orientation) {
			case Orientation.N: moveX = 0; moveY = 1; break;
			case Orientation.S: moveX = 0; moveY = -1; break;
			case Orientation.E: moveX = 1; moveY = 0; break;
			case Orientation.W: moveX = -1; moveY = 0; break;
			case Orientation.NW: moveX = -1; moveY = 1; break;
			case Orientation.NE: moveX = 1; moveY = 1; break;
			case Orientation.SW: moveX = -1; moveY = -1; break;
			case Orientation.SE: moveX = 1; moveY = -1; break;
			default: break;
		}

		animator.SetFloat("MoveX", moveX);
		animator.SetFloat("MoveY", moveY);
	}

	public void Awake() {
		animator = GetComponent<Animator>();
		movementState = MovementState.IDLE;
	}

	void Start () {
		
	}
	
	void Update () {
		checkMovementKeys();
	}

	void checkMovementKeys() {
		bool left = Input.GetKey(KeyCode.A);
		bool right = Input.GetKey(KeyCode.D);
		bool up = Input.GetKey(KeyCode.W);
		bool down = Input.GetKey(KeyCode.S);

		bool horizontal = left != right;
		bool vertical = up != down;

		if (horizontal && !vertical) {
			orientation = left ? Orientation.W : Orientation.E;
		}
		else if (vertical && !horizontal) {
			orientation = up ? Orientation.N : Orientation.S;
		}
		else if (vertical && horizontal) {
			if (up && left)
				orientation = Orientation.NW;
			else if (up && right)
				orientation = Orientation.NE;
			else if (down && left)
				orientation = Orientation.SW;
			else if (down && right)
				orientation = Orientation.SE;
		}

		// Move the character
		if (horizontal || vertical) {
			int moveX = (left ? -1 : 0) + (right ? 1 : 0);
			int moveY = (up ? 0 : -1) + (down ? 0 : 1);

			Vector3 dv = new Vector3(moveX, moveY, 0).normalized * walkSpeed * Time.deltaTime;
			transform.Translate(dv);

			movementState = MovementState.WALK;
		}
		else {
			movementState = MovementState.IDLE;
		}
	}
}
