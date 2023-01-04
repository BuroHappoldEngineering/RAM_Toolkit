/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Adapters.RAM;
using BH.oM.Adapter;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Results;
using BH.oM.Structure.Loads;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Requests;
using BH.oM.Analytical.Results;
using RAMDATAACCESSLib;
using System.IO;
using BH.oM.Spatial.SettingOut;
using BH.Engine.Units;
using BH.Engine.Adapter;
using BH.Engine.Base;

namespace BH.Adapter.RAM
{
    public partial class RAMAdapter
    {

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private List<Grid> ReadGrid(List<string> ids = null)
        {
            //Implement code for reading Grids
            List<Grid> bhomGrids = new List<Grid>();

            // Get the gridsystems that are in the model
            IGridSystems IGridSystems = m_Model.GetGridSystems();
            int numGridSystems = IGridSystems.GetCount();

            // Get all elements on each GridSystem
            for (int i = 0; i < numGridSystems; i++)
            {
                //Look into a specific gridsystem
                IGridSystem myGridSystem = IGridSystems.GetAt(i);

                //get the amoount of gridlines that are in the system
                IModelGrids IModelGrids = myGridSystem.GetGrids();

                int numGridLines = IModelGrids.GetCount();

                // Loop through all gridlines in the GridSystem and add a bhomGrid
                for (int j = 0; j < numGridLines; j++)
                {
                    IModelGrid IModelGrid = IModelGrids.GetAt(j);
                    Grid bhomGrid = IModelGrid.ToBHoMObject(myGridSystem, j);
                    bhomGrids.Add(bhomGrid);
                }
            }

            return bhomGrids;
        }

        /***************************************************/

    }

}




