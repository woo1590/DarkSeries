# DarkSeries 코드 분석 문서

## 1. 문서 범위

이 문서는 `Assets/Scripts` 아래의 직접 작성된 C# 코드를 기준으로 현재 구조와 동작을 정리한다.

- `PlayerActionMap.cs`는 Unity Input System이 `PlayerActionMap.inputactions`에서 자동 생성한 코드이므로 구현 분석 대상에서 제외한다.
- 씬, Animator Controller, Animation Clip은 코드와 연결되는 범위에서만 설명한다.
- 이 문서는 현재 구현을 기록한 분석 문서이며 코드 수정 제안은 마지막의 주의 사항에 분리했다.

## 2. 전체 구조

```text
Player
├─ PlayerInputController      입력 액션과 이동 입력값 관리
├─ PlayerMoveController       Rigidbody2D 이동, 점프, 지면 판정
└─ SwordMaster
   ├─ PlayerData              이동·점프 수치 데이터
   ├─ SwordMasterAnimData     Animator 파라미터 이름과 해시
   ├─ SwordMasterAttackController
   │                          연속 베기 인덱스와 입력 버퍼
   └─ StateMachine<SwordMaster>
      ├─ Idle / Walk / Run
      ├─ Jump / JumpToFall / Fall / Land
      ├─ SlashAttack
      └─ CrouchStart / CrouchHold / CrouchEnd
```

핵심 설계는 `SwordMaster`가 Unity 생명주기를 받고, 실제 행동 규칙은 현재 상태 객체에 위임하는 유한 상태 머신 방식이다.

## 3. 실행 흐름

### 초기화

1. `Player.Awake()`가 `SpriteRenderer`, `Animator`, `PlayerMoveController`, `PlayerInputController`를 가져온다.
2. `SwordMaster.Awake()`가 공격 컨트롤러를 가져오고 Animator 파라미터 및 상태 경로 해시를 초기화한다.
3. `SwordMaster.Start()`가 상태 머신과 상태 객체를 만들고 `Idle`을 최초 상태로 지정한다.
4. 각 상태의 `Enter()`는 베이스 상태를 통해 대응 Animator 상태와 파라미터를 즉시 동기화한다.

### 매 프레임

- `SwordMaster.Update()`
  - Movement와 프레임 단위 Attack, Jump, Crouch 입력 스냅샷을 갱신한다.
  - 현재 상태의 `Update()`를 호출한다.
- `SwordMaster.LateUpdate()`
  - 현재 상태의 `LateUpdate()`를 호출한다.
  - 기본 상태는 A/D 입력을 `UpdateFacing()`에 전달하고, SlashAttack은 `LateUpdate()`를 비워 방향 입력을 무시한다.
  - Animator가 코드 상태와 다른 상태로 전환되었으면 현재 코드 상태의 애니메이션으로 다시 맞춘다.
- `SwordMaster.FixedUpdate()`
  - 현재 상태의 `FixedUpdate()`를 호출한다.
  - 입력의 X 방향으로 실제 Rigidbody2D 이동을 수행한다.
- `PlayerMoveController.FixedUpdate()`
  - 캐릭터 아래쪽의 `OverlapBox`로 Platform 레이어 접촉 여부를 갱신한다. 같은 검사는 `Awake()`에서도 한 번 실행해 첫 프레임 상태를 안정화한다.

## 4. 공통 시스템

### `IState<T>`

모든 상태가 구현해야 하는 생명주기를 정의하는 추상 베이스 클래스다.

- `Enter()` / `Exit()`: 상태 진입과 종료
- `Update()` / `LateUpdate()` / `FixedUpdate()`: Unity 업데이트 단계에 대응
- `OnAnimationEnd()`: Animation Event가 상태에 애니메이션 종료를 알리는 선택적 콜백

이름은 인터페이스처럼 보이지만 실제로는 추상 클래스다.

### `StateMachine<T>`

상태 인스턴스를 타입별 Dictionary에 보관한다. `ChangeState<TState>()`가 호출되면 현재 상태의 `Exit()`, 다음 상태 지정, 다음 상태의 `Enter()` 순서로 전환한다. 등록되지 않은 상태와 현재 상태로의 중복 전환은 수행하지 않으며, Update 계열과 Animation Event 전달은 현재 상태가 없을 때도 안전하게 무시한다.

