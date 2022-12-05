using KefirTest.UI.Interface;
using UnityEngine;

namespace KefirTest.ResourceManager.Interface
{
    public interface IWindowResourceManager
    {
        T GetWindowView<T>(Transform parent) where T : IWindowView;
    }
}