﻿#region Copyright and License Header
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
#endregion
using System;
using System.Collections.Generic;

namespace Platinum.PluginCore3.Classes
{
    public class PluginPreferences : Dictionary<string, dynamic>
    {
        #region Public Methods

        public T Get<T>(string key)
        {
            return ContainsKey(key) ? this[key] : (T)GetDefaultValue(typeof(T));
        }

        public T Get<T>(string key, T defaultValue)
        {
            return ContainsKey(key) ? this[key] : defaultValue;
        }

        #endregion Public Methods

        #region Private Methods

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        #endregion Private Methods
    }
}