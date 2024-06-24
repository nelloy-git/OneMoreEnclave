extends Camera3D

@export var velocity: float = 0.1

func _input(event):
	if (event is InputEventKey):
		var dir: Vector3 = get_global_transform().basis.z;
		dir.y = 0;
		dir = dir.normalized();

		var key = event.keycode;
		if (key == KEY_W):
			position += - dir;
		elif (key == KEY_S):
			position += dir;
		elif (key == KEY_A):
			dir = dir.rotated(Vector3(0, 1, 0), -PI / 2);
			position += dir;
		elif (key == KEY_D):
			dir = dir.rotated(Vector3(0, 1, 0), PI / 2);
			position += dir;
