using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodesApi.Models;
using Microsoft.Data.Sqlite;

namespace NodesApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/nodepairs")]
    public class NodePairsController : ControllerBase
    {
        List<NodePairs> nodepairs = new List<NodePairs>();

        SqliteConnection sqlite_conn = new SqliteConnection("Data Source=NodesDB.sqlite;");

        [HttpGet]
        public IEnumerable<NodePairs> ListAllNodePairss()
        {
            try
            {
                sqlite_conn.Open();
                SqliteDataReader sqlite_datareader;
                SqliteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM nodePairs";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    NodePairs item = new NodePairs(){ParentId = Convert.ToInt32(sqlite_datareader["parentNodeId"]), ChildId = Convert.ToInt32(sqlite_datareader["childNodeId"])};
                    nodepairs.Add(item);
                }
                sqlite_conn.Close();
                return nodepairs;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                return null;
            }
        }

        [HttpGet("{id}")]
        public IEnumerable<NodePairs> GetNodePairById(int id)
        {
            List<NodePairs> childernNodes = new List<NodePairs>();
            try
            {
                sqlite_conn.Open();
                SqliteDataReader sqlite_datareader;
                SqliteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM nodepairs where parentNodeId = '" + id + "'";
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    NodePairs item = new NodePairs(){ParentId = Convert.ToInt32(sqlite_datareader["parentNodeId"]), ChildId = Convert.ToInt32(sqlite_datareader["childNodeId"])};
                    childernNodes.Add(item);
                }
                return childernNodes;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                return null;
            }
        }

        [HttpPut("{id}")]
        public int UpdateNodePairById(int id, [FromBody] NodePairs value)
        {
            try
            {
                sqlite_conn.Open();
                SqliteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "UPDATE nodepairs SET childNodeId = @childNodeId where parentNodeId = '" + id + "'";
                sqlite_cmd.Parameters.AddWithValue("@childNodeId", value.ChildId);
                int rowAffected = sqlite_cmd.ExecuteNonQuery();
                return rowAffected;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                return 0;
            }
        }

        [HttpPost]
        public int PostNode([FromBody] NodePairs value)
        {
                try
                {
                    sqlite_conn.Open();
                    SqliteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO `nodePairs` (parentNodeId, childNodeId) VALUES (@parentNodeId, @childNodeId);";
                        sqlite_cmd.Parameters.AddWithValue("@parentNodeId", value.ParentId);
                        sqlite_cmd.Parameters.AddWithValue("@childNodeId", value.ChildId);
                        int rowAffected = sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();
                        return rowAffected;
                }
                catch (System.Exception exception)
                {
                    System.Console.WriteLine(exception);
                    return 0;
                }
        }
    }
}