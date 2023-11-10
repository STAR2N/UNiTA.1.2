# Interaction
유저의 상호작용을 제어

## 컴포넌트
### InteractionManager
Raycast 및 Interactable의 이벤트 실행을 담당

### Interactable
유저 컴포넌트의 기능이 실행될 수 있도록 이벤트를 받는 주체

### InteractableEvents
Interactable에서 실행되는 이벤트를 인스펙터를 사용하여 관리 할 수 있도록 표시

## 사용법
2가지를 개발하여야함
 1. Interactable의 이벤트를 받아 기능을 수행하는 기능시스템
 2. InteractionManager를 사용하기 위한 입력시스템