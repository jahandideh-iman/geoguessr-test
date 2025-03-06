#nullable enable
using Arman.Foundation.Core.ServiceLocating;
using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Game
{
    public class BoardDatabase : Service
    {
        public BoardDefinition[] BoardDefinitions { get; }
        public BoardDatabase(IEnumerable<BoardDefinition> boardDefinition)
        {
            BoardDefinitions = boardDefinition.ToArray();
        }
    }
}