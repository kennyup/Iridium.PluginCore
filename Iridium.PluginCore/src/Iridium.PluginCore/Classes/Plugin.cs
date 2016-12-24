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

using System.Reflection;

namespace Platinum.PluginCore3.Classes
{
    /// <summary>
    /// A class representing a loaded plugin.
    /// </summary>
    public class Plugin<T> where T : IPlatinumPlugin
    {
        #region Public Properties

        public Assembly Assembly { get; set; } = null;
        public T Instance { get; set; } = default(T);
        public string Guid { get; set; }

        #endregion Public Properties
    }
}