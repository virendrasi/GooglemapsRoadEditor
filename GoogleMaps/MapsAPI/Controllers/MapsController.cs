using System;
using System.Web.Http;
using MapsAPI.Repository;
using MapsAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MapsAPI.Controllers
{
    [RoutePrefix("api/maps")]
    public class MapsController : ApiController
    {

        private MapRepository mapRepo;
        [Route("saveroadmap")]
        [HttpPost]
        public bool SaveRoadMap(RoadMap roadMap)
        {
            bool isSuccess = false;
            try
            {
                mapRepo = new MapRepository();
                mapRepo.InsertRoadMap(roadMap);
                isSuccess = true;

            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        [Route("getroadmaps")]
        [HttpGet]
        public List<RoadMap> GetRoadMaps()
        {
            List<RoadMap> roadMapList = new List<RoadMap>();
            try
            {
                mapRepo = new MapRepository();
                roadMapList = mapRepo.GetRoadMaps();
            }
            catch (Exception ex)
            {

            }
            return roadMapList;
        }


        [Route("getshortestpath")]
        [HttpGet]
        public List<string> GetShortestPath(string startPoint, string endPoint)
        {
            List<string> shortestPathNodes = new List<string>();
            List<RoadMap> roadMapList = new List<RoadMap>();
            try
            {
                roadMapList = this.GetRoadMaps();
                roadMapList = roadMapList.Where(r => r.IsDisable.Equals(false)).ToList();
                List<string> exceptionlst = new List<string>();
                Graph g = new Graph();
                foreach (var item in roadMapList)
                {
                    if (!exceptionlst.Contains(item.StartPoint))
                    {
                        var t = roadMapList.Where(x => x.StartPoint.Equals(item.StartPoint));
                        Dictionary<string, int> vert = new Dictionary<string, int>();
                        foreach (var x in t)
                        {
                            vert.Add(x.EndPoint, x.Distance);
                        }
                        exceptionlst.Add(item.StartPoint);
                        g.add_vertex(item.StartPoint, vert);
                    }
                }
                shortestPathNodes = g.shortest_path(startPoint, endPoint);

            }
            catch (Exception ex)
            {

            }
            return shortestPathNodes;
        }

    }
}


