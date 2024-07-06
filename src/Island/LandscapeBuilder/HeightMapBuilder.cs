using Godot;
using Godot.Collections;
using Utils;

namespace Island {
    [Tool]
    [GlobalClass]
    public partial class HeightMapBuilder : Resource {
        [Export]
        public int Width = 17;
        [Export]
        public int Height = 17;
        [Export]
        public AdditiveExpr Expression = GetDefaultExpr();
        [Export]
        public Noise Noise = new FastNoiseLite();

        static private AdditiveExpr GetDefaultExpr() {
            AdditiveExpr expr = new() {
                Inputs = new() {
                    new() {Name = "V", Second = 0},
                    new() {Name = "X", Second = 0},
                    new() {Name = "Y", Second = 0},
                    new() {Name = "W", Second = 1},
                    new() {Name = "H", Second = 1}
                },
                Params = new(),
                Return = "0"
            };
            return expr;
        }

        public HeightMap Build() {
            if (!Expression.Validate()) {
                return null;
            }

            HeightMap map = new();

            Dictionary input = new() {
                {"V", 0},
                {"X", 0},
                {"Y", 0},
                {"W", Width},
                {"H", Height}
            };
            for (int x = 0; x < Width; ++x) {
                map.Data.Add(new());
                for (int y = 0; y < Height; ++y) {
                    var v = (Noise.GetNoise2D(x, y) + 1.0) / 2;

                    input["V"] = v;
                    input["X"] = x;
                    input["Y"] = y;

                    Variant variant = Expression.Execute(input);
                    switch (variant.VariantType) {
                        case Variant.Type.Int:
                            map.Data[x].Add(variant.AsInt32());
                            break;
                        case Variant.Type.Float:
                            map.Data[x].Add(variant.AsSingle());
                            break;
                        default:
                            GD.PushError("Received unsupported type: ", variant.VariantType);
                            return null;
                    }
                }
            }

            return map;
        }
    }

}