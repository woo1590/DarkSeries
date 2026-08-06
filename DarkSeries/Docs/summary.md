# DarkSeries 저장소 구조 요약

## 1. 프로젝트 개요

이 저장소는 Unity 6 기반의 2D 액션 게임 프로젝트다. 현재 구현의 중심은 `SwordMaster` 플레이어이며, 입력·물리 이동·Animator·제네릭 상태 머신을 조합해 대기, 걷기, 점프, 낙하, 착지, 연속 베기, 앉기 상태를 처리한다.

- Unity 버전: `6000.5.0f1`
- 렌더링: Universal Render Pipeline 2D
- 입력: Unity Input System
- 주 실행 씬: `DarkSeries/Assets/Scenes/SampleScene.unity`
- 핵심 설계: `StateMachine<SwordMaster>`에 상태 객체를 등록하고 Unity 생명주기와 Animation Event를 현재 상태에 전달

## 2. 최상위 구조

```text
DarkSeries/
├─ Assets/
│  ├─ Scenes/                 실제 게임 씬
│  ├─ Scripts/
│  │  ├─ 01.Controllers/      상태 머신, 플레이어, 이동 및 공격 제어
│  │  ├─ 02.Data/             ScriptableObject 기반 플레이어 수치
│  │  └─ 03.Inputs/           Input Actions 원본과 자동 생성 C# 래퍼
│  ├─ Resources/              SwordMaster 원본 스프라이트 리소스
│  └─ Settings/               URP 2D 렌더러와 씬 템플릿
├─ Docs/                      코드 분석 및 Git 작업 문서
├─ Packages/                  Unity 패키지 의존성
└─ ProjectSettings/           Unity 프로젝트 전역 설정
```

루트의 `AGENTS.md`는 작업 범위, Git 절차, Unity 에셋 수정 시 주의사항을 정의한다. 실제 Unity 프로젝트 루트는 저장소 아래의 `DarkSeries/` 디렉터리다.

## 3. 핵심 코드 구조와 실행 흐름

### 플레이어 구성

- `DarkSeries/Assets/Scripts/01.Controllers/Player/Player.cs`
  - `SpriteRenderer`, `Animator`, `PlayerMoveController`, `PlayerInputController` 참조를 캐시한다.
  - 바라보는 방향에 따라 스프라이트를 반전한다.
- `DarkSeries/Assets/Scripts/01.Controllers/Player/PlayerInputController.cs`
  - 자동 생성된 `PlayerActionMap`을 생성하고 활성화한다.
  - 매 프레임 Movement 값을 읽는다.
- `DarkSeries/Assets/Scripts/01.Controllers/Player/PlayerMoveController.cs`
  - `Rigidbody2D.linearVelocityX/Y`로 이동과 점프를 적용한다.
  - `Platform` 레이어를 대상으로 `OverlapBox` 지면 검사를 수행한다.
- `DarkSeries/Assets/Scripts/02.Data/PlayerData.cs`
  - 기본 이동 속도, 점프 힘과 상태별 배율을 ScriptableObject로 제공한다.

### 상태 머신

- `DarkSeries/Assets/Scripts/01.Controllers/State/IState.cs`
  - 상태의 Enter/Exit 및 Unity Update 계열 메서드 계약을 정의한다.
- `DarkSeries/Assets/Scripts/01.Controllers/State/StateMachine.cs`
  - 상태를 런타임 타입으로 보관하고 `ChangeState<TState>()`로 전환한다.
- `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/SwordMaster.cs`
  - 상태를 생성·등록하고 Update/LateUpdate/FixedUpdate를 현재 상태로 전달한다.
  - 이동 입력과 방향 전환, 물리 이동도 함께 조정한다.
  - Animation Event의 `OnAnimationEnd()`를 현재 상태로 전달한다.
