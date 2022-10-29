using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core.Models
{
    public class BestTime
    {
        public BestTime(string name, int time)
        {
            Name = name;
            Time = time;
        }
        public string Name { get; }
        public int Time { get; }
    }
}
