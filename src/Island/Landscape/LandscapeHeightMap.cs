using Godot;

[Tool]
[GlobalClass]
public partial class LandscapeHeightMap : Resource {
    [Export]
    public int Width {
        set {
            _Width = value;
            _Value = new float[_Width * _Height];
        }
        get { return _Width; }
    }
    private int _Width;

    [Export]
    public int Height {
        set {
            _Height = value;
            _Value = new float[_Width * _Height];
        }
        get { return _Height; }
    }
    private int _Height;

    [Export]
    public LandscapeHeightMapGenerator Generator {
        set {
            _Generator = value;
        }
        get { return _Generator; }
    }
    private LandscapeHeightMapGenerator _Generator;

    public Mesh Mesh {
        get { return _Mesh; }
    }
    private Mesh _Mesh;

    private float[] _Value;
    private LandscapeHeightMapMeshBuilder _MeshBuilder = new();

    public LandscapeHeightMap() {
        _Width = 17;
        _Height = 17;
        _Generator = new();
        _Value = new float[Width * Height];
    }

    public LandscapeHeightMap(int width, int height, LandscapeHeightMapGenerator generator) {
        _Width = width;
        _Height = height;
        _Generator = generator;
        _Value = new float[width * height];
        Update();
    }

    public float Get(int x, int y) {
        return _Value[Index(x, y, _Width)];
    }

    public void Set(int x, int y, float h) {
        _Value[Index(x, y, _Width)] = h;
    }

    public void Update(bool useRandomSeed = false) {
        Generator.Validate();

        if (useRandomSeed) {
            Generator.ApplyRandomSeeds();
        }

        for (int x = 0; x < _Width; ++x) {
            for (int y = 0; y < Height; ++y) {
                _Value[Index(x, y, _Width)] = Generator.Eval(x, y, _Width, _Height);
            }
        }
        _Mesh = _MeshBuilder.Build(this);
    }

    private static int Index(int x, int y, int w) {
        return x + y * w;
    }
}