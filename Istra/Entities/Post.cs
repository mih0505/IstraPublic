using System.Collections.Generic;

namespace Istra.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Worker> Workers { get; set; }
        public Post()
        {
            Workers = new List<Worker>();
        }
    }
}