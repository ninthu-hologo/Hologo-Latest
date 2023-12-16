using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 july 2019
    /// Modified by: 
    /// interface for ui elements that need to be dynamically instantiated and filled with data.
    /// </summary>
    public interface IUIElement
    {
      //  Task fillWithData(IdataObject data, params CallbackGameObject[] actions);
        void fillWithDataSync(IdataObject data, params CallbackGameObject[] actions);
        IdataObject getData();
    }
}
