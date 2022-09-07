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
using System.Threading.Tasks;
using Sakaki_Entertainment.StateMachine.Core;
using UnityEngine;

namespace Sakaki_Entertainment.StateMachine.Sample
{
    public class StateMachineAsyncSample : MonoBehaviour
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

        private class AwaitStateNode : SkStateNodeAsync<SystemLoadingStateEnum>
        {
            public AwaitStateNode(SystemLoadingStateEnum stateType, SkStateMachineAsync<SystemLoadingStateEnum> stateMachine, CancellationToken token) : base(stateType, stateMachine, token)
            {
            }

            public override async Task StateEnter()
            {
                Debug.Log(string.Format("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                    MethodBase.GetCurrentMethod().ToString(), string.Format("stateType:{0} stateStatus:{1}", StateType, SkStateNodeStatusEnum.StateEnter)));
                await Task.Delay(5000, m_cancellationToken);
            }

            public override async Task StateUpdate()
            {
                Debug.Log(string.Format("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                    MethodBase.GetCurrentMethod().ToString(), string.Format("stateType:{0} stateStatus:{1}", StateType, SkStateNodeStatusEnum.StateUpdate)));
                await m_stateMachine.Shutdown();
            }
        }

        private SkStateMachineAsync<SystemLoadingStateEnum> mySTM;
        private CancellationTokenSource cts;

        // Use this for initialization
        private void OnEnable()
        {
            cts = new CancellationTokenSource();
            mySTM = new SkStateMachineAsync<SystemLoadingStateEnum>(StateChangeEvent, cts.Token, true);
            mySTM.RegisterStateNode(SystemLoadingStateEnum.Shutdown, new AwaitStateNode(SystemLoadingStateEnum.Shutdown, mySTM, cts.Token));
            mySTM.StartStateMachine(SystemLoadingStateEnum.Init);
        }

        private void OnDisable()
        {
            cts.Cancel();
        }

        private async Task StateChangeEvent(SystemLoadingStateEnum stateType, SkStateNodeStatusEnum stateStatus, CancellationToken token)
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
                        await Task.Delay(5000, token);
                        await mySTM.MoveState(SystemLoadingStateEnum.Loading);
                    }
                }
                    break;
                case SkStateNodeStatusEnum.StateUpdate:
                {
                    if (stateType == SystemLoadingStateEnum.Loading)
                    {
                        await Task.Delay(5000, token);
                        await mySTM.MoveState(SystemLoadingStateEnum.Shutdown);
                    } else if (stateType == SystemLoadingStateEnum.Shutdown)
                    {
                        await mySTM.Shutdown();
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

            await Task.Delay(1);
        }
    }
}