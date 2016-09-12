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
                .Where(c => 
                    c.Name != name && 
                    existingColours.All(exCol => exCol.Name != c.Name) 
                 )
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
                            Resource.Color.PIN_pink,
                            Resource.Color.PIN_lightpink,
                            Resource.Color.PIN_hotpink,
                            Resource.Color.PIN_deeppink,
                            Resource.Color.PIN_fuchsia,
                        },
                },
                new ColourTile
                {
                    Name = "Red",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.RED_crimson,
                            Resource.Color.RED_indianred,
                            Resource.Color.RED_red,
                            Resource.Color.RED_firebrick,
                        },
                },
                new ColourTile
                {
                    Name = "Orange",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.ORA_darkorange,
                            Resource.Color.ORA_orange,
                            Resource.Color.ORA_tomato,
                            Resource.Color.ORA_coral,
                        }
                },
                new ColourTile
                {
                    Name = "Green",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.GRE_lime,
                            Resource.Color.GRE_limegreen,
                            Resource.Color.GRE_palegreen,
                            Resource.Color.GRE_springgreen,
                            Resource.Color.GRE_yellowgreen,
                            Resource.Color.GRE_olivedrab,
                        }
                },
                new ColourTile
                {
                    Name = "Blue",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.BLU_skyblue,
                            Resource.Color.BLU_deepskyblue,
                            Resource.Color.BLU_dodgerblue,
                            Resource.Color.BLU_steelblue,
                            Resource.Color.BLU_powderblue,
                            Resource.Color.BLU_cornflowerblue,
                        }
                },
                new ColourTile
                {
                    Name = "Brown",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.BRO_tan,
                            Resource.Color.BRO_chocolate,
                            Resource.Color.BRO_peru,
                            Resource.Color.BRO_sienna,
                            Resource.Color.BRO_saddlebrown,
                            Resource.Color.BRO_brown,
                        }
                },
                new ColourTile
                {
                    Name = "Grey",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.GRY_slategray,
                            Resource.Color.GRY_silver,
                            Resource.Color.GRY_bainsboro,
                            Resource.Color.GRY_lightgray,
                        }
                },
                new ColourTile
                {
                    Name = "Purple",
                    ColorIds =
                        new[]
                        {
                            Resource.Color.PUR_purple,
                            Resource.Color.PUR_blueviolet,
                            Resource.Color.PUR_darkviolet,
                            Resource.Color.PUR_darkorchid,
                        }
                }

            };

    }
}