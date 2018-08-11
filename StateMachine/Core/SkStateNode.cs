/*******************************************************************************
SkStateNode
 
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
using System.Collections;

namespace SakakiEntertainment.StateMachine.Core
{
    public enum SkStateNodeStatusEnum
    {
        StateInitialize,
        StateEnter,
        StateUpdate,
        StateExit,
        StateFinalize
    }
    
    /// <summary>
    /// An class of StateNode of a defined state enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SkStateNode<T> where T: struct, IConvertible
    {
        private SkStateMachine<T> m_stateMachine;

        private bool m_isAllowMoveNextState;

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
        public SkStateNode(T stateType, SkStateMachine<T> stateMachine)
        {
            m_stateMachine = stateMachine;
            StateType = stateType;
        }
        
        /// <summary>
        /// Use for State initialization
        /// </summary>
        public virtual void StateInitialize()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                m_stateMachine.StateChangeEvent(StateType, SkStateNodeStatusEnum.StateInitialize).MoveNext();
            }
        }
        
        /// <summary>
        /// Will be called when state is entering
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator StateEnter()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                yield return m_stateMachine.StateChangeEvent(StateType,SkStateNodeStatusEnum.StateEnter);
            }

            yield break;
        }

        /// <summary>
        /// Will be called when state is updating
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator StateUpdate()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                yield return m_stateMachine.StateChangeEvent(StateType,SkStateNodeStatusEnum.StateUpdate);
            }

            yield break;
        }

        /// <summary>
        /// Will be called when state is exiting
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator StateExit()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                yield return m_stateMachine.StateChangeEvent(StateType,SkStateNodeStatusEnum.StateExit);
            }

            yield break;
        }

        /// <summary>
        /// Used for state finalization
        /// </summary>
        public virtual void StateFinalize()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                m_stateMachine.StateChangeEvent(StateType,SkStateNodeStatusEnum.StateFinalize).MoveNext();
            }

        }
    }
}