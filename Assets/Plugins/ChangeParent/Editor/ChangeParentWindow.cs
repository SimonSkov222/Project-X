using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Assets.Plugins.ChangeParent.Editor
{
    class ChangeParentWindow : EditorWindow
    {
        [MenuItem("Window/Change Parent")]
        public static void ShowWindow()
        {
            GetWindow<PrefabChangerWindow>("Change Parent").Show();
        }
    }
}
