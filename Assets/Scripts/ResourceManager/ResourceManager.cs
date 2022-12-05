using System.IO;
using KefirTest.ResourceManager.Interface;
using KefirTest.UI.Interface;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.ResourceManager
{
    public class ResourceManager : IGameEntityResourceManager, IWindowResourceManager
    {
        private const string ResourceFolder = "";
        
        private static readonly string _entetiesFolder = Path.Combine(ResourceFolder, "Entity");
        private static readonly string _windowsFolder = Path.Combine(ResourceFolder, "Window");
        
        public T GetEntity<T>(string name) where T : GameEntity
        {
            string entityName = Path.Combine(_entetiesFolder, name);
            T resource = Resources.Load<T>(entityName);

            return resource;
        }

        public T GetWindowView<T>(Transform parent) where T : IWindowView
        {
            string windowName = Path.Combine(_windowsFolder, typeof(T).Name);
            Object windowResource = Resources.Load(windowName);
            GameObject windowObject = (GameObject) Object.Instantiate(windowResource, parent);
            T windowView = windowObject.GetComponent<T>();

            return windowView;
        }
    }
}