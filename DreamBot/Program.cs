using PluginContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugins;
namespace DreamBot
{
    class DreamBot
    {
        public Dictionary<string, IPlugin> _Plugins;
        static void Main(string[] args)
        {
            new DreamBot();
        }
        public DreamBot()
        {
            _Plugins = new Dictionary<string, IPlugin>();
            ICollection<IPlugin> plugins = PluginLoader.Load();
        }
    }
}