- `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/States/`
  - Idle, Walk, Run, Jump, JumpToFall, Fall, Land, SlashAttack, CrouchStart/Hold/End 상태가 분리되어 있다.
  - 상태 진입과 종료 때 Input Action 콜백을 구독·해제하고 Animator bool/int 파라미터를 변경한다.

### Animator 및 애니메이션

- `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/SwordMasterAnimData.cs`
  - Inspector에 저장된 Animator 파라미터 이름을 해시로 변환한다.
- `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/Animations/SwordMaster.controller`
  - 코드 상태와 별개인 Animator 상태 및 전이 그래프를 보유한다.
- `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/Animations/*.anim`
  - 스프라이트 프레임과 일부 `OnAnimationEnd` Animation Event를 보유한다.

## 4. 복잡하거나 위험해 보이는 파일

위험도는 현재 코드와 직렬화 내용을 정적으로 검토한 결과이며, 실제 수정 전에는 Unity Editor에서 연결 상태를 다시 확인해야 한다.

### 높음

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/SwordMaster.cs`

- 플레이어 입력, 상태 머신, 물리 이동, 방향 전환, 애니메이션 종료 전달이 한 클래스에 집중되어 파급 범위가 크다.
- 런타임 스크립트에서 `UnityEditor`를 import하고 있어 플레이어 빌드 컴파일에 문제가 될 수 있다.
- `animationData`, `playerData`, 필수 컴포넌트가 씬에서 누락되면 초기화 중 null 참조가 발생한다.
- `SwordMaster_Run` 파일은 존재하지만 `InitializeStates()`에 등록되지 않아 현재 코드 경로에서 사용할 수 없다.

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/Animations/SwordMaster.controller`

- 코드 상태 머신과 별도의 Animator 전이 그래프를 유지하므로 양쪽의 상태와 파라미터가 어긋날 수 있다.
- `Idle`, `Walk`, `Attack`, `ComboIndex`, `IsGround`, `StartFall`, `Fall`, `Crouch`, `Run` 이름이 코드의 해시 및 상태 로직과 정확히 일치해야 한다.
- 공격 콤보와 공중·앉기 전이가 여러 서브 상태 및 조건에 분산되어 수동 YAML 수정은 특히 위험하다.

#### `DarkSeries/Assets/Scenes/SampleScene.unity`

