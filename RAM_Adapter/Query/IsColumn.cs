/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Adapters.RAM;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using BH.Engine.Base;
using BH.Engine.Geometry;
using RAMDATAACCESSLib;
using BH.Engine.Units;


namespace BH.Adapter.RAM
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool IsColumn(this Bar bar, IStories stories)
        {
            if (bar.IsVertical()) return true; // it's vertical, it's a column. Don't waste my time.

            IStory startStory = bar.StartNode.GetStory(stories);
            IStory endStory = bar.EndNode.GetStory(stories);

            if (startStory is null ^ endStory is null) return true; // one of the ends is at zero elevation, so it's a column.
            if (startStory is null && endStory is null) return false; // both ends are at zero elevation, so this is a beam on the ground. We'll pick it up later.
            return  startStory.lUID != endStory.lUID; // This starts on a different level than it ends, so it's probably a column. (it could be a brace but let's not think about that too hard).
        }
    }
}