상태는 실행 중 새로 생성하지 않고 시작 시 한 번 생성해 재사용한다. 따라서 각 상태 객체 안에 필드를 추가할 경우 다음 진입에도 값이 유지된다는 점을 고려해야 한다.

### `PlayerInputController`

Input System 자동 생성 래퍼인 `PlayerActionMap`을 소유한다.

- 활성화될 때 전체 액션을 Enable
- 비활성화될 때 전체 액션을 Disable
- Movement는 폴링하여 `movementInput`에 보관
- Attack과 Jump의 이번 프레임 입력, Crouch의 시작·해제·유지 상태를 매 프레임 스냅샷으로 보관
- 상태 객체는 Input Action 이벤트를 직접 구독하지 않고 해당 스냅샷만 평가
- 파괴될 때 자동 생성 Input Actions 래퍼를 Dispose

현재 입력 구성은 다음과 같다.

| 액션 | 바인딩 | 코드 사용 방식 |
|---|---|---|
| Attack | 마우스 왼쪽 버튼 | 프레임 입력 스냅샷 |
| Movement | A/D 2D Vector | 매 프레임 폴링 |
| Jump | Space | 프레임 입력 스냅샷 |
| Crouch | S | 시작·해제·유지 입력 스냅샷 |
| Run | Left Shift | Input Action에는 존재하지만 이번 리팩터링 범위에서는 제외 |

### `PlayerMoveController`

Rigidbody2D의 `linearVelocityX/Y`를 직접 변경한다.

- `Move(direction)`: `moveSpeed * direction`을 X 속도로 설정
- `Jump()`: `jumpPower`를 Y 속도로 설정
- 지면 판정: 캐릭터 위치에 offset을 더한 지점에서 Box overlap 검사
- Scene 뷰 선택 시 지면 판정 박스를 Gizmo로 표시

`moveSpeed`와 `jumpPower`는 상태가 진입할 때 현재 행동에 맞게 설정한다.

### `PlayerData`

ScriptableObject로 캐릭터 수치를 에셋에 분리한다.

- 기본 이동 속도
- 걷기 속도 배율
- 달리기 속도 배율
- 기본 점프 힘
- 점프 힘 배율

실제 걷기 속도는 `baseSpeed * walkSpeedModifier`, 달리기 속도는 `baseSpeed * runSpeedModifier`, 점프 힘은 `baseJumpPower * jumpPowerModifier`로 계산한다.

## 5. SwordMaster 구성 요소

### `SwordMaster`

Player의 구체 캐릭터 구현이며 다음 역할을 묶는다.

- 상태 머신 생성과 상태 등록
- Unity 업데이트를 상태 머신에 전달
- 물리 이동 및 바라보는 방향 갱신
- 코드 상태 진입 시 Animator 파라미터와 재생 상태를 동기화
- LateUpdate에서 상태–애니메이션 불일치를 감지해 현재 코드 상태를 기준으로 복구
- 현재 애니메이션과 완료 시점이 일치하는 Animation Event만 현재 상태에 전달
- `OnGUI()`로 현재 상태 이름을 화면에 표시하는 디버그 UI 제공

현재 등록된 상태는 Idle, Walk, SlashAttack, Jump, JumpToFall, Fall, Land, CrouchStart, CrouchHold, CrouchEnd다.

### `SwordMasterAnimData`

Inspector에 저장된 Animator 파라미터 문자열과 Controller의 전체 상태 경로를 정수 해시로 변환한다. 코드 상태가 바뀌면 관련 bool을 한곳에서 초기화하고 대응 Animator 상태를 직접 재생한다.

| 파라미터 | 사용 목적 |
|---|---|
| Idle | 대기 애니메이션 |
| Walk | 걷기 애니메이션 |
| Run | 달리기 애니메이션 |
| RunFast | 빠른 달리기 확장용 |
| Attack | 공격 애니메이션 |
| IsGround | 지상/공중 분기 |
| StartFall | 낙하 전환 애니메이션 |
| Fall | 낙하 루프 |
| Land | 착지 애니메이션용 |
| Crouch | 앉기 시작/종료 제어 |

`Idle`, `Walk`, 공중 상태, 공격 콤보, 앉기 시작·종료의 전체 Animator 상태 경로 해시를 보관한다. CrouchHold는 별도 클립이 없으므로 완료된 CrouchStart 상태를 유지한다. Run 관련 해시와 동작은 기존 상태에 남아 있으며 이번 리팩터링 대상에서 제외된다.

