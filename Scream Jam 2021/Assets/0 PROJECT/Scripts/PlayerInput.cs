// GENERATED AUTOMATICALLY FROM 'Assets/0 PROJECT/Scripts/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace ScreamJam
{
    public class @PlayerInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""input"",
            ""id"": ""76771790-bfc2-43cf-8730-96620f8fd574"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""1265f2cf-ed34-4f5c-9dc4-2ee3982c8c3b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseStairs"",
                    ""type"": ""Button"",
                    ""id"": ""b7c227fb-1455-48c8-b1aa-9b14ad0d274a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseCross"",
                    ""type"": ""Button"",
                    ""id"": ""1a3278fd-19b0-4c55-8ab9-72596c003187"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hide"",
                    ""type"": ""Button"",
                    ""id"": ""5f9d36a6-9419-47ec-9216-55de73a3f084"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""1d1701d9-df33-4c6f-bd7a-23466d18a45e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""a11565e0-770f-4f37-a5e3-c08acd6d4ae1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e30b0335-b8ca-4e66-b6b0-4a3fa433e26a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ee18d709-609a-497f-9f3b-af8d328e5fcc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d2ac28eb-1611-492c-87b3-bb08745d2cb9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b6916b5a-e56d-4037-98dd-8cf0a03bb045"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b4a3fff1-149d-431c-bf86-bc22a1d1782d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""0cca1c1f-80e9-4b2b-b60b-fd7c32bc2942"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0fc4e2b8-3354-4965-8c60-62f2917e8058"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c449dc19-ca57-4316-b52d-6cfd0bc36ba8"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d7b4936b-d56a-4092-b19a-be40f2b23fc8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseStairs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a93d4aa0-c304-4155-a269-1284b2fd400a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseStairs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58e884a7-65e9-42a4-aa59-31dd18eafc10"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseStairs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""127a82a4-9596-4398-ab48-581ccc444fb0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseStairs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9eb31261-2193-40a5-a987-5355ae8f7bec"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseStairs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66b5e79a-8158-4edc-ae79-7a3b2214c953"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseCross"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7516074-3716-462d-9034-185463a693ca"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseCross"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5631ac6b-2c16-4bc4-a99d-1550cf072428"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54d393a4-9dfd-423b-a166-0aaf5d8b19fb"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1f5f2fc-b389-4f18-94fc-1f0ec43351bd"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23838f65-eaca-4eda-b103-aaa320a0da95"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // input
            m_input = asset.FindActionMap("input", throwIfNotFound: true);
            m_input_Move = m_input.FindAction("Move", throwIfNotFound: true);
            m_input_UseStairs = m_input.FindAction("UseStairs", throwIfNotFound: true);
            m_input_UseCross = m_input.FindAction("UseCross", throwIfNotFound: true);
            m_input_Hide = m_input.FindAction("Hide", throwIfNotFound: true);
            m_input_Pause = m_input.FindAction("Pause", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // input
        private readonly InputActionMap m_input;
        private IInputActions m_InputActionsCallbackInterface;
        private readonly InputAction m_input_Move;
        private readonly InputAction m_input_UseStairs;
        private readonly InputAction m_input_UseCross;
        private readonly InputAction m_input_Hide;
        private readonly InputAction m_input_Pause;
        public struct InputActions
        {
            private @PlayerInput m_Wrapper;
            public InputActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_input_Move;
            public InputAction @UseStairs => m_Wrapper.m_input_UseStairs;
            public InputAction @UseCross => m_Wrapper.m_input_UseCross;
            public InputAction @Hide => m_Wrapper.m_input_Hide;
            public InputAction @Pause => m_Wrapper.m_input_Pause;
            public InputActionMap Get() { return m_Wrapper.m_input; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(InputActions set) { return set.Get(); }
            public void SetCallbacks(IInputActions instance)
            {
                if (m_Wrapper.m_InputActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_InputActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnMove;
                    @UseStairs.started -= m_Wrapper.m_InputActionsCallbackInterface.OnUseStairs;
                    @UseStairs.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnUseStairs;
                    @UseStairs.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnUseStairs;
                    @UseCross.started -= m_Wrapper.m_InputActionsCallbackInterface.OnUseCross;
                    @UseCross.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnUseCross;
                    @UseCross.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnUseCross;
                    @Hide.started -= m_Wrapper.m_InputActionsCallbackInterface.OnHide;
                    @Hide.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnHide;
                    @Hide.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnHide;
                    @Pause.started -= m_Wrapper.m_InputActionsCallbackInterface.OnPause;
                    @Pause.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnPause;
                    @Pause.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnPause;
                }
                m_Wrapper.m_InputActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @UseStairs.started += instance.OnUseStairs;
                    @UseStairs.performed += instance.OnUseStairs;
                    @UseStairs.canceled += instance.OnUseStairs;
                    @UseCross.started += instance.OnUseCross;
                    @UseCross.performed += instance.OnUseCross;
                    @UseCross.canceled += instance.OnUseCross;
                    @Hide.started += instance.OnHide;
                    @Hide.performed += instance.OnHide;
                    @Hide.canceled += instance.OnHide;
                    @Pause.started += instance.OnPause;
                    @Pause.performed += instance.OnPause;
                    @Pause.canceled += instance.OnPause;
                }
            }
        }
        public InputActions @input => new InputActions(this);
        public interface IInputActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnUseStairs(InputAction.CallbackContext context);
            void OnUseCross(InputAction.CallbackContext context);
            void OnHide(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
        }
    }
}
