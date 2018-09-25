/*******************************************************************************
InputTest
 
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

using System.Collections;
using System.Collections.Generic;
using SakakiEntertainment.InputProcessor.Core;
using UnityEngine;

namespace SakakiEntertainment.InputProcessor.Sample
{
    public class InputTest : MonoBehaviour
    {
        private enum InputProcessorEnum
        {
            AttackUp
        }

        // Use this for initialization
        IEnumerator Start()
        {
            InputProcessor<InputProcessorEnum> x = new InputProcessor<InputProcessorEnum>();
            x.KeyDatas.Add(new InputProcessor<InputProcessorEnum>.InputKeyData()
                           {
                               KeyNote = new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                         {
                                             Key = 1,
                                             Status = 1
                                         },
                               StartTime = 0f,
                               Duration = 1.6f
                           });
            x.KeyDatas.Add(new InputProcessor<InputProcessorEnum>.InputKeyData()
                           {
                               KeyNote = new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                         {
                                             Key = 2,
                                             Status = 1
                                         },
                               StartTime = 1.5f,
                               Duration = 2f
                           });
            x.KeyDatas.Add(new InputProcessor<InputProcessorEnum>.InputKeyData()
                           {
                               KeyNote = new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                         {
                                             Key = 3,
                                             Status = 1
                                         },
                               StartTime = 0f,
                               Duration = 1.6f
                           });
            Debug.Log(x.SimulateCombos(new List<InputProcessor<InputProcessorEnum>.KeyNoteData>()
                                       {
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 1,
                                               Status = 1
                                           },
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 2,
                                               Status = 1
                                           },
                                       }, 0f));
            Debug.Log(x.SimulateCombos(new List<InputProcessor<InputProcessorEnum>.KeyNoteData>()
                                       {
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 1,
                                               Status = 1
                                           },
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 2,
                                               Status = 1
                                           },
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 3,
                                               Status = 1
                                           },
                                       }, 0.4f));
            Debug.Log(x.SimulateCombos(new List<InputProcessor<InputProcessorEnum>.KeyNoteData>()
                                       {
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 1,
                                               Status = 1
                                           },
                                           new InputProcessor<InputProcessorEnum>.KeyNoteData()
                                           {
                                               Key = 3,
                                               Status = 1
                                           },
                                       }, 1.6f));
            yield break;
        }

    }
}