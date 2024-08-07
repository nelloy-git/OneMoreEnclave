using Godot;

public partial class LandscapeHeightMapMeshBuilder {
    const int INVALID_INDEX = int.MinValue;

    public ArrayMesh Build(LandscapeHeightMap height_map) {
        _HeightMap = height_map;
        // GD.Print("w: ", _HeightMap.Width, ", h: ", _HeightMap.Height);

        _VertCount = 0;
        _VertIndex = new int[_HeightMap.Width, _HeightMap.Height];
        for (int y = 0; y < _HeightMap.Height; ++y) {
            for (int x = 0; x < _HeightMap.Width; ++x) {
                _VertIndex[x, y] = INVALID_INDEX;
            }
        }

        _Surf.Begin(Mesh.PrimitiveType.Triangles);
        _FillVertices();
        _FillTriangles();
        _Surf.GenerateNormals();
        _Surf.GenerateTangents();
        var mesh = _Surf.Commit();
        _Surf.Clear();
        return mesh;
    }

    private void _FillVertices() {
        for (int y = 0; y < _HeightMap.Height; ++y) {
            for (int x = 0; x < _HeightMap.Width; ++x) {
                _AddVert(x, y);
            }
        }
        for (int i = 0; i < 3 - _VertCount % 3; ++i) {
            _Surf.AddVertex(new Vector3(0, 0, 0));
        }
    }

    private void _AddVert(int x, int y) {
        if (_IsValidVert(x, y)) {
            _VertIndex[x, y] = _VertCount++;
            _Surf.SetUV(new Vector2(x % 2 == 0 ? 1 : 0, y % 2 == 0 ? 0 : 1));

            Vector3 pos = new(x - _HeightMap.Width / 2,
                              _HeightMap.Get(x, y),
                              y - _HeightMap.Height / 2);
            _Surf.AddVertex(pos);
        }
    }

    private void _FillTriangles() {
        for (int y = 0; y < (_HeightMap.Height - 1); ++y) {
            for (int x = 0; x < (_HeightMap.Width - 1); ++x) {
                _AddCell(x, y);
            }
        }

    }

    private void _AddCell(int x, int y) {
        int[] index = new int[4];
        index[0] = _VertIndex[x, y];
        index[1] = _VertIndex[x + 1, y];
        index[2] = _VertIndex[x, y + 1];
        index[3] = _VertIndex[x + 1, y + 1];

        //   0 --- 1
        //   |   / |
        //   | /   |
        //   2 --- 3
        if (index[1] != INVALID_INDEX && index[2] != INVALID_INDEX) {
            if (index[0] != INVALID_INDEX) {
                _Surf.AddIndex(index[0]);
                _Surf.AddIndex(index[1]);
                _Surf.AddIndex(index[2]);
            }
            if (index[3] != INVALID_INDEX) {
                _Surf.AddIndex(index[1]);
                _Surf.AddIndex(index[3]);
                _Surf.AddIndex(index[2]);
            }
            return;
        }

        //   0 --- 1
        //   | \   |
        //   |   \ |
        //   2 --- 3
        if (index[0] != INVALID_INDEX && index[3] != INVALID_INDEX) {
            if (index[1] != INVALID_INDEX) {
                _Surf.AddIndex(index[0]);
                _Surf.AddIndex(index[1]);
                _Surf.AddIndex(index[3]);
            }
            if (index[2] != INVALID_INDEX) {
                _Surf.AddIndex(index[0]);
                _Surf.AddIndex(index[3]);
                _Surf.AddIndex(index[2]);
            }
            return;
        }
    }

    private bool _IsValidVert(int x, int y) {
        return _HeightMap.Get(x, y) >= 0;
    }

    private SurfaceTool _Surf = new();
    private LandscapeHeightMap _HeightMap;
    private int _VertCount;
    private int[,] _VertIndex;
};
