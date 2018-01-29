using UnityEngine;

////////////////////////////////////////////////////////////////////
//                  Beskrivelse
//
//      En Button der kan udføre et event
//
////////////////////////////////////////////////////////////////////
namespace Assets.Plugins.PrefabChanger.Scripts
{
    public class ButtonInfo
    {
        ///////////////////////////////
        //      Public Fields
        ///////////////////////////////
        #region

        public delegate void OnClickDelegate(object sender);
        public int index;
        public bool isEnabled;
        public string title;
        public Vector2 margin;
        public OnClickDelegate onClick;

        #endregion

        ///////////////////////////////
        //      Public Metods
        ///////////////////////////////
        #region

        /// <summary>
        /// Giver vores fields værdier
        /// </summary>
        public ButtonInfo(int id, string title, Vector2 margin, OnClickDelegate onClick)
        {
            this.index = id;
            this.title = title;
            this.margin = margin;
            this.onClick = onClick;
            this.isEnabled = true;
        }


        /// <summary>
        /// Udseende for Knap
        /// </summary>
        public void Display(float width)
        {
            if (isEnabled)
            {
                if (GUI.Button(new Rect(margin, new Vector2(width - 10, 20)), title))
                {
                    onClick(this);
                }
            }
        }

        #endregion
    }
}
