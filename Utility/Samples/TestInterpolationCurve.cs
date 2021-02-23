/*******************************************************************************
TestInterpolationCurve, a sample usage of Easing utility.
 
Author:
      Paulus Ery Wasito Adhi <paupawsan@gmail.com>

Copyright (c) 2021

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
using System.Collections;
using System.Collections.Generic;
using SakakiEntertainment.Utility;
using UnityEngine;

[ExecuteAlways]
public class TestInterpolationCurve : MonoBehaviour
{
    [SerializeField] private Easing.EasingTypeEnum _easingInTypeEnum;
    [SerializeField] private Easing.EasingTypeEnum _easingOutTypeEnum;

    [SerializeField] private AnimationCurve _easingCurve;

    private void OnValidate()
    {
        Easing.Init(20);
        
        _easingCurve = Easing.GenerateInterpolationCurve(_easingInTypeEnum, _easingOutTypeEnum);
    }

    private float _accumulatedTime = 0f;
    private void Update()
    {
        _accumulatedTime += Time.smoothDeltaTime;
        Debug.Log(_easingCurve.Evaluate(_accumulatedTime % 1f));
        Debug.Log(Easing.GetInterpolationValue( _easingInTypeEnum, _easingOutTypeEnum, _accumulatedTime % 1f));
    }
}
