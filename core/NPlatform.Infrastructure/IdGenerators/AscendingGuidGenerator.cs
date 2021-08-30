/* Copyright 2010-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

namespace NPlatform.IdGenerators
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// A GUID generator that generates GUIDs in ascending order. To enable 
    /// an index to make use of the ascending nature make sure to use
    /// as the storage representation.
    /// Internally the GUID is of the form
    /// 8 bytes: Ticks from DateTime.UtcNow.Ticks
    /// 3 bytes: hash of machine name
    /// 2 bytes: low order bytes of process Id
    /// 3 bytes: increment
    /// </summary>
    [Obsolete("此ID生成器已过期，如果是需要生成平台通用id，请使用IdGenerator")]
    public class AscendingGuidGenerator : IIdGenerator
    {
        // private static fields
        private static readonly AscendingGuidGenerator __instance = new AscendingGuidGenerator();
        // public static properties

        /// <summary>
        /// Gets an instance of AscendingGuidGenerator.
        /// </summary>
        public static AscendingGuidGenerator Instance
        {
            get
            {
                return __instance;
            }
        }

        // public methods

        /// <summary>
        /// Generates an ascending Guid .
        /// </summary>
        /// <returns>A Guid.</returns>
        public object GenerateId()
        {
            return SnowflakeHelper.GenerateId();
        }


        /// <summary>
        /// Tests whether an id is empty.
        /// </summary>
        /// <param name="id">The id to test.</param>
        /// <returns>True if the Id is empty. False otherwise</returns>
        public bool IsEmpty(object id)
        {
            return SnowflakeHelper.IsEmpty(id);
        }
    }
}