using System.Collections.Generic;
using Assets.Scripts.Models.Building;

namespace Assets.Scripts.Models.Animation
{
    public static class AnimationKind
    {
        public static readonly string Idle = "Idle";
        public static readonly string Walking = "Walking";
        public static readonly string Running = "Running";
        public static readonly string Mining = "Mining";
        public static readonly string FarmWorking = "FarmWorking";
    }

    public static class AnimationMapper
    {
        public static Dictionary<string, string> BuildingToAnimation = new Dictionary<string, string>
        {
            {BuildingsNames.Mine, AnimationKind.Mining},
            {BuildingsNames.Farm, AnimationKind.FarmWorking}
        };
    }
}