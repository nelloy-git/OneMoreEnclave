class_name TerrainGeneration_HeightMap
extends RefCounted;

enum GradientFunc {
    Linear,
    LinearRand,
    Squared,
    LinearHeight,
    SquaredHeight,
    LinearNoiseLinearHeight
};

var width: int;
var height: int;
var min_amp: float;
var max_amp: float;
var gradient_func: GradientFunc;
var noise: FastNoiseLite;

var _half_w: float;
var _half_h: float;
var _radius: float;
var _data: Array;

func at(x: int, y: int) -> float:
    return _data[x][y];

func toPackedData() -> PackedFloat32Array:
    var arr: PackedFloat32Array = [];
    arr.resize(width * height);

    var i: int = 0;
    for y in range(0, height):
        for x in range(0, width):
            arr.set(i, at(x, y));
            i += 1;
    return arr.duplicate();

func _to_string():
    return "%s{w:%d,h:%d,r:[%d,%d]}" % ["TerrainGeneration_HeightMap", width, height, min_amp, max_amp];

func _init(_width: int,
           _height: int,
           _min_amp: float=0,
           _max_amp: float=1.0,
           _gradient_func: GradientFunc=GradientFunc.Linear,
           _noise: FastNoiseLite=FastNoiseLite.new()):
    if (_min_amp > _max_amp):
        push_error("_min_amp > _max_amp");
    width = _width;
    height = _height;
    min_amp = _min_amp;
    max_amp = _max_amp;
    gradient_func = _gradient_func;
    noise = _noise;

    _half_w = float(width) / 2;
    _half_h = float(height) / 2;
    _radius = sqrt(_half_w * _half_w + _half_h * _half_h);
    _fillArray();

func _fillArray():
    _data = [];
    for x in range(0, width):
        _data.push_back([]);
        for y in range(0, height):
            _data[x].push_back(_generateHeight(x, y));

func _generateHeight(x: int, y: int) -> float:
    # [0, 1]
    var noise_v = (noise.get_noise_2d(x, y) + 1.0) / 2;
    noise_v = _applyGradient(noise_v, x, y);
    # [min_amp, max_amp]
    return noise_v * (max_amp - min_amp) + min_amp;

func _applyGradient(noise_v: float, x: int, y: int) -> float:
    match (gradient_func):
        GradientFunc.Linear:
            var dist = _getDist(x, y)
            return noise_v * (1 - dist / _radius);
        GradientFunc.LinearRand:
            var dist = _getDist(x, y)
            return noise_v * (randf_range(0.95, 1.05) - dist / _radius);
        GradientFunc.Squared:
            var dist = _getDist(x, y)
            return noise_v * (1 - dist / _radius * dist / _radius);
        GradientFunc.LinearHeight:
            var dist = _getDist(x, y);
            return noise_v - 1.2 * dist / _radius
        GradientFunc.SquaredHeight:
            var dist = _getDist(x, y);
            return noise_v - pow(dist / _radius, 2)
        GradientFunc.LinearNoiseLinearHeight:
            var dist = _getDist(x, y);
            return noise_v * (1 - dist / _radius) - dist / _radius

    push_error("Unknown GradientFunc");
    return 0;

func _getDist(x: int, y: int) -> float:
    var dist_x: float = abs(x - _half_w);
    var dist_y: float = abs(y - _half_h);
    return sqrt(dist_x * dist_x + dist_y * dist_y);
