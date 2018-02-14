using UnityEngine;

namespace Assets.Plugins.Decals.Scripts
{
    ////////////////////////////////////////////////////////////////////////////////
    //                      Beskrivelse
    //  
    //  ##!! Bruges ikke mere. Se DecalObject.cs beskrivelse                    !!##
    //  
    //  Denne class for extension metoder. 
    //  Kombinere to Texture2D til en Texture2D.
    //
    //  #NOTE 
    //  Dette virker ikke helt som vi havde h�bet da der 
    //  er problemet med st�rrelsesforholdet.
    //
    ////////////////////////////////////////////////////////////////////////////////
    public static class BlitTexture2DExt
    {
        ///////////////////////////////
        //      Public Static Methods
        ///////////////////////////////
        #region 

        /// <summary>
        /// Her kombinere vi de to Texture2D der skal blive til et
        /// Vi har hoved Texture2D(texture) ogs� det andet(decal) der skal tilf�jes p�
        /// en angivet position(position)
        /// </summary>
        public static void Blit(this Texture2D texture, Texture2D decal, Vector2 position)
        {
            //put decal on texture
            Color[] decalColors = decal.GetPixels();
            int decalWidth = decal.width;
            int decalHeight = decal.height;

            texture.SetPixels((int)(texture.width * position.x), (int)(texture.height * position.y), decalWidth, decalHeight, decalColors);
            texture.Apply();
        }

        #endregion
    }
}
