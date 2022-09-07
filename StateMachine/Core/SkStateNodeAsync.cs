/*******************************************************************************
SkStateNode
 
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
using System.Threading;
using System.Threading.Tasks;

namespace Sakaki_Entertainment.StateMachine.Core
{
    /// <summary>
    /// An asynchronous StateNode class of a defined state enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SkStateNodeAsync<T> where T: struct, IConvertible
    {
        protected SkStateMachineAsync<T> m_stateMachine;

        protected bool m_isAllowMoveNextState;
        protected readonly CancellationToken m_cancellationToken;

        public void MoveNextState(T nextState)
        {
            m_stateMachine.MoveState(nextState);
        }
        
        /// <summary>
        /// Get current state type
        /// </summary>
        public T StateType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stateType"></param>
        /// <param name="stateMachine"></param>
        public SkStateNodeAsync(T stateType, SkStateMachineAsync<T> stateMachine, CancellationToken token)
        {
            m_cancellationToken = token;
            m_stateMachine = stateMachine;
            StateType = stateType;
        }
        
        /// <summary>
        /// Use for State initialization
        /// </summary>
        public virtual async Task StateInitialize()
        {
            if (m_stateMachine.DefaultStateChangeDefaultEvent != null)
            {
                await m_stateMachine.DefaultStateChangeDefaultEvent(StateType, SkStateNodeStatusEnum.StateInitialize, m_cancellationToken);
            }
        }
        
        /// <summary>
        /// Will be called when state is entering
        /// </summary>
        /// <returns></returns>
        public virtual async Task StateEnter()
        {
            if (m_stateMachine.DefaultStateChangeDefaultEvent != null)
            {
                await m_stateMachine.DefaultStateChangeDefaultEvent(StateType,SkStateNodeStatusEnum.StateEnter, m_cancellationToken);
            }
        }

        /// <summary>
        /// Will be called when state is updating
        /// </summary>
        /// <returns></returns>
        public virtual async Task StateUpdate()
        {
            if (m_stateMachine.DefaultStateChangeDefaultEvent != null)
            {
                await m_stateMachine.DefaultStateChangeDefaultEvent(StateType,SkStateNodeStatusEnum.StateUpdate, m_cancellationToken);
            }
        }

        /// <summary>
        /// Will be called when state is exiting
        /// </summary>
        /// <returns></returns>
        public virtual async Task StateExit()
        {
            if (m_stateMachine.DefaultStateChangeDefaultEvent != null)
            {
                await m_stateMachine.DefaultStateChangeDefaultEvent(StateType,SkStateNodeStatusEnum.StateExit, m_cancellationToken);
            }
        }

        /// <summary>
        /// Used for state finalization
        /// </summary>
        public virtual async Task StateFinalize()
        {
            if (m_stateMachine.DefaultStateChangeDefaultEvent != null)
            {
                await m_stateMachine.DefaultStateChangeDefaultEvent(StateType, SkStateNodeStatusEnum.StateFinalize, m_cancellationToken);
            }

        }
    }
}