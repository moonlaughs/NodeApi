using Microsoft.EntityFrameworkCore;

namespace NodesApi.Models
{
    public class NodeContext : DbContext
    {
        public NodeContext(DbContextOptions<NodeContext> options) : base(options)
        {

        }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodePairs> NodePairs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=nodesDB.sqlite");
    }
}