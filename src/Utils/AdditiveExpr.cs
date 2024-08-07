using System.Linq;
using Godot;
using Godot.Collections;

namespace Utils {
    [Tool]
    [GlobalClass]
    public partial class AdditiveExpr : Resource {
        [Export]
        public bool ValidateBtn {
            get { return false; }
            set {
                if (!value) {
                    return;
                }

                if (Validate()) {
                    GD.Print("Validation successful");
                }
                else {
                    GD.Print("Validation failed");
                }
            }
        }

        [Export]
        public Array<ConstParam> Inputs;
        [Export]
        public Array<AbstractParam> Params;
        [Export]
        public string Return = "0";

        public bool Validate() {
            if (!ValidateUniqueNames()) {
                GD.PushError("ValidateUniqueNames failed");
                return false;
            }

            if (!(ParseParams() && ParseReturn())) {
                return false;
            }

            Dictionary test_input = new();
            foreach (var input in Inputs) {
                test_input[input.Name] = input.Value;
            }
            return Execute(test_input).VariantType != Variant.Type.Nil;
        }

        public bool ValidateUniqueNames() {
            Dictionary set = new();
            foreach (var input in Inputs) {
                if (set.ContainsKey(input.Name)) {
                    GD.PushError("Key duplication: ", input.Name);
                    return false;
                }
                set.Add(input.Name, true);
            }
            foreach (var param in Params) {
                if (set.ContainsKey(param.Name)) {
                    GD.PushError("Key duplication: ", param.Name);
                    return false;
                }
                set.Add(param.Name, true);
            }
            return true;
        }

        private bool ParseParams() {
            Array<string> names = new();
            foreach (var input in Inputs) {
                names.Add(input.Name);
            }

            _params = new();
            foreach (var param in Params) {
                switch (param) {
                    case ConstParam p:
                        _params[param.Name] = p.Value;
                        break;
                    case ExprParam p:
                        Expression expr = new();
                        var err = expr.Parse(p.Value, names.ToArray());
                        if (err != Error.Ok) {
                            GD.PushError(param.Name, ": ", expr.GetErrorText());
                            return false;
                        }
                        _params[param.Name] = expr;
                        break;
                    default:
                        GD.PushError("Invalid parameter type: ", param.Name);
                        return false;
                }
                names.Add(param.Name);
            }
            return true;
        }

        private bool ParseReturn() {
            Array<string> names = new();
            foreach (var input in Inputs) {
                names.Add(input.Name);
            }
            foreach (var param in Params) {
                names.Add(param.Name);
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
                if (!inputs.ContainsKey(required_input.Name)) {
                    GD.PushError("Key: \"", required_input.Name, "\" is not found");
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

            if (result.VariantType != Variant.Type.Int && result.VariantType != Variant.Type.Float) {
                GD.PushError("Invalid result type: ", result, "(", result.VariantType, ")\nInput: ", inputs);
            }

            return result;
        }

        private Dictionary<string, Variant> _params = new();
        private Expression _return = new();
    }

}