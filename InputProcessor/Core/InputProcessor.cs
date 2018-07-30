/*******************************************************************************
InputProcessor
 
Author:
      Paulus Ery Wasito Adhi <paupawsan@gmail.com>

Copyright (c) 2018

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sakaki_Entertainment.InputProcessor.Core
{
    /// <summary>
    /// This class will map given key combos agains realtime input data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InputProcessor<T> where T : struct, IConvertible
    {
        /// <summary>
        /// Store key note id and it's status
        /// </summary>
        public struct KeyNoteData
        {
            /// <summary>
            /// Key could be ButtonID, touchid etc..
            /// </summary>
            public int Key;
            
            /// <summary>
            /// Status could be Up, Down, Pressed, Touched etc..
            /// </summary>
            public int Status;
        }

        /// <summary>
        /// This will store each corespondence Key note in the processor timeline
        /// </summary>
        public class InputKeyData
        {
            public KeyNoteData KeyNote;
            public float       StartTime;
            public float       Duration;
        }

        private float startTriggerTime;

        public List<InputKeyData> KeyDatas;

        public InputProcessor()
        {
            KeyDatas = new List<InputKeyData>();
            startTriggerTime = -1;
        }
        
        /// <summary>
        /// Check wheter the combo is detected
        /// </summary>
        /// <param name="keyNotes">Realtime input data</param>
        /// <param name="time">Current time</param>
        /// <returns></returns>
        public bool SimulateCombos(List<KeyNoteData> keyNotes, float time)
        {
            if (KeyDatas == null || KeyDatas.Count == 0 || keyNotes == null || keyNotes.Count == 0) return false;
            bool ret = true;
            
            // Is first triggered
            if (startTriggerTime < 0)
            {
                startTriggerTime = time;
            }

            // Offset the time
            float curTime = time - startTriggerTime;

            // Get valid InputKeys in the time line that life time covers the current time
            List<InputKeyData> validKeys =
                KeyDatas.FindAll(kd => (kd.StartTime <= curTime) &&
                                       (kd.Duration > 0 ? (kd.Duration + kd.StartTime) >= curTime : true));
            
            if (validKeys != null && validKeys.Count > 0)
            {
                for (int i = 0; i < validKeys.Count; i++)
                {
                    bool isTriggered = false;
                    for (int key = 0; key < keyNotes.Count; key++)
                    {
                        if (validKeys[i].KeyNote.Equals(keyNotes[key]))
                        {
                            isTriggered = true;
                            break;
                        }
                    }

                    ret = ret && isTriggered;
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }

        public void ResetTriggerTime()
        {
            startTriggerTime = -1;
        }
    }
}