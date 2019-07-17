using System;
using System.Collections.Generic;
using System.Linq;
using Elements;
using Elements.Geometry;
using Elements.GeoJSON;
using GeometryEx;
using RoomKit;

namespace MultiResidence
{
    public static class MultiResidence
    {
        /// <summary>
        /// The MultiResidence function.
        /// </summary>
        /// <param name="model">The model. 
        /// Add elements to the model to have them persisted.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A MultiResidenceOutputs instance containing computed results.</returns>
        public static MultiResidenceOutputs Execute(Model model, MultiResidenceInputs input)
        {

            var boundary = new Elements.Geometry.Polygon(ParseCoords(model, input));
            var centroid = boundary.Centroid();
            var site = new Mass(new Profile(boundary),
                                0.2,
                                new Material("site", new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.0f, 0.0f),
                                new Transform(0.0, 0.0, 0.2));
            model.AddElement(site);
            var tower = new Tower
            {
                Color = Palette.White,
                StoryHeight = 4.0,
                TargetArea = 0.0
            };
            if (input.Units < 8)
            {
                var polygon = Elements.Geometry.Polygon.Rectangle(23.0, 16.0);
                //if (boundary.Covers(polygon.MoveFromTo(Vector3.Origin, centroid)))
                //{
                //    model.AddElement(site);
                //    return new MultiResidenceOutputs(0.0, 0.0, 0.0, 0.0);
                //}
                tower.Floors = (int)Math.Ceiling(input.Units / 2);
                tower.Perimeter = polygon;
                tower.Stack();
                foreach (var story in tower.Stories)
                {
                    story.RoomsByDivision(2, 1, 3.5, 0.1, color: Palette.Aqua);
                    Room corridor = null;
                    if (story.Elevation <= 0.0)
                    {
                        corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(0.0, -8.0), new Vector3(0.0, 8.0)).Thicken(3.0),
                                            height: 3.5);
                    }
                    else
                    {
                        corridor = new Room(perimeter: new Elements.Geometry.Line(Vector3.Origin, new Vector3(0.0, 8.0)).Thicken(3.0),
                                            height: 3.5);
                    }
                    story.AddCorridor(corridor);
                    if (tower.Floors > 1)
                    {
                        var core = Elements.Geometry.Polygon.Rectangle(3.0, 6.0).MoveFromTo(Vector3.Origin, new Vector3(0.0, 8.0));
                        tower.AddCore(core, 0, 3.0, Palette.Gray);
                    }
                }
            }
            else if (input.Units < 30)
            {
                var polygon = Elements.Geometry.Polygon.Rectangle(23.0, 32.0);
                //if (boundary.Covers(polygon.MoveFromTo(Vector3.Origin, centroid)))
                //{
                //    model.AddElement(site);
                //    return new MultiResidenceOutputs(0.0, 0.0, 0.0, 0.0);
                //}
                tower.Floors = (int)input.Units / 4;
                tower.Perimeter = polygon;
                tower.Stack();
                foreach (var story in tower.Stories)
                {
                    story.RoomsByDivision(2, 1, 3.5, 0.1, color: Palette.Aqua);
                    Room corridor = null;
                    if (story.Elevation <= 0.0)
                    {
                        corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(0.0, -16.0), new Vector3(0.0, 16.0)).Thicken(3.0),
                                            height: 3.5);
                    }
                    else
                    {
                        corridor = new Room(perimeter: new Elements.Geometry.Line(Vector3.Origin, new Vector3(0.0, 8.0)).Thicken(3.0),
                                            height: 3.5);
                    }
                    story.AddCorridor(corridor);
                    if (tower.Floors > 1)
                    {
                        var core = Elements.Geometry.Polygon.Rectangle(3.0, 6.0).MoveFromTo(Vector3.Origin, new Vector3(0.0, 8.0));
                        tower.AddCore(core, 0, 3.0, Palette.Gray);
                    }
                }
            }
            else
            {
                var polygon = Elements.Geometry.Polygon.Rectangle(36.0, 32.0);
                //if (!boundary.Covers(polygon.MoveFromTo(Vector3.Origin, centroid)))
                //{
                //    model.AddElement(site);
                //    return new MultiResidenceOutputs(0.0, 0.0, 0.0, 0.0);
                //}
                tower.Floors = (int)input.Units / 6;
                tower.Perimeter = Elements.Geometry.Polygon.Rectangle(36.0, 32.0);
                tower.Stack();
                foreach (var story in tower.Stories)
                {
                    story.RoomsByDivision(3, 2, 3.5, 0.1, color: Palette.Aqua);
                    Room corridor = null;
                    if (story.Elevation <= 0.0)
                    {
                        corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(-6.5, -16.0), new Vector3(-6.5, 16.0)).Thicken(3.0),
                                            height: 3.5);
                        story.AddCorridor(corridor);
                        corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(6.5, -16.0), new Vector3(6.5, 16.0)).Thicken(3.0),
                                            height: 3.5);
                        story.AddCorridor(corridor);
                    }
                    else
                    {
                        corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(-6.5, 0.0), new Vector3(-6.5, 16.0)).Thicken(3.0),
                                            height: 3.5);
                        story.AddCorridor(corridor);
                        corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(6.5, 0.0), new Vector3(6.5, 16.0)).Thicken(3.0),
                                            height: 3.5);
                        story.AddCorridor(corridor);
                    }
                    corridor = new Room(perimeter: new Elements.Geometry.Line(new Vector3(-10.0, 0.0), new Vector3(10, 0.0)).Thicken(3.0),
                                        height: 3.5);
                    story.AddCorridor(corridor);
                    if (tower.Floors > 1)
                    {
                        var core = Elements.Geometry.Polygon.Rectangle(6.0, 6.0).MoveFromTo(Vector3.Origin, new Vector3(-8.0, 16.0));
                        tower.AddCore(core, 0, 3.0, Palette.Gray);
                        core = Elements.Geometry.Polygon.Rectangle(6.0, 6.0).MoveFromTo(Vector3.Origin, new Vector3(8.0, 16.0));
                        tower.AddCore(core, 0, 3.0, Palette.Gray);
                    }
                }
            }
            tower.MoveFromTo(Vector3.Origin, centroid);
            foreach (var core in tower.Cores)
            {
                model.AddElement(core.AsMass);
            }
            var unitQty = 0;
            var totalArea = 0.0;
            foreach (var story in tower.Stories)
            {
                unitQty += story.Rooms.Count;
                totalArea += story.Area;
                foreach (var mass in story.InteriorsAsMasses)
                {
                    model.AddElement(mass);
                }
                model.AddElement(story.Slab);
            }

            var output = new MultiResidenceOutputs(unitQty, totalArea, Math.Abs(tower.Area / boundary.Area()), tower.Height);
            return output;
        }

        private static List<List<string>> ParseCSV(string csv)
        {
            var rows = new List<List<string>>();
            var lines = csv.Split('\n');
            foreach (var line in lines)
            {
                var split = line.Split(',');
                var row = new List<string>(line.Split(','));
                rows.Add(row);
            }
            return rows;
        }

        private static Vector3[] ParseCoords(Model model, MultiResidenceInputs input)
        {
            var rows = ParseCSV(System.IO.File.ReadAllText(input.Site.LocalFilePath));
            var points = new List<Vector3>();
            double x = 0.0;
            var xCorrect = 0.0;
            var yCorrect = 0.0;
            for (int i = 0; i < rows[0].Count(); i++)
            {
                if (i == 0 &&
                    double.TryParse(rows[0][0].ToString(), out double lon) &&
                    double.TryParse(rows[0][1].ToString(), out double lat))
                {
                    xCorrect = MercatorProjection.lonToX(lon);
                    yCorrect = MercatorProjection.lonToX(lat);
                    model.Origin = new Position(lon, lat);
                }
                if (i % 2 == 0 && double.TryParse(rows[0][i].ToString(), out lon))
                {
                    x = MercatorProjection.lonToX(lon) - xCorrect;
                }
                else if (double.TryParse(rows[0][i].ToString(), out lat))
                {
                    points.Add(new Vector3(x, MercatorProjection.lonToX(lat) - yCorrect));
                }
            }
            //double.TryParse(rows[1][0].ToString(), out double parkX);
            //double.TryParse(rows[1][1].ToString(), out double parkY);
            //parkX = (MercatorProjection.lonToX(parkX) - xCorrect) * 0.001;
            //parkY = (MercatorProjection.latToY(parkY) - yCorrect) * 0.001;

            //double.TryParse(rows[2][0].ToString(), out double busX);
            //double.TryParse(rows[2][1].ToString(), out double busY);
            //busX = (MercatorProjection.lonToX(busX) - xCorrect) * 0.001;
            //busY = (MercatorProjection.latToY(busY) - yCorrect) * 0.001;

            //double.TryParse(rows[3][0].ToString(), out double schoolX);
            //double.TryParse(rows[3][1].ToString(), out double schoolY);
            //schoolX = (MercatorProjection.lonToX(schoolX) - xCorrect) * 0.001;
            //schoolY = (MercatorProjection.latToY(schoolY) - yCorrect) * 0.001;

            //model.AddElement(new Mass(new Profile(Elements.Geometry.Polygon.Circle(20.0, 50)),
            //                          0.1,
            //                          new Material("park", Palette.Green),
            //                          new Transform(parkX, parkY, 0.5)));

            //model.AddElement(new Mass(new Profile(Elements.Geometry.Polygon.Circle(1.0)),
            //                  5.0,
            //                  new Material("bus", Palette.Cobalt),
            //                  new Transform(busX, busY, 0.0)));

            //model.AddElement(new Mass(new Profile(Elements.Geometry.Polygon.Rectangle(30.0, 30.0)),
            //                  5.0,
            //                  new Material("school", Palette.Lavender),
            //                  new Transform(schoolX, schoolY, 0.0)));
            return points.Distinct().ToArray();
        }
    }
}