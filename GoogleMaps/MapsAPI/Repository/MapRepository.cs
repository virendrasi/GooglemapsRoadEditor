using MapsAPI.Entities;
using System;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections.Generic;
namespace MapsAPI.Repository
{
    public class MapRepository
    {
        private OracleConnection connection;
        public MapRepository()
        {
            connection = new OracleConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["OGCPDB"].ConnectionString;
        }

        private string DBPARAMETERIDENTIFIER = ":";

        private enum RoadMapColumn
        {
            ROAD_ID,
            ROAD_NAME,
            START_LAT_LONG,
            END_LAT_LONG,
            DISTANCE,
            ROAD_DISABLE,
        }

        private OracleCommand getCmdInsertRoadMap(RoadMap roadMap)
        {
            OracleCommand cmd = new OracleCommand();
            OracleParameter param = null;
            string query = @"INSERT INTO HT_ROAD_MAP(ROAD_NAME, START_LAT_LONG, END_LAT_LONG,DISTANCE)
                             VALUES ("
                                    + DBPARAMETERIDENTIFIER + RoadMapColumn.ROAD_NAME.ToString()
                                    + ", " + DBPARAMETERIDENTIFIER + RoadMapColumn.START_LAT_LONG.ToString()
                                    + ", " + DBPARAMETERIDENTIFIER + RoadMapColumn.END_LAT_LONG.ToString()
                                    + ", " + DBPARAMETERIDENTIFIER + RoadMapColumn.DISTANCE.ToString()
                                    + " ) ";
            param = new OracleParameter() { Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String, Value = roadMap.RoadName, ParameterName = RoadMapColumn.ROAD_NAME.ToString() };
            cmd.Parameters.Add(param);
            param = new OracleParameter() { Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String, Value = roadMap.StartPoint, ParameterName = RoadMapColumn.START_LAT_LONG.ToString() };
            cmd.Parameters.Add(param);
            param = new OracleParameter() { Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String, Value = roadMap.EndPoint, ParameterName = RoadMapColumn.END_LAT_LONG.ToString() };
            cmd.Parameters.Add(param);
            param = new OracleParameter() { Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.Int32, Value = roadMap.Distance, ParameterName = RoadMapColumn.DISTANCE.ToString() };
            cmd.Parameters.Add(param);
            cmd.Connection = connection;
            cmd.CommandText = query;
            return cmd;
        }

        public void InsertRoadMap(RoadMap roadMap)
        {
            OracleCommand command = null;
            try
            {

                using (command = getCmdInsertRoadMap(roadMap))
                {
                    if (command.Connection.State == System.Data.ConnectionState.Closed)
                        command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (command != null && command.Connection.State == System.Data.ConnectionState.Open)
                    command.Connection.Close();

            }
        }



        private OracleCommand getCmdSelectRoadMaps()
        {
            OracleCommand cmd = new OracleCommand();
            string query = @"SELECT ROAD_ID, ROAD_NAME, START_LAT_LONG, END_LAT_LONG, ROAD_DISABLE,DISTANCE FROM HT_ROAD_MAP";
            cmd.Connection = connection;
            cmd.CommandText = query;
            return cmd;
        }

        public List<RoadMap> GetRoadMaps()
        {
            List<RoadMap> roadMapList = new List<RoadMap>();
            RoadMap data = null;
            OracleCommand command = null;
            try
            {

                using (command = getCmdSelectRoadMaps())
                {
                    if (command.Connection.State == System.Data.ConnectionState.Closed)
                        command.Connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            data = new RoadMap();
                            data.RoadId = Convert.ToInt32(reader[RoadMapColumn.ROAD_ID.ToString()]);
                            data.RoadName = Convert.ToString(reader[RoadMapColumn.ROAD_NAME.ToString()]);
                            data.StartPoint = Convert.ToString(reader[RoadMapColumn.START_LAT_LONG.ToString()]);
                            data.EndPoint = Convert.ToString(reader[RoadMapColumn.END_LAT_LONG.ToString()]);
                            data.Distance = Convert.ToInt32(reader[RoadMapColumn.DISTANCE.ToString()]);
                            data.IsDisable = Convert.ToInt32(reader[RoadMapColumn.ROAD_DISABLE.ToString()]) == 1 ? true : false;
                            roadMapList.Add(data);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (command != null && command.Connection.State == System.Data.ConnectionState.Open)
                    command.Connection.Close();
            }
            return roadMapList;
        }
    }
}