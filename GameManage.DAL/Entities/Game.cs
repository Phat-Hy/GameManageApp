using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManage.DAL.Entities
{
    public class Game
    {
        public int Id { get; set; }  // Unique identifier for each game
        public string Name { get; set; }  // Name of the game
        public string ExePath { get; set; }  // Path to the executable file
        public string FrontCover { get; set; }  // Base64 string for front cover image
        public string Banner { get; set; }  // Base64 string for banner image
        public string Publisher { get; set; }  // Publisher of the game
        public DateTime ReleaseDate { get; set; }  // Release date of the game
        public string Description { get; set; }  // Description of the game
    }
}
