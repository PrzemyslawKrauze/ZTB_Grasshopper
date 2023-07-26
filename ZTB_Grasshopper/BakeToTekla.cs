using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace ZTB_Grasshopper
{
    public class BakeToTekla : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BakeToTekla class.
        /// </summary>
        public BakeToTekla()
          : base("BakeToTekla", "BtT",
              "Description",
              "ZTB", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ModelObject", "MO", "Object to bake", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "R", "True to bake", GH_ParamAccess.item);
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
            bool run = false;
            DA.GetData(1, ref run);

            if (run)
            {          
                object mo = null;
                DA.GetData(0, ref mo);
                GH_Goo<Tekla.Structures.Model.ModelObject> modelObjectGoo = mo as GH_Goo<Tekla.Structures.Model.ModelObject>;
                var modelObject = modelObjectGoo.Value as Tekla.Structures.Model.ModelObject;
                modelObject.Insert();
                new Tekla.Structures.Model.Model().CommitChanges();
            }
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
            get { return new Guid("3A3F9C4B-F022-44D2-A24D-8915D13F7EE9"); }
        }
    }
}