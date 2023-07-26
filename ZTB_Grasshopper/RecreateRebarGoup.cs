using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Render.UI;

using Tekla.Structures;
using Tekla.Structures.Model;

namespace ZTB_Grasshopper
{
    public class RecreateRebarGoup : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public RecreateRebarGoup()
           : base("RecreteRebarGroup", "RRG",
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
            pManager.AddGenericParameter("Rebar group", "RG", "Rebar group", GH_ParamAccess.item);
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

            Tekla.Structures.Geometry3d.Vector  vector = new Tekla.Structures.Geometry3d.Vector(0, 0, 0);
            ModelObject copiedRebarGroup = Tekla.Structures.Model.Operations.Operation.CopyObject(rebarGroup,vector);
            rebarGroup.Delete();
            DA.SetData(0,copiedRebarGroup);
            
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
            get { return new Guid("D35BA0AD-576C-44E7-BBAA-77D353DE7A4D"); }
        }
    }
}