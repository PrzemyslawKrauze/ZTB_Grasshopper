using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;

using ZeroTouchTeklaLibrary;
using Tekla.Structures.Model;

namespace ZeroTouchGrasshopper
{
    public class ZTGCreateRebar : GH_Component
    {

        //Fields
        private List<string> _storedGuids = new List<string>();
        private List<string> ReadGuids()
        {
            return new List<string>(_storedGuids);
        }
        private void WriteGuids(List<string> guids)
        {
            _storedGuids = guids;
        }

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ZTGCreateRebar() : base("CreateRebar", "CR", "Create rebar", "ZTG", "Rebar")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BEAM", "B", "Beam to put rebar in", GH_ParamAccess.item);
            pManager.AddTextParameter("RebarName", "RN", "Name of rebar to create", GH_ParamAccess.item);
            pManager.AddTextParameter("Dictionary", "D", "Text coresponding to rebar parameter and its value", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Succesful", "S", "True if component created rebar", GH_ParamAccess.item);
            pManager.AddGenericParameter("LayerDictionary", "L", "Dictionary with layer information", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Model model = new Model();
            TransformationPlane userWorkPlane = model.GetWorkPlaneHandler().GetCurrentTransformationPlane();
            //Convert component input to Tekla.Structures.Model.Part
            object mo = null;
            bool beamLoad = DA.GetData(0, ref mo);
            GH_Goo<Tekla.Structures.Model.ModelObject> modeObjectGoo = mo as GH_Goo<Tekla.Structures.Model.ModelObject>;
            var moGoo = modeObjectGoo.Value as Tekla.Structures.Model.ModelObject;
            Tekla.Structures.Model.Part[] parts = new Tekla.Structures.Model.Part[1];
            parts[0] = moGoo as Tekla.Structures.Model.Part;

            //Get name of rebar to create
            string rebarName = string.Empty;
            bool rebarNameLoad = DA.GetData(1, ref rebarName);

            //Get rebar parameters
            Dictionary<string, string> paramDictionary = new Dictionary<string, string>();
            List<string> paramList = new List<string>();
            bool paramListLoad = DA.GetDataList(2, paramList);
            foreach (string param in paramList)
            {
                string[] splitedParam = param.Split(',', ';', '/');
                paramDictionary.Add(splitedParam[0], splitedParam[1]);
            }

            //Delete old rebar if exists
            List<string> oldRebarsGUID = ReadGuids();
            for (int i = 0; i < oldRebarsGUID.Count; i++)
            {
                if (!string.IsNullOrEmpty(oldRebarsGUID[i]))
                {
                    var rsToDelete = new RebarSet();
                    rsToDelete.Identifier = new Model().GetIdentifierByGUID(oldRebarsGUID[i]);
                    rsToDelete.Delete();
                }
            }
            Storage.ParameterDictionary = paramDictionary;

            Dictionary<string, int[]> layerDictionary = new Dictionary<string, int[]>();
            try
            {
                Element element = RebarCreator.CreateSingleSetForPartGH(rebarName, parts);

                layerDictionary = element.LayerDictionary;
                List<string> newGuids = new List<string>();

                for (int i = 0; i < element.RebarSets.Count; i++)
                {
                    RebarSet rebarSet = element.RebarSets[i];
                    newGuids.Add(rebarSet.Identifier.GUID.ToString());
                }
                WriteGuids(newGuids);
            }
            finally
            {
                //Restore user work plane
                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(userWorkPlane);
                model.CommitChanges();
            }

            bool succes = true;
            DA.SetData(0, succes);
            DA.SetData(1, layerDictionary);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("F5BFA5AE-A563-45CE-8012-AE1D540DCEC2");
    }
}