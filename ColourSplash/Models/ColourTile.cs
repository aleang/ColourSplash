using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace ColourSplash.Model
{
    public class ColourTile
    {
        public int[] ColorIds { get; set; }
        public bool IsAnswer { get; set; }
        public string Name { get; set; }

        public int GetRandomColour()
        {
            return ColorIds[new Random().Next(ColorIds.Length)];
        }
    }
}