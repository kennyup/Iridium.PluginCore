#region Copyright and License Header

/*

	Platinum Plugin Core

	Copyright (c) 2016 0xFireball, IridiumIon Software, ExaPhaser Industries

	Author(s): 0xFireball

	Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing
	permissions and limitations under the License.

*/

#endregion Copyright and License Header

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Iridium.PluginCore.Classes
{
    /// <summary>
    /// Represents a collection of plugins. The collection maintains a GUID cache
    /// to prevent loading a plugin multiple times.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PluginCollection<T> : Collection<Plugin<T>> where T : IPlatinumPlugin
    {
        /// <summary>
        /// Cache GUIDs to prevent loading a plugin multiple times
        /// </summary>
        private readonly List<string> _guidCache = new List<string>();

        protected override void InsertItem(int index, Plugin<T> item)
        {
            var currentGuid = item.Instance.PluginGuid;
            if (_guidCache.Contains(currentGuid)) return;
            base.InsertItem(index, item);
            _guidCache.Add(currentGuid);
        }

        protected override void RemoveItem(int index)
        {
            var pluginToRemove = this[index];
            _guidCache.Remove(pluginToRemove.Instance.PluginGuid);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            _guidCache.Clear(); //Reset cache
            base.ClearItems();
        }

        public Plugin<T> FindPluginByGuid(string searchGuid)
        {
            return this.Where(plugin => plugin.Guid == searchGuid).ToArray()[0];
        }
    }
}