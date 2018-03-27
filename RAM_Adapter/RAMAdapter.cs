﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter;
using BH.Engine.RAM;
using RAMDATAACCESSLib;
using System.IO;

namespace BH.Adapter.RAM
{
    public partial class RAMAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        //Add any applicable constructors here, such as linking to a specific file or anything else as well as linking to that file through the (if existing) com link via the API
        public RAMAdapter(string filePath = "")
        {
            AdapterId = BH.Engine.RAM.Convert.AdapterId;   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine

            Config.SeparateProperties = true;   //Set to true to push dependant properties of objects before the main objects are being pushed. Example: push nodes before pushing bars
            Config.MergeWithComparer = true;    //Set to true to use EqualityComparers to merge objects. Example: merge nodes in the same location
            Config.ProcessInMemory = false;     //Set to false to to update objects in the toolkit during the push
            Config.CloneBeforePush = true;      //Set to true to clone the objects before they are being pushed through the software. Required if any modifications at all, as adding a software ID is done to the objects
            Config.UseAdapterId = true;         //Tag objects with a software specific id in the CustomData. Requires the NextIndex method to be overridden and implemented


            IDBIO1 RAMDataAccIDBIO;
            IModel IModel;

            m_RAMApplication = new RamDataAccess1();

            // Initialize to interface (CREATE NEW MODEL in RAM data folder by default)
            if (filePath == "" && !File.Exists(filePath))
            {
                try
                {

                    string filePathTest = "C:\\ProgramData\\Bentley\\Engineering\\RAM Structural System\\Data\\Grasshopper_Model.rss";
                    RAMDataAccIDBIO = m_RAMApplication.GetDispInterfacePointerByEnum(EINTERFACES.IDBIO1_INT);

                    // Object Model Interface
                    IModel = m_RAMApplication.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);
                    RAMDataAccIDBIO.CreateNewDatabase2(filePathTest, EUnits.eUnitsEnglish, "Grasshopper");

                }
                catch
                {
                    Console.WriteLine("Cannot create RAM database, check that a compatible version of RAM is installed");
                }

            }

            // Initialize to interface (CREATE NEW MODEL at provided filepath)
            if (filePath != "" && !File.Exists(filePath))
            {
                try
                {
                    RAMDataAccIDBIO = m_RAMApplication.GetDispInterfacePointerByEnum(EINTERFACES.IDBIO1_INT);

                    // Object Model Interface
                    IModel = m_RAMApplication.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);
                    RAMDataAccIDBIO.CreateNewDatabase2(filePath, EUnits.eUnitsEnglish, "Grasshopper");

                }
                catch
                {
                    Console.WriteLine("Cannot create RAM database, check that the provided filepath is valid");
                }
            }

            // Initialize to inferface (OF EXISTING MODEL)
            if (File.Exists(filePath))
            {
                try
                {
                    string filePathUserfile = filePath.Replace(".rss", ".usr");
                    RAMDataAccIDBIO = m_RAMApplication.GetDispInterfacePointerByEnum(EINTERFACES.IDBIO1_INT);
                    RAMDataAccIDBIO.LoadDataBase(filePath);

                    // Object Model Interface
                    IModel = m_RAMApplication.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);
                }
                catch
                {
                    Console.WriteLine("Cannot load RAM database, check that existing model is closed");
                }

            }
        }
            

      

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        //private SoftwareComLink m_softwareNameCom;

        private RamDataAccess1 m_RAMApplication;
        

        /***************************************************/


    }
}
