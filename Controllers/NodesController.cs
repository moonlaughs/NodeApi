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
    [Route("api/nodes")]
    public class NodesController : ControllerBase
    {
        List<Node> nodes = new List<Node>();

        SqliteConnection sqlite_conn = new SqliteConnection("Data Source=NodesDB.sqlite;");

        
        [HttpGet]
        public IEnumerable<Node> ListAllNodes()
        {
            try
            {
                sqlite_conn.Open();
                SqliteDataReader sqlite_datareader;
                SqliteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM nodes";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    Node item = new Node(){NodeId = Convert.ToInt32(sqlite_datareader["nodeId"]), NodeName = sqlite_datareader["nodeName"].ToString()};
                    nodes.Add(item);
                }
                sqlite_conn.Close();
                return nodes;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                return null;
            }
        }

        [HttpGet("{id}")]
        public Node GetNodeById(int id)
        {
            try
            {
                sqlite_conn.Open();
                SqliteDataReader sqlite_datareader;
                SqliteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM nodes where nodeId = '" + id + "'";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                sqlite_datareader.Read();
                Node item = new Node(){NodeId = Convert.ToInt32(sqlite_datareader["nodeId"]), NodeName = sqlite_datareader["nodeName"].ToString()};
                return item;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                return null;
            }
        }

        [HttpPut("{id}")]
        public int UpdateNodeById(int id, [FromBody] Node value)
        {
            try
            {
                sqlite_conn.Open();
                SqliteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "UPDATE nodes SET nodeName = @nodeName where nodeId = '" + id + "'";
                sqlite_cmd.Parameters.AddWithValue("@nodeName", value.NodeName);
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
        public int PostNode([FromBody] Node value)
        {
                try
                {
                    sqlite_conn.Open();
                    SqliteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    // Random r = new Random();
                    // int randomNodeId = r.Next(0, 1000);
                    sqlite_cmd.CommandText = "INSERT INTO nodes (nodeName) VALUES(@nodeName)";
                        // sqlite_cmd.Parameters.AddWithValue("@nodeId", randomNodeId);
                        sqlite_cmd.Parameters.AddWithValue("@nodeName", value.NodeName);
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();
                        return 1;
                }
                catch (System.Exception exception)
                {
                    System.Console.WriteLine(exception);
                    return 0;
                }
        }

        [HttpDelete("{id}")]
        public int DeleteNode(int id)
        {
            try
            {
                    sqlite_conn.Open();
                    SqliteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "DELETE FROM nodes WHERE nodeId = " + id + ";";
                    int rowsAffected = sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();
                    return rowsAffected;
            }
            catch (System.Exception exception)
                {
                    System.Console.WriteLine(exception);
                    return 0;
            }
        }
    }
}
