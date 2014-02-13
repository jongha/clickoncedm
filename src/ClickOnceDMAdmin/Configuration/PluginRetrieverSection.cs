using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ClickOnceDMAdmin.Configuration
{
    public class PluginRetrieverSection : ConfigurationSection
    {
        [ConfigurationProperty("plugins", IsDefaultCollection = true)]
        public PluginElementCollection Plugins
        {
            get { return (PluginElementCollection)this["plugins"]; }
            set { this["plugins"] = value; }
        }
    }
}