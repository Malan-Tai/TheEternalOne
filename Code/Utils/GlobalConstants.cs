using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Utils
{
    public static class GlobalConstants
    {
        public static Vector2[] Directions = new Vector2[]
        {
            new Vector2(1, 0), new Vector2(0, 1), new Vector2 (-1, 0), new Vector2(0, -1),
            new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1)
        };

        public static Vector2[] CardinalDirections = new Vector2[]
        {
            new Vector2(1, 0), new Vector2(0, 1), new Vector2 (-1, 0), new Vector2(0, -1)
        };
    }
}
