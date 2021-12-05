using UnityEditor;
using UnityEngine;

namespace MYgame.Scripts.Editor
{
    public class BuildingRotater : UnityEditor.Editor
    {
        [MenuItem("Tools/Randomly Rotate Selected Buildings %&r", false, 200)]
        public static void RotateSelectedBuildings()
        {
            Undo.RecordObjects(Selection.transforms, "Random Rotation");
            foreach (var transform in Selection.transforms)
                transform.localRotation =
                    Quaternion.Euler(0, Random.Range(0, 3) * 90, 0);
        }
    }
}
