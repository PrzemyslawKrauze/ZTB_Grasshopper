using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Tekla.Structures.Model;

namespace ZeroTouchGrasshopper
{
    public class AddChamferToPlate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AddChamferToPlate class.
        /// </summary>
        public AddChamferToPlate()
          : base("AddChamferToPlate", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Plate", "P", "Plate fot which chamfers will be added", GH_ParamAccess.item);
            pManager.AddIntegerParameter("ContourPointsIdex", "CPI", "Indexes of countour points to be modified", GH_ParamAccess.list);
            pManager.AddGenericParameter("Chamfers", "C", "List of chamfers to be assigned to contour points", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Plate", "P", "Plate width assigned chamfers", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            ContourPlate plate = new ContourPlate();
            List<int> contourPointIndexes = new List<int>();
            List<Chamfer> chamfers = new List<Chamfer>();
            DA.GetData(0, ref plate);
            DA.GetDataList(1, contourPointIndexes);
            DA.GetDataList(2, chamfers);

            List<ContourPoint> contourPoints = plate.Contour.ContourPoints.Cast<ContourPoint>().ToList();

            for (var i = 0; i < contourPoints.Count; i++)
            {
                if (contourPointIndexes.Contains(i))
                {
                    contourPoints[i].Chamfer = (Chamfer)chamfers[i];
                }
                else
                {
                    contourPoints[i].Chamfer.Type = Chamfer.ChamferTypeEnum.CHAMFER_NONE;
                }
            }

            plate.Contour.ContourPoints = new System.Collections.ArrayList(contourPoints);
            plate.Modify();

            DA.SetData(0,plate);
            new Tekla.Structures.Model.Model().CommitChanges();
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
            get { return new Guid("EAD2CEBD-46C1-4B56-A421-D91875EB7BF6"); }
        }
    }
}