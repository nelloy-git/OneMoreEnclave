@tool
extends EditorPlugin

class_name Primitive3D;

static func addLine(mesh: ImmediateMesh,
					pos1: Vector3,
					pos2: Vector3,
					color: Color=Color.WHITE_SMOKE):
	var material := getMaterial(color);

	mesh.surface_begin(Mesh.PRIMITIVE_LINES, material)
	mesh.surface_add_vertex(pos1)
	mesh.surface_add_vertex(pos2)
	mesh.surface_end()
	
	return mesh

static func addRect(mesh: ImmediateMesh,
					pos: Vector3,
					side1: Vector3,
					side2: Vector3,
					color: Color=Color.WHITE_SMOKE):
	var material := getMaterial(color);

	mesh.surface_begin(Mesh.PRIMITIVE_TRIANGLES, material)
	mesh.surface_add_vertex(pos);
	mesh.surface_add_vertex(side1);
	mesh.surface_add_vertex(side1 + side2 - pos);
	mesh.surface_add_vertex(pos);
	mesh.surface_add_vertex(side1 + side2 - pos);
	mesh.surface_add_vertex(side2);
	mesh.surface_end();

	pass

static func addRects(mesh: ImmediateMesh,
					 rects: Array[Primitive3dRectPos],
					 color: Color=Color.WHITE_SMOKE):
	var material := getMaterial(color);

	mesh.surface_begin(Mesh.PRIMITIVE_TRIANGLES, material)
	for i in range(0, rects.size()):
		rects[i].addSurface(mesh);
	mesh.surface_end();

	pass

static var _material: Dictionary = {};
static func getMaterial(color: Color) -> ORMMaterial3D:
	if (not _material.has(color)):
		_material[color] = ORMMaterial3D.new();
		_material[color].shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
		_material[color].albedo_color = color
	return _material[color] as ORMMaterial3D;
