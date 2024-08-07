using Godot;
using Godot.Collections;

namespace UI {

    public partial class BuildingTileMesh : Decal {

        public BuildingTileMesh(Image image) {
            // _Width = image.GetWidth();
            // _Height = image.GetHeight();
            // _Decal = new Decal() {
            Size = new Vector3(image.GetWidth(), 20, image.GetHeight());
            TextureAlbedo = ImageTexture.CreateFromImage(image);
            // };

            // int c = 0;
            // for (int y = 0; y < _Height; ++y) {
            //     _Decals.Add(new());
            //     for (int x = 0; x < _Width; ++x) {
            //         var decal = new Decal() {
            //             Position = new Vector3(x, 0, y),
            //             Size = new Vector3(1, 20, 1),
            //             TextureAlbedo = new GradientTexture1D() {
            //                 Gradient = new Gradient() {
            //                     InterpolationMode = Gradient.InterpolationModeEnum.Constant,
            //                     Offsets = new float[2] { 0, 0 },
            //                     Colors = new Color[2] { image.GetPixel(x, y), image.GetPixel(x, y) },
            //                 }
            //             }
            //         };
            //         AddChild(decal);
            //         _Decals[y].Add(decal);
            //         ++c;
            //     }
            // }
            // GD.Print("Decals.Count: ", c);
        }

        // private static GradientTexture1D _Texture;

        // private int _Width;
        // private int _Height;
        private Decal _Decal = null;
        private Array<Array<Decal>> _Decals = new();
    }

}