using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Plugins.Decals.Scripts
{
    public static class BlitTexture2DExt
    {
        public static void Blit(this Texture2D texture, Texture2D decal, Vector2 position)
        {
            //put decal on texture
            Color[] decalColors = decal.GetPixels();
            int decalWidth = decal.width;
            int decalHeight = decal.height;

            texture.SetPixels((int)(texture.width * position.x), (int)(texture.height * position.y), decalWidth, decalHeight, decalColors);
            texture.Apply();
        }
    }
}