### `SwordMasterAttackController`

연속 베기 상태를 관리한다.

- `BufferSlash()`: 공격 중 다음 공격 입력을 예약
- `ConsumeBuffer()`: 예약 입력이 있으면 소비하고 true 반환
- `IncreaseCombo()`: 콤보 인덱스를 증가시키며 3을 넘으면 0으로 순환
- `ResetCombo()`: 입력 버퍼와 콤보 인덱스를 초기화

현재 콤보 인덱스 범위는 0~3이다.

## 6. 상태별 동작

### 지상 이동 상태

#### `SwordMaster_Idle`

- Idle 애니메이션과 파라미터를 동기화하고 이동 속도를 0으로 둔다.
- 이동 입력이 생기면 Walk로 전환한다.
- 지면에서 떨어지면 JumpToFall로 전환한다.
- Crouch, Attack, Jump 순으로 프레임 입력을 평가하고 각 상태로 전환한다.

#### `SwordMaster_Walk`

- Walk 애니메이션과 파라미터를 동기화한다.
- 걷기 속도를 데이터의 기본 속도와 배율로 계산한다.
- 이동 입력이 취소되면 Idle로 전환한다.
- Crouch, Attack 또는 Jump 입력 시 해당 상태로 전환한다.
- 지면에서 떨어지면 JumpToFall로 전환한다.

#### `SwordMaster_Run`

- Run Animator bool을 켠다.
- 달리기 속도를 데이터의 기본 속도와 배율로 계산한다.
- 현재 파일은 존재하지만 `SwordMaster.InitializeStates()`에 등록되지 않았고 Run 입력 콜백도 연결되지 않아 실행 경로가 완성되지 않았다.

### 공중 상태

#### `SwordMaster_Jump`

- Jump 애니메이션과 공중 파라미터를 동기화한다.
- 데이터에서 점프 힘을 계산하고 Rigidbody2D에 적용한다.
- Y 속도가 0 이하가 되면 JumpToFall로 전환한다.

#### `SwordMaster_JumpToFall`

- JumpToFall 애니메이션과 공중 파라미터를 동기화한다.
- 전환 애니메이션이 끝나면 Fall로 이동한다.
- 먼저 지면에 닿으면 Crouch 입력 유지 여부에 따라 CrouchStart 또는 Land로 이동한다.

#### `SwordMaster_Fall`

- Fall 애니메이션과 파라미터를 동기화한다.
- 지면이 감지되면 Crouch 입력 유지 여부에 따라 CrouchStart 또는 Land로 전환한다.
- 착지 모션과 CrouchStart 모션이 같은 점을 고려해, Crouch 입력을 유지한 착지에서는 Land를 건너뛰어 같은 모션이 두 번 재생되지 않게 한다.

#### `SwordMaster_Land`

- Land 애니메이션과 지상 파라미터를 동기화하고 이동 속도를 0으로 둔다.
- 착지 애니메이션 종료 시 이동 입력이 있으면 Walk, 없으면 Idle로 이동한다.

### 공격 상태

#### `SwordMaster_SlashAttack`

- 현재 콤보 인덱스에 대응하는 Slash 애니메이션과 Attack 파라미터를 동기화하고 이동 속도를 0으로 설정한다.
- 공격 상태의 `LateUpdate()`에서는 A/D 입력을 처리하지 않아 공격 방향을 고정한다.
- 공격 중 추가 공격 입력을 버퍼에 저장한다.
- 애니메이션 종료 시 버퍼가 없으면 콤보를 초기화하고 이동 입력에 따라 Walk 또는 Idle로 돌아간다.
- 버퍼가 있으면 콤보 인덱스를 증가시키고 코드에서 다음 공격 애니메이션을 직접 재생한다.

### 앉기 상태

#### `SwordMaster_CrouchStart`

- CrouchStart 애니메이션과 파라미터를 동기화하고 이동 속도를 0으로 만든다.
- 시작 중 입력을 놓으면 즉시 CrouchEnd로 전환하고, 유지한 채 애니메이션이 끝나면 CrouchHold로 이동한다.

#### `SwordMaster_CrouchHold`

