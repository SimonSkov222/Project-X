using UnityEngine;
using UnityEditor;


////////////////////////////////////////////////////////////////////
//                  Beskrivelse
//
//      Option box der kan holde på en prefab med apply og remove knap
//      har unik id så man kan kende forskel.
//
////////////////////////////////////////////////////////////////////
namespace Assets.Plugins.PrefabChanger.Scripts
{

    public class PrefabOption
    {
        ///////////////////////////////
        //      Private Static Fields
        ///////////////////////////////
        #region

        private static int indexCounter = 0;

        #endregion

        ///////////////////////////////
        //      Public Fields
        ///////////////////////////////
        #region

        public int index;
        public GameObject prefab;

        #endregion

        ///////////////////////////////
        //      Private Fields
        ///////////////////////////////
        #region

        private ButtonInfo makeChange;
        private ButtonInfo remove;
        public Vector2 margin;

        #endregion

        ///////////////////////////////
        //      Public Metods
        ///////////////////////////////
        #region

        /// <summary>
        /// Giver vores fields værdier
        /// </summary>
        public PrefabOption(ButtonInfo.OnClickDelegate onMakeChange = null, ButtonInfo.OnClickDelegate onRemove = null)
        {
            index = ++indexCounter;
            this.margin = Vector2.zero;
            this.prefab = null;
            this.makeChange = new ButtonInfo(index, "Apply", new Vector2(115, 55), onMakeChange);
            this.remove = new ButtonInfo(index, "Remove", new Vector2(115, 80), onRemove);
        }

        /// <summary>
        /// Udseende for option box
        /// </summary>
        public void Display(float width)
        {
            //Gem det hele i en box så vi nemt at flytte på alle elementerne
            float widthWithoutMargin = width - margin.x * 2;
            GUI.BeginGroup(new Rect(margin, new Vector2(widthWithoutMargin, 100)));

            //Gør at man kan vælge prefab
            prefab = (GameObject)EditorGUI.ObjectField(new Rect(115, 5, widthWithoutMargin - 115, 18), prefab, typeof(GameObject), true);

            //Vis billedet af prefab hvis den har billedet 
            // ellers hvis en hvid box
            var picRect = new Rect(5, 5, 100, 100);
            if (prefab != null)
            {
                EditorGUI.DrawPreviewTexture(picRect, AssetPreview.GetAssetPreview(prefab));
            }
            else
            {
                EditorGUI.DrawRect(picRect, Color.white); ;
            }
            //this.makeChange.isEnabled = prefab != null;
            //this.remove.isEnabled = prefab != null;

            //Knapperne
            this.makeChange.Display(widthWithoutMargin - 115);
            this.remove.Display(widthWithoutMargin - 115);

            GUI.EndGroup();
        }

        #endregion
    }
}
