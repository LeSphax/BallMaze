﻿using System;
using BallMaze.Inputs;
using GenericStateMachine;
using System.Collections.Generic;

namespace BallMaze.Cube
{
    internal abstract class E_Camera { }

    internal abstract class E_Delayed : E_Camera { }
    
    internal class E_Direction : E_Delayed
    {
        public Direction direction;

        public E_Direction(Direction direction)
        {
            this.direction = direction;
        }
    }

    internal class E_ChangePerspective : E_Delayed { }

    internal class E_FinishedAnimating : E_Camera { }

    internal class E_LevelChanged : E_Camera { }

    internal class E_InitCamera : E_Camera { }

    internal class CameraStateMachine : StateMachine<CameraStateMachine, E_Camera>
    {
        internal Queue<E_Delayed> nextEvents = new Queue<E_Delayed>();
        internal CameraController cameraController;
        
        void Awake()
        {
            cameraController = GetComponent<CameraController>();
        }

        void Start()
        {
            InputManager inputManager = GameObjects.GetInputManager();
            inputManager.DirectionEvent += ChangeDirection;
            inputManager.ChangePerspectiveEvent += ChangePerspective;
            GameObjects.GetLevelLoader().LevelChanged += LevelChanged;
        }

        private void ChangeDirection(Direction direction)
        {
            handleEvent(new E_Direction(direction));
        }

        private void ChangePerspective()
        {
            handleEvent(new E_ChangePerspective());
        }

        private void LevelChanged()
        {
            handleEvent(new E_LevelChanged());
        }

        internal override State<CameraStateMachine, E_Camera> DefineFirst()
        {
            return new S_Init(this);
        }
    }

}
