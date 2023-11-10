namespace Game.CMF.Input {
    using global::CMF;
    using UnityEngine;

    public class CharacterInputHandler : CharacterInput {
        public CharacterInput DefaultInput;

        [SerializeField] string m_ProxyName;
        JoystickInputProxy m_Proxy = null;
        [SerializeField] string m_JumpEventName;
        public bool ReceivedLastJumpEvent = false;

        private UI.ButtonEvent.EventListener m_JumpListener = null;
        private void Awake() {
            m_JumpListener = UI.ButtonEvent.Regist(m_JumpEventName, () => {
                ReceivedLastJumpEvent = true;
            });
        }
        private void Start() {
            if (JoystickInputProxy.Proxies.TryGetValue(m_ProxyName, out var proxy)) {
                m_Proxy = proxy;
            }

        }
        private void OnDestroy() {
            m_JumpListener?.Dispose();
            m_JumpListener = null;
        }

        public override float GetHorizontalMovementInput() {
            if (InGameUI_ChatWindow.Instance)
            {
                if (InGameUI_ChatWindow.Instance.isSelected)
                {
                    return 0.0f;
                }
            }

            if(TutorialManager.Instance) {
                if(!TutorialManager.Instance.isDone) {
                    return 0.0f;
                }
            }

            if(LetterMaster.Instance) {
            	if(LetterMaster.Instance.IsWriting) {
                	return 0.0f;
				}
			}

            if (m_Proxy == null) {
                return DefaultInput.GetHorizontalMovementInput();
            } else {
                return m_Proxy.Input.Horizontal;
            }
        }

        public override float GetVerticalMovementInput()
        {
            if (InGameUI_ChatWindow.Instance)
            {
                if (InGameUI_ChatWindow.Instance.isSelected)
                {
                    return 0.0f;
                }
            }

            if(TutorialManager.Instance) {
                if(!TutorialManager.Instance.isDone) {
                    return 0.0f;
                }
            }

            if(LetterMaster.Instance) {
            	if(LetterMaster.Instance.IsWriting) {
                	return 0.0f;
				}
			}

            if (m_Proxy == null) {
                return DefaultInput.GetVerticalMovementInput();
            } else {
                return m_Proxy.Input.Vertical;
            }
        }

        public override bool IsJumpKeyPressed() {
            if (InGameUI_ChatWindow.Instance)
            {
                if (InGameUI_ChatWindow.Instance.isSelected)
                {
                    return false;
                }
            }
            
            if(TutorialManager.Instance) {
                if(!TutorialManager.Instance.isDone) {
                    return false;
                }
            }

            if(LetterMaster.Instance) {
            	if(LetterMaster.Instance.IsWriting) {
                	return false;
				}
			}

            var jumpEvent = ReceivedLastJumpEvent;
            ReceivedLastJumpEvent = false;

            if (m_Proxy == null) {
                return DefaultInput.IsJumpKeyPressed() || jumpEvent;
            } else {
                return jumpEvent;
            }
        }
    }
}