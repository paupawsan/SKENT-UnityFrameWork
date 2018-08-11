/*******************************************************************************
SKStateMachine
 
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
using System.Collections.Generic;
using System.Reflection;

namespace SakakiEntertainment.StateMachine.Core
{
    /// <summary>
    /// An class of StateMachine of a defined state enum.
    /// </summary>
    /// <typeparam name="T">Any enum type</typeparam>
    public sealed class SkStateMachine<T> where T : struct, IConvertible
    {
        /// <summary>
        /// Store state node information
        /// </summary>
        private class StateNodeDataItem
        {
            public T              StateType;
            public SkStateNode<T> StateNode;
        }

        private List<StateNodeDataItem> m_stateNodeDataItems;

        private bool m_isShuttingDown = false;


        private T m_prevState = default(T);
        private T m_nextState = default(T);
        private T m_curState = default(T);

        public OnStateChange StateChangeEvent;
        
        /// <summary>
        /// Delegate for updating state note status
        /// </summary>
        /// <param name="stateType">Current state type</param>
        /// <param name="stateStatus">Current state status</param>
        public delegate IEnumerator OnStateChange(T stateType, SkStateNodeStatusEnum stateStatus);

        /// <summary>
        /// Construct state machine
        /// </summary>
        /// <param name="eventStatusCallback">State change status event callback</param>
        /// <param name="autoRegister">When value is on, state machine will be populated with state nodes of specified state type enum</param>
        public SkStateMachine(OnStateChange eventStatusCallback, bool autoRegister = false)
        {
            StateChangeEvent += eventStatusCallback;
            m_stateNodeDataItems = new List<StateNodeDataItem>();

            if (!autoRegister) return;
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                RegisterStateNode((T) value, new SkStateNode<T>((T) value, this));
            }
        }

        ~SkStateMachine()
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                UnRegisterStateNode((T) value);
            }
            
            m_stateNodeDataItems = null;
        }

        /// <summary>
        /// Get state node from stateType
        /// </summary>
        /// <param name="stateType">Target state type</param>
        /// <returns>Return state node of the specified state type</returns>
        StateNodeDataItem GetStateNode(T stateType)
        {
            return m_stateNodeDataItems.Find(node => node.StateType.Equals(stateType));
        }
        
        /// <summary>
        /// Move to next state
        /// </summary>
        /// <param name="nextStateType">State type to move to</param>
        public void MoveState(T nextStateType)
        {
            m_nextState = nextStateType;
        }
        
        /// <summary>
        /// Register state node
        /// </summary>
        /// <param name="stateType">State type to register</param>
        /// <param name="stateNode">State node to register</param>
        public void RegisterStateNode(T stateType, SkStateNode<T> stateNode)
        {
            StateNodeDataItem stateNodeDataItem = GetStateNode(stateType);
            if (stateNodeDataItem == null)
            {
                Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                                  MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", string.Format("StateType {0} Registered!", stateType)));
                m_stateNodeDataItems.Add(new StateNodeDataItem() {StateType = stateType, StateNode = stateNode});
                stateNode.StateInitialize();
            }
            else
            {
                Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                                  MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", string.Format("ERROR: StateType {0} already exist!", stateType)));
                m_stateNodeDataItems.Add(new StateNodeDataItem() {StateType = stateType, StateNode = stateNode});
            }
        }

        /// <summary>
        /// Unregister state node
        /// </summary>
        /// <param name="stateType">State type to unregister</param>
        public void UnRegisterStateNode(T stateType)
        {
            StateNodeDataItem stateNodeDataItem = GetStateNode(stateType);
            if (stateNodeDataItem != null)
            {
                Console.WriteLine("_pLog_ {0} [{1}@{2}] {3}", DateTime.UtcNow.Ticks, this.GetType(),
                                  MethodBase.GetCurrentMethod().ToString(), string.Format("{0}", ""));
                stateNodeDataItem.StateNode.StateFinalize();
                stateNodeDataItem.StateNode = null;
                m_stateNodeDataItems.Remove(stateNodeDataItem);
                stateNodeDataItem = null;
            }
        }

        /// <summary>
        /// Start state machine
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartStateMachine(T nextState)
        {
            //m_curState = m_prevState = m_nextState = m_stateNodeDataItems[0].StateType;
            m_nextState = nextState;
            while (!m_isShuttingDown)
            {
                if (!m_curState.Equals(m_nextState))
                {
                    //Exit prev curstate
                    yield return GetStateNode(m_curState).StateNode.StateExit();

                    m_prevState = m_curState;
                    m_curState = m_nextState;
                    //Move next state
                    yield return GetStateNode(m_nextState).StateNode.StateEnter();
                }
                else
                {
                    //Update
                    yield return GetStateNode(m_curState).StateNode.StateUpdate();
                }
                yield return null;
            }
            //Exit prev curstate
            yield return GetStateNode(m_curState).StateNode.StateExit();
        }

        /// <summary>
        /// Shutdown state machine
        /// </summary>
        public void Shutdown()
        {
            m_isShuttingDown = true;
        }
    }
}