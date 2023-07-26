using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace ZeroTouchGrasshopper
{
    public class CreateChamfer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateChamfer() : base("CreateChamfer", "CreateChamfer", "Description", "ZTG", "Model")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("ChamferType", "CT", "Type of chamfer to be created", GH_ParamAccess.item);
            pManager.AddNumberParameter("X", "X", "Chamfer X vaule", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "Chamfer Y vaule", GH_ParamAccess.item);
            pManager.AddNumberParameter("DZ1", "DZ1", "Chamfer dz1 vaule", GH_ParamAccess.item);
            pManager.AddNumberParameter("DZ2", "DZ2", "Chamfer dz2 vaule", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Chamfer", "C", "Chamfer", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int chamferType = 0;
            int x = 0;
            int y = 0;
            int dz1 = 0;
            int dz2 = 0;
            DA.GetData(0, ref chamferType);
            DA.GetData(1, ref x);
            DA.GetData(2, ref y);
            DA.GetData(3, ref dz1);
            DA.GetData(4, ref dz2);
            Chamfer chamfer = new Chamfer(x, y, (Chamfer.ChamferTypeEnum)chamferType);
            chamfer.DZ1 = dz1;
            chamfer.DZ2 = dz2;
           DA.SetData(0, chamfer);
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
            get { return new Guid("2659E54B-DE95-4773-A181-54FA7ADCC175"); }
        }
    }
}