/*******************************************************************************
StateMachineSample
 
Author:
      Paulus Ery Wasito Adhi <paupawsan@gmail.com>

Copyright (c) 2018, 2022

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
using System.Reflection;
using System.Threading;
using Sakaki_Entertainment.StateMachine.Core;
using UnityEngine;

namespace Sakaki_Entertainment.StateMachine.Sample
{
    public class StateMachineSample : MonoBehaviour
    {
        private enum SystemLoadingStateEnum
        {
            None,
            Init,
            Loading,
            Update,
            Shutdown,
            Finalize
        }

        private class CancellableWait : CustomYieldInstruction
        {
            private CancellationToken _token;
            private float _seconds = 0f;
            private float _curTime = 0f;
            
            public override bool keepWaiting =>  (Time.time - _curTime) < _seconds && !_token.IsCancellationRequested;

            public CancellableWait(float seconds, CancellationToken token = default)
            {
                _curTime = Time.time;
                _seconds = seconds;
            }
        }
        
        private class AwaitStateNode : SkStateNode<SystemLoadingStateEnum>
        {
            public AwaitStateNode(SystemLoadingStateEnum stateType, SkStateMachine<SystemLoadingStateEnum> stateMachine) : base(stateType, stateMachine)
            {
            }

            public override IEnumerator StateUpdate()
            {
                m_stateMachine.Shutdown();
                yield break;
            }
        }

        private SkStateMachine<SystemLoadingStateEnum> mySTM;
        private CancellationTokenSource cts;

        // Use this for initialization
        private void OnEnable()
        {
            cts = new CancellationTokenSource();
            mySTM = new SkStateMachine<SystemLoadingStateEnum>(StateChangeEvent, true);
            mySTM.RegisterStateNode(SystemLoadingStateEnum.Shutdown, new AwaitStateNode(SystemLoadingStateEnum.Shutdown, mySTM));
            StartCoroutine(mySTM.StartStateMachine(SystemLoadingStateEnum.Init));
        }

        private void OnDisable()
        {
            cts.Cancel();
        }

        private IEnumerator StateChangeEvent(SystemLoadingStateEnum stateType, SkStateNodeStatusEnum stateStatus)
        {
            Debug.Log(string.Format("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                MethodBase.GetCurrentMethod().ToString(), string.Format("stateType:{0} stateStatus:{1}", stateType, stateStatus)));
            switch (stateStatus)
            {
                case SkStateNodeStatusEnum.StateInitialize:
                    break;
                case SkStateNodeStatusEnum.StateEnter:
                {
                    if (stateType == SystemLoadingStateEnum.Init)
                    {
                        yield return new CancellableWait(5f, cts.Token);
                        mySTM.MoveState(SystemLoadingStateEnum.Loading);
                    }
                }
                    break;
                case SkStateNodeStatusEnum.StateUpdate:
                {
                    if (stateType == SystemLoadingStateEnum.Loading)
                    {
                        yield return new CancellableWait(5f, cts.Token);
                        mySTM.MoveState(SystemLoadingStateEnum.Shutdown);
                    } else if (stateType == SystemLoadingStateEnum.Shutdown)
                    {
                        mySTM.Shutdown();
                    }
                }
                    break;
                case SkStateNodeStatusEnum.StateExit:
                {
                }
                    break;
                case SkStateNodeStatusEnum.StateFinalize:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("stateStatus", stateStatus, null);
            }

            yield break;
        }
    }
}