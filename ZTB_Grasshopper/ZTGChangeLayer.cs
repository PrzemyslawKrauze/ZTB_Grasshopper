using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using ZeroTouchTeklaLibrary;

namespace ZeroTouchGrasshopper
{
    public class ZTGChangeLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ZTGChangeLayer():base("ChangeLayer", "CL", "Change rebar layer", "ZTG", "ChangeLayer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LayerDictionary", "L", "Dictionary containing information about layer numbers of rebar created", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Dictionary<string, int[]> layerDictionary = new Dictionary<string, int[]>();

            //Create one layer dictionary from input list
            List<Dictionary<string, int[]>> layerList = new List<Dictionary<string, int[]>>();
            bool paramListLoad = DA.GetDataList(0, layerList);
            foreach (Dictionary<string, int[]> d in layerList)
            {
                foreach(KeyValuePair<string,int[]> keyValuePair in d)
                {
                    layerDictionary.Add(keyValuePair.Key,keyValuePair.Value);
                }
            }

            RebarCreator.ChangeLayer(layerDictionary);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("B275620C-A527-4F07-BC7E-6FF2D0D8599C"); }
        }
    }
}