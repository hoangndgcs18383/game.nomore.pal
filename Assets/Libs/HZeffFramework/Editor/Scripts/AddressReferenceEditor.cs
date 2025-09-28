namespace SAGE.Framework.Core.Tools
{
    using UnityEngine.AddressableAssets;
    using UnityEditor;
    using Object = UnityEngine.Object;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AddressReferenceEditor", menuName = "SAGE/Tools/AddressReferenceEditor")]
    public partial class AddressReferenceEditor : ScriptableObject
    {
        public string group;


        public string className;


        public string getNameMethod = "GetName";

        public List<AddressReferenceConfig> addresses;

        [SerializeField] private string Folder;


        public void Insert()
        {
            addresses.Clear();
            List<Object> objects = LoadAssetsAtPath<Object>(Folder);

            for (int i = 0; i < objects.Count; i++)
            {
                AssetReference reference =
                    new AssetReference(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(objects[i])));
                addresses.Add(new AddressReferenceConfig
                {
                    key = "M" + objects[i].name,
                    isAddressable = true,
                    labels = new string[]
                    {
                        group
                    },
                    reference = reference,
                });
            }
        }

        public static List<T> LoadAssetsAtPath<T>(string path) where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { path });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
    }
}