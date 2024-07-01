using System.Linq;
using Godot;
using Godot.Collections;

namespace Utils {
    [Tool]
    [GlobalClass]
    public partial class AdditiveExpr : Resource {
        // TODO Dictionary with signals

        [Export]
        public Dictionary Inputs = new();
        [Export]
        public Dictionary Params = new();
        [Export]
        public string Return = "0";

        [Export]
        public bool ValidateBtn {
            get { return false; }
            set {
                if (!Engine.IsEditorHint()) {
                    GD.PushError("Editor only");
                    return;
                }
                if (!value) {
                    return;
                }

                if (Validate()) {
                    GD.Print("Validation successful");
                }
            }
        }

        public bool Validate() {
            if (!(ParseInputs() && ParseParams() && ParseReturn())) {
                return false;
            }
            return Execute(Inputs).VariantType != Variant.Type.Nil;
        }

        private bool ParseInputs() {
            foreach (var input in Inputs) {
                if (input.Key.VariantType != Variant.Type.String) {
                    GD.PushError("Key requires String instead of ", input.Key);
                    return false;
                }
            }
            return true;
        }

        private bool ParseParams() {
            Array<string> names = new();
            foreach (var input in Inputs) {
                names.Add(input.Key.AsString());
            }

            _params = new();
            foreach (var param in Params) {
                if (param.Key.VariantType != Variant.Type.String) {
                    GD.PushError("Key requires String");
                    return false;
                }
                if (names.Any(name => name == param.Key.AsString())) {
                    GD.PushError("Key duplication: ", param.Key);
                    return false;
                }

                switch (param.Value.VariantType) {
                    case Variant.Type.Int:
                        _params[param.Key.AsString()] = param.Value.AsInt64();
                        break;

                    case Variant.Type.Float:
                        _params[param.Key.AsString()] = param.Value.AsSingle();
                        break;

                    case Variant.Type.String:
                        Expression expr = new();
                        var err = expr.Parse(param.Value.AsString(), names.ToArray<string>());
                        if (err != Error.Ok) {
                            GD.PushError(param.Key, ": ", expr.GetErrorText());
                            return false;
                        }
                        _params[param.Key.AsString()] = expr;
                        break;

                    default:
                        GD.PushError("Invalid parameter type: ", param.Key);
                        return false;
                }

                names.Add(param.Key.AsString());
            }
            return true;
        }

        private bool ParseReturn() {
            Array<string> names = new();
            foreach (var input in Inputs) {
                names.Add(input.Key.AsString());
            }
            foreach (var param in Params) {
                names.Add(param.Key.AsString());
            }

            Expression expr = new();
            var err = expr.Parse(Return, names.ToArray());
            if (err != Error.Ok) {
                GD.PushError("Return: ", expr.GetErrorText());
                return false;
            }
            _return = expr;

            return true;
        }

        public Variant Execute(Dictionary inputs) {
            foreach (var required_input in Inputs) {
                if (!inputs.ContainsKey(required_input.Key)) {
                    GD.PushError("Key: \"", required_input.Key, "\" is not found");
                    return new Variant();
                }
            }

            Array pipeline = new();
            pipeline.AddRange(inputs.Values.ToArray());

            foreach (var param in _params) {
                switch (param.Value.VariantType) {
                    case Variant.Type.Int:
                    case Variant.Type.Float:
                        pipeline.Add(param.Value.AsSingle());
                        break;

                    case Variant.Type.Object:
                        pipeline.Add(param.Value.As<Expression>().Execute(pipeline));
                        if (param.Value.As<Expression>().HasExecuteFailed()) {
                            GD.PushError("Can not evaluate \"", param.Key, "\": ", param.Value.As<Expression>().GetErrorText());
                            return new Variant();
                        }
                        break;

                    default:
                        GD.PushError("Invalid parameter type: ", param.Key, "(", param.Value.VariantType, ")");
                        return false;

                }
            }

            var result = _return.Execute(pipeline);
            if (_return.HasExecuteFailed()) {
                GD.PushError("Can not evaluate \"Return\": ", _return.GetErrorText());
                return new Variant();
            }

            return result;
        }

        private Dictionary<string, Variant> _params = new();
        private Expression _return = new();
    }

}