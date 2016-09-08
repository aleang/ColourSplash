using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using ColourSplash.Model;

namespace ColourSplash
{
    public class ColourTileRepository
    {
        public List<ColourTile> GetNewSetOfTiles()
        {
            ColourTiles.ForEach(t => t.IsAnswer = false);
            ColourTile[] list = new ColourTile[ColourTiles.Count]; 
            ColourTiles.CopyTo(list);

            var resultList = list.ToList();
            var rand = new Random();
            while (resultList.Count > 4)
            {
                resultList.RemoveAt(rand.Next(resultList.Count));
            }

            resultList[rand.Next(4)].IsAnswer = true;
            return resultList.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public int GetTheWrongColour(string name, List<ColourTile> existingColours)
        {
            var eligibleColours = ColourTiles
                .Where(c => c.Name != name)
                .SelectMany(c => c.ColorIds)
                .ToList();
            var resultColour = eligibleColours
                .ElementAt(new Random()
                .Next(eligibleColours.Count));
            return resultColour;
        }

        private static List<ColourTile> ColourTiles =
            new List<ColourTile>
            {
                new ColourTile
                {
                    Name = "Pink",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.pink,
                            Resource.Color.lightpink,
                            Resource.Color.hotpink,
                            Resource.Color.deeppink,
                            Resource.Color.fuchsia,
                        },
                },
                new ColourTile
                {
                    Name = "Red",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.crimson,
                            Resource.Color.indianred,
                            Resource.Color.red,
                            Resource.Color.firebrick,
                        },
                },
                new ColourTile
                {
                    Name = "Orange",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.darkorange,
                            Resource.Color.orange,
                            Resource.Color.tomato,
                            Resource.Color.coral,
                        }
                },
                new ColourTile
                {
                    Name = "Green",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.lime,
                            Resource.Color.limegreen,
                            Resource.Color.palegreen,
                            Resource.Color.springgreen,
                            Resource.Color.yellowgreen,
                            Resource.Color.olivedrab,
                        }
                },
                new ColourTile
                {
                    Name = "Blue",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.skyblue,
                            Resource.Color.deepskyblue,
                            Resource.Color.dodgerblue,
                            Resource.Color.steelblue,
                            Resource.Color.powderblue,
                            Resource.Color.cornflowerblue,
                        }
                },
                new ColourTile
                {
                    Name = "Brown",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.tan,
                            Resource.Color.chocolate,
                            Resource.Color.peru,
                            Resource.Color.sienna,
                            Resource.Color.saddlebrown,
                            Resource.Color.brown,
                        }
                },
                new ColourTile
                {
                    Name = "Grey",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.slategray,
                            Resource.Color.silver,
                            Resource.Color.bainsboro,
                            Resource.Color.lightgray,
                        }
                },
                new ColourTile
                {
                    Name = "Purple",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.purple,
                            Resource.Color.blueviolet,
                            Resource.Color.darkviolet,
                            Resource.Color.darkorchid,
                        }
                }

            };

    }
}