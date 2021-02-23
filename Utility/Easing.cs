/*******************************************************************************
Easing, a porting and extension of https://easings.net/
 
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
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SakakiEntertainment.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class Easing
    {
        private delegate float EasingFunction(float inValue);

        public enum EasingTypeEnum : int
        {
            None = -1,
            Sinusoidal,
            Quad,
            Cubic,
        }

        private static List<EasingFunction> _easingInFunctionsList = new List<EasingFunction>()
        {
            rawSineEaseIn,
            rawQuadEaseIn,
            rawCubicEaseIn
        };

        private static List<EasingFunction> _easingOutFunctionsList = new List<EasingFunction>()
        {
            rawSineEaseOut,
            rawQuadEaseOut,
            rawCubicEaseOut
        };

        private static List<AnimationCurve> _interpolatedEasingInCurveList = new List<AnimationCurve>();
        private static List<AnimationCurve> _interpolatedEasingOutCurveList = new List<AnimationCurve>();

        private class EasingPairData
        {
            public EasingTypeEnum InType;
            public EasingTypeEnum OutType;
            public AnimationCurve InterpolatedCurve;
        }
        
        private static List<EasingPairData> _interpolatedEasingPairCurveList = new List<EasingPairData>();
        

        public static bool IsInitialized { get; private set; }

        public static void Init(int sampleCount, bool isForceInit = false)
        {
            if (!isForceInit && IsInitialized) return;
            
            _interpolatedEasingInCurveList.Clear();
            _interpolatedEasingOutCurveList.Clear();
            _interpolatedEasingPairCurveList.Clear();
            IsInitialized = false;
            PopulateInterpolatedCurve(sampleCount);
            PopulateInterpolatedCurve(sampleCount, false);
            IsInitialized = true;
        }

        private static void PopulateInterpolatedCurve(int sampleCount, bool isEasingIn = true)
        {
            foreach (int easingTypeIdx in Enum.GetValues(typeof(EasingTypeEnum)))
            {
                if (easingTypeIdx < 0) continue;
                var animCurve = new AnimationCurve();
                for (int i = 0; i < sampleCount; i++)
                {
                    var time = (float) i / (float) sampleCount;
                    var easingFunction = isEasingIn ? _easingInFunctionsList : _easingOutFunctionsList;
                    var value = easingFunction[easingTypeIdx].Invoke(time);
                    animCurve.AddKey(time, value);
                }

                for (int i = 0; i < sampleCount; i++)
                {
                    animCurve.SmoothTangents(i, 0.5f);
                }

                var interpolatedCurve = isEasingIn ? _interpolatedEasingInCurveList : _interpolatedEasingOutCurveList;
                interpolatedCurve.Add(animCurve);
            }
        }

        public static float GetInterpolationValue(EasingTypeEnum easingInType,
            EasingTypeEnum easingOutType, float time)
        {
            var animCurve = GenerateInterpolationCurve(easingInType, easingOutType);
            return animCurve.Evaluate(time);
        }
        
        public static AnimationCurve GenerateInterpolationCurve(EasingTypeEnum easingInType,
            EasingTypeEnum easingOutType)
        {
            AnimationCurve easeIn = null;
            AnimationCurve easeOut = null;

            if (easingInType != EasingTypeEnum.None)
            {
                easeIn = _interpolatedEasingInCurveList[(int) easingInType];
            }

            if (easingOutType != EasingTypeEnum.None)
            {
                easeOut = _interpolatedEasingOutCurveList[(int) easingOutType];
            }

            if (easeIn == null && easeOut == null) return null;
            if (easeIn != null && easeOut == null) return easeIn;
            if (easeIn == null && easeOut != null) return easeOut;

            AnimationCurve result = null;

            var pairCurve = _interpolatedEasingPairCurveList.Find(x => x.InType == easingInType && x.OutType == easingOutType);
            if (pairCurve == null)
            {
                result = new AnimationCurve();
                for (int i = 0; i < easeIn.length; i++)
                {
                    var time = (float) i / (float) easeIn.length;
                    var inValue = easeIn.Evaluate(time);
                    var outValue = easeOut.Evaluate(time);
                    result.AddKey(time / 2f, inValue / 2f);
                    if (i > 0)
                    {
                        result.AddKey(0.5f + time / 2f, 0.5f + outValue / 2f);
                    }
                }

                for (int i = 0; i < easeIn.length; i++)
                {
                    result.SmoothTangents(i, 0.5f);
                }
                pairCurve = new EasingPairData()
                {
                    InType = easingInType,
                    OutType = easingOutType,
                    InterpolatedCurve = result
                };
                _interpolatedEasingPairCurveList.Add(pairCurve);
            }
            else
            {
                result = pairCurve.InterpolatedCurve;
            }
            
            return result;
        }

        #region Easing Raw Function

        #region Sinusoidal

        /// <summary>
        /// Sinusoidal easing-in generator.
        /// https://easings.net/#easeInSine
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        private static float rawSineEaseIn(float inValue)
        {
            return 1f - Mathf.Cos((inValue * Mathf.PI) / 2f);
        }

        /// <summary>
        /// Sinusoidal easing-out generator.
        /// https://easings.net/#easeOutSine
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        private static float rawSineEaseOut(float inValue)
        {
            return Mathf.Sin((inValue * Mathf.PI) / 2f);
        }

        #endregion

        #region Quadruple

        /// <summary>
        /// Quadruple easing-in generator.
        /// https://easings.net/#easeInQuad
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        private static float rawQuadEaseIn(float inValue)
        {
            return inValue * inValue;
        }

        /// <summary>
        /// Quadruple easing-out generator.
        /// https://easings.net/#easeOutQuad
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        private static float rawQuadEaseOut(float inValue)
        {
            return 1f - (1f - inValue) * (1f - inValue);
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Cubic easing-in generator.
        /// https://easings.net/#easeInQuad
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        private static float rawCubicEaseIn(float inValue)
        {
            return inValue * inValue * inValue;
        }

        /// <summary>
        /// Cubic easing-out generator.
        /// https://easings.net/#easeOutCubic
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        private static float rawCubicEaseOut(float inValue)
        {
            return 1f - Mathf.Pow(1f - inValue, 3f);
            ;
        }

        #endregion

        #endregion
    }
}