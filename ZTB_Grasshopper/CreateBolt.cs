using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Geometry3d;

namespace ZeroTouchGrasshopper
{
    public class CreateBolt : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SetDetailUpVector class.
        /// </summary>
        public CreateBolt()
          : base("CreateBolt", "CB","Description","ZTG", "Tekla")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Object", GH_ParamAccess.item);
            pManager.AddPointParameter("FirstPoint", "FP", "Point", GH_ParamAccess.item);
            pManager.AddPointParameter("SecondPoint", "SP", "SecondPoint", GH_ParamAccess.item);
            pManager.AddTextParameter("Name","N","BoltName",GH_ParamAccess.item);            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bolt", "B", "Bolt", GH_ParamAccess.item);
        }

        //Fields
        private List<string> _storedGuids = new List<string>();
        string _name = string.Empty;
        string _namePropertyName = "BOLT_USERFIELD_1";
        private List<string> ReadGuids()
        {
            return new List<string>(_storedGuids);
        }
        private void AddGUID(string guid)
        {
            _storedGuids.Add(guid);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Model model = new Model();

            double scale = 1;
            Rhino.RhinoDoc rhinoDoc = Rhino.RhinoDoc.ActiveDoc;
            Rhino.UnitSystem unitSystem = rhinoDoc.ModelUnitSystem;
            if(unitSystem==Rhino.UnitSystem.Meters)
            {
                scale = 1000;
            }

            object mo = null;
            DA.GetData(0, ref mo);
            GH_Goo<Tekla.Structures.Model.ModelObject> modeObjectGoo = mo as GH_Goo<Tekla.Structures.Model.ModelObject>;
            var moGoo = modeObjectGoo.Value as Tekla.Structures.Model.ModelObject;
            Part part = moGoo as Tekla.Structures.Model.Part;

            Point3d firstPoint = new Point3d();
            DA.GetData(1, ref firstPoint);

            Point3d secondPoint = new Point3d();
            DA.GetData(2, ref secondPoint);

            string currentName = string.Empty;
            DA.GetData(3,ref currentName);

            TransformationPlane globalplane = new TransformationPlane();
            //TransformationPlane localPlane = new TransformationPlane(part.GetCoordinateSystem());
            model.GetWorkPlaneHandler().SetCurrentTransformationPlane(globalplane);

            /*
            if (currentName != _name)
            {
                _name = currentName;
                List<string> oldBoltsArrayGUIDs = ReadGuids();
                for (int i = 0; i < oldBoltsArrayGUIDs.Count; i++)
                {
                    if (!string.IsNullOrEmpty(oldBoltsArrayGUIDs[i]))
                    {
                        var baToDelete = new BoltArray();
                        baToDelete.Identifier = new Model().GetIdentifierByGUID(oldBoltsArrayGUIDs[i]);
                        string baName = String.Empty;
                        baToDelete.GetUserProperty(_namePropertyName, ref baName);
                        if (baName != _name)
                        {
                            baToDelete.Delete();
                            oldBoltsArrayGUIDs.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            */

            BoltArray boltArray = new BoltArray();
            boltArray.PartToBeBolted = part;
            boltArray.PartToBoltTo = part;

            boltArray.FirstPosition = new Tekla.Structures.Geometry3d.Point(firstPoint.X*scale, firstPoint.Y * scale, firstPoint.Z * scale);
            boltArray.SecondPosition = new Tekla.Structures.Geometry3d.Point(secondPoint.X * scale, secondPoint.Y * scale, secondPoint.Z * scale);
                      
            boltArray.BoltStandard = "NELSON";
            boltArray.BoltSize = 22;
            boltArray.Tolerance = 3;
            boltArray.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray.Length = 250;
            boltArray.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;

            boltArray.Position.Depth = Position.DepthEnum.MIDDLE;
            boltArray.Position.Plane = Position.PlaneEnum.MIDDLE;
            boltArray.Position.Rotation = Position.RotationEnum.FRONT;
                      
            boltArray.AddBoltDistY(0);
            boltArray.EndPointOffset.Dy = 0;

            boltArray.AddBoltDistX(0);
            boltArray.EndPointOffset.Dx = 0;
            

            boltArray.Insert();
            boltArray.SetUserProperty(_namePropertyName, _name);
            boltArray.Modify();
            AddGUID(boltArray.Identifier.GUID.ToString());

            model.CommitChanges();

            DA.SetData(0, boltArray);
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
            get { return new Guid("8310A2DD-2A64-4B37-AB14-944CD2E0E575"); }
        }


    }
}