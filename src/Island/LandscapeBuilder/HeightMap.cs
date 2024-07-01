using Godot;
using Godot.Collections;

namespace Island {
    [Tool]
    [GlobalClass]
    public partial class HeightMap : Resource {
        [Export]
        public Array<Array<float>> Data = new();

        public float Get(int x, int y) {
            return Data[x][y];
        }

        public void Set(int x, int y, float h) {
            Data[x][y] = h;
        }

        public int GetSizeX() {
            return Data.Count;
        }

        public int GetSizeY() {
            return Data[0].Count;
        }

        public int GetCellsHigherThan(float height) {
            int sq = 0;
            for (int x = 0; x < Data.Count - 1; ++x) {
                for (int y = 0; y < Data[x].Count - 1; ++y) {
                    if (Data[x][y] >= height
                        && Data[x + 1][y] >= height
                        && Data[x][y + 1] >= height
                        && Data[x + 1][y + 1] >= height) {
                        ++sq;
                    }
                }
            }
            return sq;
        }
    }

}