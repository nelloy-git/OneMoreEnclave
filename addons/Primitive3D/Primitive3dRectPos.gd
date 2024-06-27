extends RefCounted

class_name Primitive3dRectPos;

var pos: Vector3;
var side1: Vector3;
var side2: Vector3;

func addSurface(mesh: ImmediateMesh):
    mesh.surface_add_vertex(pos);
    mesh.surface_add_vertex(side1);
    mesh.surface_add_vertex(side1 + side2 - pos);
    mesh.surface_add_vertex(pos);
    mesh.surface_add_vertex(side1 + side2 - pos);
    mesh.surface_add_vertex(side2);