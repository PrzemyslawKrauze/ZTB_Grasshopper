using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Tekla.Structures;

namespace ZTB_Grasshopper
{
    public class GetRebarFromGroup : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetRebarFromGroup class.
        /// </summary>
        public GetRebarFromGroup()
           : base("GetRebarFromGroup", "GRfG",
              "Description",
              "ZTB", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Rebar group", "RG", "Rebar group", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Single rebar", "SR", "List of single rebars", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object modelObject = null;
            DA.GetData(0, ref modelObject);
            GH_Goo<Tekla.Structures.Model.ModelObject> gH_Goo = modelObject as GH_Goo<Tekla.Structures.Model.ModelObject>;
            Tekla.Structures.Model.RebarGroup rebarGroup = gH_Goo.Value as Tekla.Structures.Model.RebarGroup;
            int numberOfBars = rebarGroup.GetNumberOfRebars();
            List<Tekla.Structures.Model.RebarGeometry> singleRebars = new List<Tekla.Structures.Model.RebarGeometry>();
            for(int i=0;i<numberOfBars; i++)
            {
                singleRebars.Add(rebarGroup.GetSingleRebar(i, true));
            }
            DA.SetDataList(0, singleRebars);
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
            get { return new Guid("623406FF-C2B9-4B37-83D0-990E11383517"); }
        }
    }
}