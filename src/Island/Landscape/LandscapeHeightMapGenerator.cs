using System;
using Godot;
using Godot.Collections;
using Utils;

[Tool]
[GlobalClass]
public partial class LandscapeHeightMapGenerator : Resource {
    [Export]
    public Array<Noise> Noises;
    [Export]
    public AdditiveExpr Evaluator;

    public LandscapeHeightMapGenerator() {
        Noises = new() { new FastNoiseLite() };
        Evaluator = new() {
            Inputs = new(){
                new(){Name = "x", Value = 0},
                new(){Name = "y", Value = 0},
                new(){Name = "w", Value = 1},
                new(){Name = "h", Value = 1},
                new(){Name = "n0", Value = 0}
            },
            Params = new(),
            Return = "n0"
        };
    }

    public bool Validate() {
        return Evaluator.Validate();
    }

    public void ApplyRandomSeeds() {
        foreach (var noise in Noises) {
            switch (noise) {
                case FastNoiseLite fastNoise:
                    fastNoise.Seed = (int)GD.Randi();
                    break;
                default:
                    throw new ArgumentException("Unknown Noise type");
            };
        }
    }

    public float Eval(int x, int y, int w, int h) {
        Dictionary input = new() {
            {"x", x},
            {"y", y},
            {"w", w},
            {"h", h}
        };
        for (int i = 0; i < Noises.Count; ++i) {
            input["n" + i.ToString()] = Noises[i].GetNoise2D(x, y);
        }

        Variant variant = Evaluator.Execute(input);
        return variant.VariantType switch {
            Variant.Type.Int => variant.AsInt32(),
            Variant.Type.Float => variant.AsSingle(),
            _ => throw new ArgumentException("Received unsupported type: " + variant.VariantType + " with params: " + input),
        };
    }
}