- 완료된 CrouchStart 모션과 이동 속도 0을 Crouch 입력이 취소될 때까지 유지한다.
- 입력 취소 시 CrouchEnd로 이동한다.

#### `SwordMaster_CrouchEnd`

- CrouchEnd 애니메이션을 직접 재생하고 이동 속도를 0으로 유지한다.
- 종료 애니메이션이 끝난 뒤 이동 입력이 있으면 Walk, 없으면 Idle로 이동한다.

## 7. 상태 전환 요약

```mermaid
stateDiagram-v2
    [*] --> Idle
    Idle --> Walk: 이동 입력
    Idle --> Jump: 점프 입력
    Idle --> SlashAttack: 공격 입력
    Idle --> CrouchStart: 앉기 입력
    Idle --> JumpToFall: 지면 이탈

    Walk --> Idle: 이동 취소
    Walk --> Jump: 점프 입력
    Walk --> SlashAttack: 공격 입력
    Walk --> CrouchStart: 앉기 입력
    Walk --> JumpToFall: 지면 이탈

    Jump --> JumpToFall: Y 속도 <= 0
    JumpToFall --> Fall: 애니메이션 종료
    JumpToFall --> Land: 지면 접촉, 앉기 미입력
    JumpToFall --> CrouchStart: 지면 접촉, 앉기 유지
    Fall --> Land: 지면 접촉, 앉기 미입력
    Fall --> CrouchStart: 지면 접촉, 앉기 유지
    Land --> Walk: 이동 입력 있음
    Land --> Idle: 이동 입력 없음

    SlashAttack --> SlashAttack: 버퍼 입력 있음
    SlashAttack --> Walk: 버퍼 입력 없음, 이동 입력 있음
    SlashAttack --> Idle: 버퍼 입력 없음, 이동 입력 없음

    CrouchStart --> CrouchHold: 애니메이션 종료
    CrouchStart --> CrouchEnd: 앉기 입력 조기 취소
    CrouchHold --> CrouchEnd: 앉기 입력 취소
    CrouchEnd --> Walk: 애니메이션 종료, 이동 입력 있음
    CrouchEnd --> Idle: 애니메이션 종료, 이동 입력 없음
```

Run 상태는 파일과 Animator 파라미터는 있지만 현재 전환 그래프에는 연결되지 않는다.

## 8. 현재 확인되는 주의 사항

1. `SwordMaster_Run`은 생성 및 등록되지 않으며 Run 입력도 상태 흐름에 연결되지 않는다. 이번 상태–애니메이션 동기화 리팩터링에서도 명시적으로 제외했다.
2. Jump 상태가 매 프레임 `GetComponent<Rigidbody2D>()`를 호출한다. 기능상 문제는 없지만 이미 `PlayerMoveController`가 같은 Rigidbody2D를 캐시하고 있다.
3. 여러 상태 파일에 사용하지 않는 namespace와 빈 Update 계열 override가 남아 있다. 현재 동작에는 영향을 주지 않는다.
4. `Land` 파라미터 이름과 해시는 보존되지만 현재 Animator Controller에는 Land 파라미터가 없고, Land 상태는 전체 상태 경로를 직접 재생한다.
5. Animation Clip의 기존 `OnAnimationEnd` 이벤트는 유지된다. 코드에서도 비루프 애니메이션의 정규화 시간을 확인하므로 이벤트가 누락되더라도 상태 종료를 진행할 수 있다.

## 9. 기능 확장 시 확인 순서

새 상태를 추가할 때는 다음 연결을 함께 확인해야 한다.

1. 상태 클래스 작성
2. `SwordMaster.InitializeStates()`에서 인스턴스 생성 및 등록
3. 진입 가능한 이전 상태에서 `ChangeState<T>()` 호출
4. `PlayerInputController`의 프레임 입력 스냅샷 중 필요한 값을 상태에서 평가
5. `SwordMasterAnimationState`와 `SwordMasterAnimData.GetStateHash()`에 대응 Animator 전체 경로를 추가
6. 상태의 `animationState`를 오버라이드하고 진입 시 중앙 동기화가 적용되는지 확인
7. 비루프 애니메이션은 정규화 시간 또는 Animation Event로 상태 종료를 처리
8. 모든 `ChangeState<T>()` 호출 뒤 이전 상태 로직이 계속되지 않도록 즉시 반환 여부 확인

