@tool
extends EditorPlugin

const node_name = "MeshGridCustom"
const mesh_grid_script = preload ("res://addons/MeshGridCustom/MeshGridCustom.gd")
var active_mesh_list: GridContainer = GridContainer.new();

func _enter_tree():
	active_mesh_list.visible = false
	pass

func _exit_tree():
	active_mesh_list.visible = false
	pass

func _handles(object: Object):
	if (object is MeshGridCustom):
		print("It is MeshGridCustom")