- `SwordMaster` GameObject의 스크립트, Animator Controller, 데이터 에셋, 물리 컴포넌트 참조를 묶는 통합 지점이다.
- Unity YAML의 GUID/fileID를 직접 변경하면 참조가 조용히 끊길 수 있다.
- `Test Floor`는 Platform 레이어를 사용하므로 `PlayerMoveController`의 지면 판정과 `TagManager.asset` 설정에 의존한다.

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/Animations/*.anim`

- `jump_to_fall`, `land`, `crouch_start`, `crouch_end` 및 공격 클립의 Animation Event가 코드 상태 전환을 진행시킨다.
- 이벤트 이름이나 시점이 변경·삭제되면 상태가 종료되지 않고 고정될 수 있다.
- 스프라이트 참조가 GUID와 fileID로 직렬화되므로 원본 이미지 재임포트나 `.meta` 변경의 영향이 크다.

### 중간

#### `DarkSeries/Assets/Scripts/01.Controllers/State/StateMachine.cs`

- 모든 플레이어 상태 전환의 공통 기반이다.
- 초기 상태 설정 전에 Update 계열 메서드가 호출되면 `currState` null 참조가 발생한다.
- 상태의 Update 또는 입력 콜백 도중 즉시 다른 상태로 전환하므로, 전환 뒤 기존 메서드가 계속 실행되면 같은 프레임에 추가 전환이 발생할 수 있다.

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/States/SwordMaster_BaseState.cs`

- 모든 상태가 공유하는 Input Action 구독·해제 지점이므로 누락이나 비대칭 변경 시 중복 콜백 또는 해제되지 않은 콜백이 발생한다.
- 각 상태 전환 때 다수의 이벤트를 반복해서 연결하므로 상태 전환 로직과 강하게 결합되어 있다.

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/States/SwordMaster_Idle.cs`

- 공중 상태로 전환한 직후 `return`하지 않아 같은 Update에서 이동 입력이 있으면 Walk로 다시 전환할 가능성이 있다.
- 공격, 점프, 앉기 입력 진입점이 한 상태에 집중되어 있다.

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/States/SwordMaster_Walk.cs`

- 공중 상태로 전환한 뒤 메서드를 즉시 종료하지 않는다.
- Run 입력 전환이 없고, `SwordMaster_Run`도 등록되지 않아 Animator의 Run 구성과 코드가 불일치한다.

#### `DarkSeries/Assets/Scripts/01.Controllers/Player/SwordMaster/States/SwordMaster_SlashAttack.cs`

- 입력 버퍼, 콤보 인덱스, Animator 전이, Animation Event에 동시에 의존한다.
- 콤보 범위 `0~3`과 Animator의 네 공격 상태 및 `ComboIndex` 조건이 함께 유지되어야 한다.

#### `DarkSeries/Assets/Scripts/03.Inputs/PlayerActionMap.inputactions`

- 실제 플레이어 입력의 원본이다. 액션 이름 변경은 상태 코드와 생성 래퍼 전체에 영향을 준다.
- 수정 후 `PlayerActionMap.cs`가 다시 생성될 수 있으므로 자동 생성 파일을 직접 수정하면 변경이 덮어써진다.

#### `DarkSeries/Assets/Scripts/03.Inputs/PlayerActionMap.cs`

- Input System이 자동 생성한 대형 래퍼 파일이다.
- 수동 수정 대상이 아니며 원본 `.inputactions` 재생성 시 덮어써진다.

#### `DarkSeries/Assets/InputSystem_Actions.inputactions`

- 프로젝트 템플릿에서 생성된 별도의 대형 Input Actions 에셋으로 보이며, 플레이어가 사용하는 `PlayerActionMap.inputactions`와 공존한다.
- 현재 코드에서 직접 사용하는 흔적은 보이지 않아 향후 어느 입력 에셋이 기준인지 혼동할 위험이 있다. 삭제 여부는 Unity 참조 검색 후 판단해야 한다.

#### `DarkSeries/ProjectSettings/TagManager.asset`

- `PlayerMoveController`가 문자열 `Platform`으로 레이어 마스크를 얻는다.
- 레이어 이름이나 번호 변경은 씬 바닥 오브젝트와 지면 감지를 동시에 깨뜨릴 수 있다.

## 5. 변경 시 확인할 연결 관계

```text
PlayerActionMap.inputactions
  → PlayerActionMap.cs (자동 생성)
  → PlayerInputController
  → SwordMaster_BaseState 및 개별 상태

PlayerData.asset
  → PlayerData.cs
  → Walk / Run / Jump 상태

SwordMaster 상태 코드
  ↔ SwordMasterAnimData의 파라미터 이름
  ↔ SwordMaster.controller의 파라미터와 전이
  ↔ *.anim의 OnAnimationEnd 이벤트
  → SampleScene의 SwordMaster 컴포넌트 참조
```

기능을 추가하거나 상태를 수정할 때는 코드 파일만 보지 말고 입력 에셋, Animator Controller, 관련 Animation Clip, 씬 직렬화 참조를 한 묶음으로 검증해야 한다. `.meta` 파일의 GUID는 임의로 변경하거나 삭제하지 않아야 한다.

## 6. 현재 확인된 구현 공백

- Run 상태 클래스와 Animator 파라미터/상태는 존재하지만 상태 머신 등록과 입력 전환이 연결되지 않았다.
- Enemy 컨트롤러 디렉터리는 존재하지만 구현 스크립트는 없다.
- 자동화된 게임플레이 테스트나 상태 머신 테스트 파일은 확인되지 않았다.
- Unity Editor 실행 및 Play Mode 검증은 이 문서 작성 과정에서 수행하지 않았다.
