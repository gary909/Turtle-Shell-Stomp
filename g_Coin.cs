using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

// This is used instead of the MM coin.cs script.  USe in conjunction 
// with g_BonusBlock so the coin launches out of the block and coins collected
// animated properly etc

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Items/g_Coin")]
    public class g_Coin : MonoBehaviour
    {
        [Header("Coin")]
        public int PointsToAdd = 1;

        // This method is called when the block's movement is complete
        public void Disappear()
        {
            // Trigger the points event
            CorgiEnginePointsEvent.Trigger(PointsMethods.Add, PointsToAdd);

            // Destroy the coin game object
            Destroy(gameObject);
        }
    }
}
