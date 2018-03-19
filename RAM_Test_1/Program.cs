﻿using System;
using System.Collections.Generic;
using RAMDATAACCESSLib;
using RAMDataBaseAccess;

namespace RAM_Test
{

    public class Program
    {

        static void Main(string[] args)
        {

            Test();

        }

        private static void Test()

        {

            string filePathNew;
            string filePathExisting;
            string filePathUserfile;
            string filePathAdd;
            string strWorkingDir;
            string filePathEdited;
            Boolean run;
            int type;
            List<int> Stories;
            Boolean loaded;
            IStories IStories;

            // Define Variables
            IRamDataAccess1 RAMDataAcc1;
            IDBIO1 RAMDataAccIDBIO;
            IModel IModel;
            ISteelCriteria ISteelCriteria;
            IModelData1 IModelData1;
            IStory IStory;
            IBeams IBeams;
            IColumns IColumns;
            IFloorTypes IFloorTypes;
            IFloorType IFloorType;
            ILayoutColumns ILayoutColumns;
            Stories = new List<int>();
            List<string> ColumnSections = new List<string>();

            // Set filepaths (New can be any filepath, existing has to be an actual model; will give errors if interface has not been released, still working on it)
            filePathNew = "C:\\ProgramData\\Bentley\\Engineering\\RAM Structural System\\Data\\Tutorial\\new_2.rss";
            filePathExisting = "C:\\ProgramData\\Bentley\\Engineering\\RAM Structural System\\Data\\Tutorial\\Tutorial_v1507_US.rss";
            filePathAdd = "C:\\ProgramData\\Bentley\\Engineering\\RAM Structural System\\Data\\Tutorial\\test.rss";
            strWorkingDir = "C:\\ProgramData\\Bentley\\Engineering\\RAM Structural System\\Data\\Tutorial";
            filePathEdited = filePathExisting.Replace(".rss", "API.rss");

            // Usr filepath so we can delete .usr at end of function
            filePathUserfile = filePathExisting.Replace(".rss", ".usr");
            
            // Initialize Data Access
            RAMDataAcc1 = new RamDataAccess1();

            // Set Type (for testing)
            //string Type = "Create";
            string Type = "Add";
            //string Type = "Existing";

            RAMDataAccIDBIO = null;

            
            RAMDataAccIDBIO = RAMDataAcc1.GetDispInterfacePointerByEnum(EINTERFACES.IDBIO1_INT);

            
            // Initialize to interface (CREATE NEW MODEL)
            if (Type.Equals("Create")) {

                RAMDataAccIDBIO.CreateNewDatabase2(filePathNew, EUnits.eUnitsEnglish, "Grasshopper");

                // Object Model Interface
                IModel = RAMDataAcc1.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);

                // Testing element creation
            
                IFloorTypes = IModel.GetFloorTypes();
                IFloorTypes.Add("Type_1");
                IFloorType = IFloorTypes.GetAt(0);
                ILayoutColumns = IFloorType.GetLayoutColumns();

                // Once we have the ILayoutColumn we can do iterative creation with list of input points, properties, etc (not working yet, need to create grid system first?)
                ILayoutColumn ILayoutColumn = ILayoutColumns.Add2(EMATERIALTYPES.ESteelMat, 0, 0, 2, 2, 8, 0);
                ILayoutColumn.strSectionLabel = "W14X48";
                filePathUserfile = filePathNew.Replace(".rss", ".usr");

            }







            // Existing model, initialize model and then add columns
            if (Type.Equals("Add"))
            {

                filePathUserfile = filePathAdd.Replace(".rss", ".usr");
                RAMDataAccIDBIO.LoadDataBase(filePathAdd);

                // Object Model Interface
                IModel = RAMDataAcc1.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);

                // Testing element creation

                IFloorTypes = IModel.GetFloorTypes();
                IFloorType = IFloorTypes.GetAt(0);
                ILayoutColumns = IFloorType.GetLayoutColumns();

                // Once we have the ILayoutColumn we can do iterative creation with list of input points, properties, etc (not working yet, need to create grid system first?)
                ILayoutColumn ILayoutColumn = ILayoutColumns.Add2(EMATERIALTYPES.ESteelMat, 4, 4, 2, 2, 4, 0);
                ILayoutColumn.strSectionLabel = "W14X48";
                ILayoutColumn = ILayoutColumns.Add2(EMATERIALTYPES.EConcreteMat, 4, 4, 2, 2, 4, 0);
                ILayoutColumn.strSectionLabel = "C12X26";

                filePathUserfile = filePathAdd.Replace(".rss", ".usr");
            }






                // Initialize to interface (FOR EXISTING MODEL)
                if (Type.Equals("Existing")) {

                filePathUserfile = filePathExisting.Replace(".rss", ".usr");
                RAMDataAccIDBIO.LoadDataBase(filePathExisting);
       
                // Object Model Interface
                IModel = RAMDataAcc1.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);

                // Get stories
                IStories = IModel.GetStories();
                int numStories = IStories.GetCount();
                Stories.Add(numStories);



                // Get columns on first story
                IColumns = IStories.GetAt(1).GetColumns();
                int numColumns = IColumns.GetCount();

                // Find name of every column (to begin)
                for (int i = 0; i < IColumns.GetCount(); i++)
                {

                    // Get the name of every column
                    IColumn IColumn = IColumns.GetAt(i);
                    string section = IColumn.strSectionLabel;
                    ColumnSections.Add(section);

                }

                //Write output of original database
                Console.WriteLine(filePathExisting);
                Stories.ForEach(i => Console.Write("{0}\t", i));
                ColumnSections.ForEach(i => Console.Write("{0}\t", i));

                // Set every column to a standard section size
                for (int i = 0; i < IColumns.GetCount(); i++)
                {

                    // Set every column to a standard size (working, need to save database after update)
                    IColumn IColumn = IColumns.GetAt(i);
                    IColumn.strSectionLabel = "W14X48";

                }

                // Find name of every column (to check updated section names)
                ColumnSections.Clear();
                for (int i = 0; i < IColumns.GetCount(); i++)
                {

                    // Get the name of every column
                    IColumn IColumn = IColumns.GetAt(i);
                    string section = IColumn.strSectionLabel;
                    ColumnSections.Add(section);

                }

            }

            //Write output of new database
            Console.WriteLine(filePathExisting);
            Stories.ForEach(i => Console.Write("{0}\t", i));
            ColumnSections.ForEach(i => Console.Write("{0}\t", i));

            //Save file
            RAMDataAccIDBIO.SaveDatabase();

            // Release main interface and delete user file
            RAMDataAccIDBIO = null;
            System.IO.File.Delete(filePathUserfile);

            int test = 1;
        }






    }
}

