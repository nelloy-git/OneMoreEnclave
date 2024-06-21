@tool
extends EditorPlugin

class_name Primitive3D;

static func addLine(mesh: ImmediateMesh,
                    pos1: Vector3,
                     pos2: Vector3,
                     color: Color=Color.WHITE_SMOKE):
    var material := getLineMaterial(color);

    mesh.surface_begin(Mesh.PRIMITIVE_LINES, material)
    mesh.surface_add_vertex(pos1)
    mesh.surface_add_vertex(pos2)
    mesh.surface_end()
    
    return mesh

static var line_material: Dictionary = {};
static func getLineMaterial(color: Color) -> ORMMaterial3D:
    if (not line_material.has(color)):
        line_material[color] = ORMMaterial3D.new();
        line_material[color].shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
        line_material[color].albedo_color = color
    return line_material[color] as ORMMaterial3D;
