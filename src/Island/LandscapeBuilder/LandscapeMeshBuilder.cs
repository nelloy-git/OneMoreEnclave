using System;
using System.Xml.Linq;
using Godot;

namespace Island {

    public partial class LandscapeMeshBuilder {
        const int INVALID_INDEX = int.MinValue;

        public ArrayMesh Build(HeightMap height_map) {
            int size_x = height_map.GetSizeX();
            int size_y = height_map.GetSizeY();

            _surf = new();
            _height_map = height_map;
            _vert_count = 0;
            _vert_index = new int[size_x, size_y];
            for (int i = 0; i < size_x; ++i) {
                for (int j = 0; j < size_y; ++j) {
                    _vert_index[i, j] = INVALID_INDEX;
                }
            }

            _surf.Begin(Mesh.PrimitiveType.Triangles);
            for (int y = 0; y < size_y; ++y) {
                for (int x = 0; x < size_x; ++x) {
                    addVert(x, y);
                }
            }
            for (int i = 0; i < 3 - _vert_count % 3; ++i) {
                _surf.AddVertex(new Vector3(0, 0, 0));
            }

            for (int y = 0; y < (size_y - 1); ++y) {
                for (int x = 0; x < (size_x - 1); ++x) {
                    addCell(x, y);
                }
            }

            _vert_index = null;

            _surf.GenerateNormals();
            _surf.GenerateTangents();
            return _surf.Commit();
        }

        private void addVert(int x, int y) {
            if (IsValidVert(x, y)) {
                _vert_index[x, y] = _vert_count++;
                _surf.SetUV(new Vector2(0, 0));
                _surf.AddVertex(
                    new Vector3(x - _height_map.GetSizeX() / 2,
                                _height_map.Get(x, y),
                                y - _height_map.GetSizeY() / 2)
                );
            }
        }

        private void addCell(int x, int y) {
            int[] index = new int[4];
            index[0] = _vert_index[x, y];
            index[1] = _vert_index[x + 1, y];
            index[2] = _vert_index[x, y + 1];
            index[3] = _vert_index[x + 1, y + 1];

            //          0 --- 1
            //          |   / |
            //          | /   |
            //          2 --- 3
            if (index[1] != INVALID_INDEX && index[2] != INVALID_INDEX) {
                if (index[0] != INVALID_INDEX) {
                    _surf.AddIndex(index[0]);
                    _surf.AddIndex(index[1]);
                    _surf.AddIndex(index[2]);
                }
                if (index[3] != INVALID_INDEX) {
                    _surf.AddIndex(index[1]);
                    _surf.AddIndex(index[3]);
                    _surf.AddIndex(index[2]);
                }
                return;
            }

            //   0 --- 1
            //   | \   |
            //   |   \ |
            //   2 --- 3
            if (index[0] != INVALID_INDEX && index[3] != INVALID_INDEX) {
                if (index[1] != INVALID_INDEX) {
                    _surf.AddIndex(index[0]);
                    _surf.AddIndex(index[1]);
                    _surf.AddIndex(index[3]);
                }
                if (index[2] != INVALID_INDEX) {
                    _surf.AddIndex(index[0]);
                    _surf.AddIndex(index[3]);
                    _surf.AddIndex(index[2]);
                }
                return;
            }
        }

        private bool IsValidVert(int x, int y) {
            return _height_map.Get(x, y) >= 0;
        }

        private SurfaceTool _surf;
        private HeightMap _height_map;
        private int _vert_count;
        private int[,] _vert_index;
    };

}